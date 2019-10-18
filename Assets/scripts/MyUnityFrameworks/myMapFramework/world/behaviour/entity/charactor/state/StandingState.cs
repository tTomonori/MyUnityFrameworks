using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    /// <summary>
    /// 棒立ち状態
    /// </summary>
    public class StandingState : State {
        public StandingState() {

        }
        public override void update() {

        }
        public override bool move(Vector2 aVector, float aMaxMoveDistance = float.PositiveInfinity) {
            //移動状態に遷移
            parent.transitionState(new WalkingState());
            return parent.mState.move(aVector, aMaxMoveDistance);
        }
        public override bool speak() {
            parent.mMovingData.mSpeak = true;
            return true;
        }
    }
}

