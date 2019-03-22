using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    private class JumpingState : State{
        public JumpingState(MapCharacter aParent,Direction aDirection,float aDistance):base(aParent){
            parent.direction = aDirection;
            mDistance = Mathf.Abs(aDistance) + parent.mCollider.size[(int)DirectionOperator.convertToAxis(aDirection)];
            mTOSystem = new TakeOutFloatSystem(mDistance, mDistance);
        }
        private float mDistance;
        private TakeOutFloatSystem mTOSystem;
        public override void enter(){
            parent.mCharaSprite.mInterval = 0.4f;
            parent.mImageAnimator.jump(0.3f, 1);
        }
        public override void update(){
            Vector2 tV = DirectionOperator.convertToVector(parent.direction) * mTOSystem.takeOut();
            parent.mWalker.foceMoveBy(tV);
            if(mTOSystem.isEmpty){
                parent.changeState(new StandingState(parent));
            }
        }
    }
}
