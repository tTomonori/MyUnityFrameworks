using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratumBorder : MapTrigger{
    [SerializeField] private BorderDirection mBorderDirection;
    private int mStratumNum;
    public override MapWalker.PassType confirmPassType(MapWalker aBehaviour,Vector2 aPosition){
        return MapWalker.PassType.through;
    }
    private void Start(){
        mStratumNum = gameObject.GetComponentInParent<MapStratum>().stratumNum;
    }
    public override void onEnter(MapStepper aStepper){
        correctStratum(aStepper);
    }
    public override void onMove(MapStepper aStepper){
        correctStratum(aStepper);
    }
    public override void onExit(MapStepper aStepper){
        correctStratum(aStepper);
    }
    private void correctStratum(MapStepper aStepper){
        float tP;
        switch(mBorderDirection){
            case BorderDirection.upHigh:
                tP = positionY - 0.5f;
                if(aStepper.preStepPosition.y < tP){
                    if (tP <= aStepper.curPosition.y) 
                        aStepper.changeStratum(mStratumNum);
                }else{
                    if (aStepper.curPosition.y < tP)
                        aStepper.changeStratum(mStratumNum - 1);
                }
                break;
            case BorderDirection.leftHigh:
                tP = positionX + 0.5f;
                if(tP < aStepper.preStepPosition.x){
                    if (aStepper.curPosition.x <= tP) aStepper.changeStratum(mStratumNum);
                }else{
                    if (tP < aStepper.curPosition.x) aStepper.changeStratum(mStratumNum - 1);
                }
                break;
            case BorderDirection.rightHigh:
                tP = positionX - 0.5f;
                if(aStepper.preStepPosition.x < tP){
                    if (tP <= aStepper.curPosition.x) aStepper.changeStratum(mStratumNum);
                }else{
                    if (aStepper.curPosition.x < tP) aStepper.changeStratum(mStratumNum - 1);
                }
                break;
        }
    }
    public enum BorderDirection{
        upHigh,leftHigh,rightHigh
    }
}
