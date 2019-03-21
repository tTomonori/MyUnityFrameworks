using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    private class WalkAroundAi : Ai{
        public WalkAroundAi(MapCharacter aParent,Arg aArg):base(aParent){
            mRangeX = aArg.get<float>("rangeX");
            mRangeY = aArg.get<float>("rangeY");
        }
        private bool mInitialFlag=false;
        private Vector2 mInitialPosition;
        private float mRangeX;
        private float mRangeY;
        private bool mIsMoving = false;
        private Vector2 mMovingDirection;
        private float mMovingTime = 0;
        public override void update(){
            //初期化
            if(!mInitialFlag){
                mInitialFlag = true;
                mInitialPosition = parent.position2D;
            }
            //移動開始
            if(!mIsMoving){
                if (Random.Range(0, 500) > 10) return;
                mIsMoving = true;
                mMovingDirection = DirectionOperator.randomVector();
                mMovingTime = 0;
            }
            //移動終了
            if(Random.Range(1,2)<mMovingTime){
                mIsMoving = false;
            }
            //移動処理
            mMovingTime += Time.deltaTime;
            //現在座標
            Vector2 tCurPosition = parent.position2D;
            //最大移動距離
            Vector2 tMax = new Vector2(
                (mMovingDirection.x < 0) ? (mInitialPosition.x - mRangeX - tCurPosition.x) : (mInitialPosition.x + mRangeX - tCurPosition.x),
                (mMovingDirection.y < 0) ? (mInitialPosition.y - mRangeY - tCurPosition.y) : (mInitialPosition.y + mRangeY - tCurPosition.y)
            );
            move(mMovingDirection, 0.7f, tMax);
            tCurPosition = parent.position2D;
            if(tCurPosition.x <= mInitialPosition.x - mRangeX || mInitialPosition.y + mRangeY <= tCurPosition.y ||
               tCurPosition.y <= mInitialPosition.y - mRangeY || mInitialPosition.y + mRangeY <= tCurPosition.y ){
                mIsMoving = false;
            }
        }
    }
}
