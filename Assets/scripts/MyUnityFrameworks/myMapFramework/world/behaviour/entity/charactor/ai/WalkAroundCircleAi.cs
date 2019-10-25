using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    /// <summary>
    /// 円形範囲内を歩き回る
    /// </summary>
    public class WalkAroundCircleAi : Ai {
        //移動範囲の半径
        private float mRange;
        //移動範囲の中心
        private MapPosition mCenterPosition;
        private WalkState mWalkState;
        public WalkAroundCircleAi(MyTag aTag) {
            mRange = float.Parse(aTag.mArguments[0]);
            if (aTag.mArguments.Length >= 3) {
                mCenterPosition = new MapPosition(new Vector3(float.Parse(aTag.mArguments[1]), float.Parse(aTag.mArguments[2])));
            }
        }
        public override void update() {
            if (mWalkState == null) {
                if (mCenterPosition == null)
                    mCenterPosition = parent.mMapPosition;
                mWalkState = new StoppingState(this);
            }
            mWalkState.update();
        }
        public override string save() {
            return "<walkAroundCircle," + mRange + "," + mCenterPosition.x + "," + mCenterPosition.y + ">";
        }

        private class WalkState {
            protected WalkAroundCircleAi mParent;
            public virtual void update() { }
        }
        //移動中
        private class WalkingState : WalkState {
            //移動先の座標
            private Vector2 mTargetPosition;
            public WalkingState(WalkAroundCircleAi aParent) {
                mParent = aParent;
                mTargetPosition = mParent.mCenterPosition.vector2 + Random.Range(0, mParent.mRange) * VectorCalculator.randomVector();
            }
            public override void update() {
                if (MapCharacterMoveSystem.arrived(mParent.parent, mTargetPosition)) {
                    //移動先に着いた
                    mParent.mWalkState = new StoppingState(mParent);
                    return;
                }
                //移動方向の決定
                Vector2 tDirection = mTargetPosition - mParent.parent.mMapPosition.vector2;
                mParent.parent.mState.move(tDirection);
            }
        }
        //停止中
        private class StoppingState : WalkState {
            //止まっている時間の最長と最短
            static private float kMaxStoppingTime = 8;
            static private float kMinStoppingTime = 3;
            //移動を開始するまでの止まっている時間
            private float mStoppingTime;
            //止まっていた時間
            private float mStopedTime = 0;
            public StoppingState(WalkAroundCircleAi aParent) {
                mParent = aParent;
                mStoppingTime = Random.Range(kMinStoppingTime, kMaxStoppingTime);
            }
            public override void update() {
                mStopedTime += Time.deltaTime;
                if (mStoppingTime <= mStopedTime) {
                    //移動を始める
                    mParent.mWalkState = new WalkingState(mParent);
                }
            }
        }
    }
}