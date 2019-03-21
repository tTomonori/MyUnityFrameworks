using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MapTrigger {
    private MapEvent mMapEvent;
    private TriggerConfig mTriggerConfig;
    public void set(Arg aTriggerData,Arg aEventData){
        mTriggerConfig = new TriggerConfig(aTriggerData);
        mMapEvent = new MapEvent(aEventData, mTriggerConfig.mRequireAi);
    }
    public override MapWalker.PassType confirmPassType(MapBehaviour aBehaviour){
        //behaviourなし
        if (aBehaviour == null) return MapWalker.PassType.through;
        //characterではない
        if (!(aBehaviour is MapCharacter)) return MapWalker.PassType.through;
        //characterの場合
        //対象のキャラでない
        if (!isTarget(aBehaviour)) return MapWalker.PassType.through;
        //hijack不可
        if (!((MapCharacter)aBehaviour).isCanHijack()) return MapWalker.PassType.through;

        return MapWalker.PassType.stop;
    }
    public override void onEnter(MapStepper aStepper){
        //イベント発火対象でない
        if (!isTarget(aStepper)) return;
        mMapEvent.run(mMapWorld.mEventOperator);
    }
    //イベント発火のトリガーとなるキャラかどうか
    private bool isTarget(MapBehaviour aBehaviour){
        foreach(Arg tConditions in mTriggerConfig.mTriggerTarget){
            if (tConditions.ContainsKey("name") && tConditions.get<string>("name") != aBehaviour.name) continue;
            return true;
        }
        return false;
    }
    private class TriggerConfig{
        //トリガーとなるキャラが通過する時の通過タイプ
        public MapWalker.PassType mPassType = MapWalker.PassType.stop;
        //イベント発火のトリガーとなるキャラの条件
        public List<Arg> mTriggerTarget = new List<Arg>();
        //イベント発火時にhijackするキャラ
        public List<string> mRequireAi = new List<string>();
        public TriggerConfig(Arg aData){
            if (aData.ContainsKey("prototype")){
                switch (aData.get<string>("prototype")){
                    case "player":
                        mPassType = MapWalker.PassType.stop;
                        mTriggerTarget.Add(new Arg(new Dictionary<string, object>(){
                            {"name","player"}
                        }));
                        mRequireAi.Add("player");
                        break;
                }
            }
        }
    }
}
