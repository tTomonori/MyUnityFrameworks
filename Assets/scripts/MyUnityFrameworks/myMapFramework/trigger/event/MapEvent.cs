using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapEvent {
    private Arg mData;
    private MapEventChild mEvent;
    private MapWorld.MapEventOperator mEventOperator;
    //乗っ取ったキャラのAI
    private Dictionary<string, MapCharacter.HijackedAi> mAi = new Dictionary<string, MapCharacter.HijackedAi>();
    //イベント発火と同時に乗っ取るAI
    private List<string> mRequireAi;
    //現在イベント実行中か
    private bool mIsRunning = false;
    public MapEvent(Arg aData,List<string> aRequireAi=null){
        mData = aData;
        mEvent = MapEventChild.create(aData);
        mRequireAi = (aRequireAi == null) ? new List<string>() : aRequireAi;
    }
    public MapEvent(MapEventChild aEvent,List<string> aRequireAi=null){
        mEvent = aEvent;
        mRequireAi = (aRequireAi == null) ? new List<string>() : aRequireAi;
    }
    //イベント実行
    public void run(MapWorld.MapEventOperator aOperator){
        mEventOperator = aOperator;
        //既に実行中
        if (mIsRunning) return;
        //乗っ取り不可能なAIがある
        if (!canHijack()) return;
        mIsRunning = true;
        //AI乗っ取り
        foreach(string tName in mRequireAi){
            mAi[tName] = mEventOperator.getCharacter(tName).hijack();
        }
        //イベント実行
        mEvent.run(this,() => {
            done();
        });
    }
    //イベント実行完了
    public void done(){
        //AIの乗っ取り終了
        foreach(KeyValuePair<string,MapCharacter.HijackedAi> tPair in mAi){
            tPair.Value.endHijack();
        }
        mAi.Clear();
        mIsRunning = false;
    }

    //キャラのAI取得
    protected MapCharacter.HijackedAi getAi(string aName){
        if(!mAi.ContainsKey(aName)||mAi[aName]==null)
            mAi[aName] = mEventOperator.getCharacter(aName).hijack();
        return mAi[aName];
    }
    //必要なキャラのAIを乗っ取りできるか
    private bool canHijack(){
        foreach (string tName in mRequireAi){
            if (!mEventOperator.getCharacter(tName).isCanHijack()) return false;
        }
        return true;
    }

    //////////
    //一つのイベントクラス
    public abstract class MapEventChild{
        public abstract void init(Arg aData);
        public abstract void run(MapEvent aParent, Action aCallback);
        //データからイベント生成
        static public MapEventChild create(Arg aData){
            Type tType = Type.GetType("MapEvent+"+aData.get<string>("event"));
            if (tType == null) tType = typeof(outerEvent);
            MapEventChild tEvent = (MapEventChild)Activator.CreateInstance(tType);
            //MapEventChild tEvent = (MapEventChild)tType.GetConstructor(new Type[0]).Invoke(new object[0]);
            tEvent.init(aData);
            return tEvent;
        }
    }
    //関数を実行イベント
    public class function:MapEventChild{
        private Action<Action> mFunction;
        public function(Action<Action> aFunction){
            mFunction = aFunction;
        }
        public override void init(Arg aData){
            mFunction = aData.get<Action<Action>>("function");
        }
        public override void run(MapEvent aParent, Action aCallback){
            mFunction(aCallback);
        }
    }
    //外部通知イベント
    public class outerEvent:MapEventChild{
        private Arg mData;
        private Dictionary<string, MapEventChild> mBranch;
        public override void init(Arg aData){
            mData = aData;
            if (!mData.ContainsKey("branch")) return;
            //外部イベントからの戻り値により分岐する場合のイベント
            mBranch = new Dictionary<string, MapEventChild>();
            foreach(KeyValuePair<string,Arg> tPair in mData.get<Arg>("branch").dictionary){
                mBranch.Add(tPair.Key, MapEventChild.create(tPair.Value));
            }
        }
        public override void run(MapEvent aParent, Action aCallback){
            MyMap.mEventHandler.onFireOuterEvent(mData, (aRes) =>{
                if(mBranch==null || !mBranch.ContainsKey(aRes)){
                    if (aRes != "") Debug.Log("MapEvent : 「" + mData.get<string>("event") + "」イベントの戻り値「" + aRes + "」に対応したイベントがないよ");
                    aCallback();
                    return;
                }
                mBranch[aRes].run(aParent, aCallback);
            });
        }
    }
}