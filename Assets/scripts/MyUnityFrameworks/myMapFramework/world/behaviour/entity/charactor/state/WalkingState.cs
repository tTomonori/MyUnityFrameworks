using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    /// <summary>
    /// 移動中の状態
    /// </summary>
    public class WalkingState : State {
        public WalkingState() {

        }
        public override void update() {
            if (parent.mMovingData.mDirection == Vector2.zero) {
                //移動しなかった
                //棒立ち状態に遷移
                parent.mImage.moved(Vector2.zero);
                parent.transitionState(new StandingState());
            }
        }
        public override bool move(Vector2 aVector, float aMaxMoveDistance = float.PositiveInfinity) {
            parent.mMovingData.mDirection = aVector;
            parent.mMovingData.mMaxMoveDistance = aMaxMoveDistance;
            parent.mImage.moved(aVector);
            return true;
        }
    }
}

