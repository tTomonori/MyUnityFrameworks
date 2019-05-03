using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountTrigger : MapTrigger {
    public static Vector2 mLastCount = new Vector2(-1, -1);
    public float mEncountMagnification = 1;
    public Arg mEncountData;
    public override MapWalker.PassType confirmPassType(MapWalker aBehaviour,Vector2 aPosition){
        if (!canEncount(aBehaviour)) return MapWalker.PassType.through;

        //移動距離
        float tDistance = Vector2.Distance(aBehaviour.position2D, aPosition);
        return MyMapEncountSystem.simulate(tDistance * mEncountMagnification) ? MapWalker.PassType.stop : MapWalker.PassType.through;
    }
    public override void onMove(MapStepper aStepper){
        count(aStepper);
    }
    public override void onEnter(MapStepper aStepper){
        count(aStepper);
    }
    public override void onExit(MapStepper aStepper){
        mLastCount = aStepper.curPosition;
    }
    //エンカウント対象でありエンカウント可能である
    private bool canEncount(MapWalker aWalker){
        //playerでない
        if (aWalker.name != "player") return false;
        //イベント中
        if (!((MapCharacter)aWalker.cEntity).isCanHijack()) return false;
        return true;
    }
    private void count(MapStepper aStepper){
        //エンカウントしないキャラor状態
        if (!canEncount(aStepper.cWalker)) return;
        //既にカウントした
        if (mLastCount.x == aStepper.curPosition.x && mLastCount.y == aStepper.curPosition.y) return;

        mLastCount = aStepper.curPosition;
        //移動距離
        float tDistance = Vector2.Distance(aStepper.curPosition,aStepper.preStepPosition);
        if (!MyMapEncountSystem.count(tDistance * mEncountMagnification)) return;

        //エンカウントした
        MapEvent.battle tBattle = new MapEvent.battle();
        tBattle.init(new Arg(new Dictionary<string, object>(){
            {"data",mEncountData}
        }));
        MapEvent tEvent = new MapEvent(tBattle, new List<string>() { "player" });
        mMapWorld.mEventOperator.addEvent(tEvent);
    }
}
