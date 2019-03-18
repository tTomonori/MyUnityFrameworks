using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratumBorder : MapBorder{
    [SerializeField] private BorderDirection mBorderDirection;
    private int mStratumNum;
    private void Start(){
        mStratumNum = gameObject.GetComponentInParent<MapStratum>().stratumNum;
    }
    public override void onEnter(MapBorderStepper aStepper, Vector2 aPre, Vector2 aCur){
        correctStratum(aStepper, aPre, aCur);
    }
    public override void onMove(MapBorderStepper aStepper, Vector2 aPre, Vector2 aCur){
        correctStratum(aStepper, aPre, aCur);
    }
    public override void onExit(MapBorderStepper aStepper, Vector2 aPre, Vector2 aCur){
        correctStratum(aStepper, aPre, aCur);
    }
    private void correctStratum(MapBorderStepper aStepper, Vector2 aPre, Vector2 aCur){
        float tP;
        switch(mBorderDirection){
            case BorderDirection.upHigh:
                tP = positionY - 0.5f;
                if(aPre.y < tP){
                    if (tP <= aCur.y) 
                        aStepper.changeStratum(mStratumNum);
                }else{
                    if (aCur.y < tP)
                        aStepper.changeStratum(mStratumNum - 1);
                }
                break;
            case BorderDirection.leftHigh:
                tP = positionX + 0.5f;
                if(tP < aPre.x){
                    if (aCur.x <= tP) aStepper.changeStratum(mStratumNum);
                }else{
                    if (tP < aCur.x) aStepper.changeStratum(mStratumNum - 1);
                }
                break;
            case BorderDirection.rightHigh:
                tP = positionX - 0.5f;
                if(aPre.x < tP){
                    if (tP <= aCur.x) aStepper.changeStratum(mStratumNum);
                }else{
                    if (aCur.x < tP) aStepper.changeStratum(mStratumNum - 1);
                }
                break;
        }
    }
    public enum BorderDirection{
        upHigh,leftHigh,rightHigh
    }
}
