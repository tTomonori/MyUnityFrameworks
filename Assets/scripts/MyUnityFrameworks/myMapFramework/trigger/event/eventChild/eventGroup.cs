using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapEvent{
    //リスト内のイベントを順番に実行(callbackが呼ばれるまで次のイベントは実行されない)
    public class list : MapEventChild{
        private List<MapEventChild> mList;
        public override void init(Arg aData){
            mList = new List<MapEventChild>();
            foreach(Arg tData in aData.get<List<Arg>>("list")){
                mList.Add(MapEventChild.create(tData));
            }
        }
        public override void run(MapEvent aParent, Action aCallback){
            int tLength = mList.Count;
            Action<int,Action> fRunEventChild = (int aI,Action aC) => {};
            fRunEventChild = (int aI, Action aC) => {
                if(aI==tLength){
                    //イベント全て終了
                    aC();
                    return;
                }
                mList[aI].run(aParent, ()=>{
                    //次のイベントを実行
                    fRunEventChild(aI + 1, aC);
                });
            };
            //リストの先頭のイベント実行
            fRunEventChild(0, aCallback);
        }
    }
    //リスト内のイベントを同時に実行
    public class group:MapEventChild{
        private List<MapEventChild> mGroup;
        public override void init(Arg aData){
            mGroup = new List<MapEventChild>();
            foreach(Arg tData in aData.get<List<Arg>>("group")){
                mGroup.Add(MapEventChild.create(tData));
            }
        }
        public override void run(MapEvent aParent, Action aCallback){
            CallbackSystem tCS = new CallbackSystem();
            foreach(MapEventChild tEvent in mGroup){
                tEvent.run(aParent, tCS.getCounter());
            }
            tCS.then(aCallback);
        }
    }
}
