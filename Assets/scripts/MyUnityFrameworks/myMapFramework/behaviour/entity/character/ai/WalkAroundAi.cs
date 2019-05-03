using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    private class WalkAroundAi : Ai{
        public WalkAroundAi(MapCharacter aParent,Arg aArg):base(aParent){
            mRangeX = aArg.get<float>("rangeX");
            mRangeY = aArg.get<float>("rangeY");
        }
        private float mRangeX;
        private float mRangeY;
        private Vector2? mInitialPosition;
        public override void start(){
            mInitialPosition = parent.position2D;
            Action tWait = () => { };
            tWait = () =>{
                if (UnityEngine.Random.Range(0, 500) > 10) return;
                removeMiniRoutine(tWait);
                //移動開始
                //移動さきの初期位置からの距離
                Vector2 tTarget = new Vector2(UnityEngine.Random.Range(-mRangeX / 2, mRangeX / 2), UnityEngine.Random.Range(-mRangeY / 2, mRangeY / 2));
                //現在地の初期位置からの距離
                Vector2 tCur = parent.position2D - (Vector2)mInitialPosition;
                addMoveByRoutine(tTarget - tCur, 0.7f, () =>{
                    addMiniRoutine(tWait);
                });
            };
            addMiniRoutine(tWait);
        }
    }
}
