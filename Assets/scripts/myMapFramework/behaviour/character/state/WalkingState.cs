using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapBehaviour {
    private class WalkingState : State{
        public WalkingState(MapCharacter aParent):base(aParent){}
        private bool mIsMoved = false;
        public override void enter(){
            parent.mCharaSprite.mOrder = new int[4] { 0, 1, 2, 1 };
            parent.mCharaSprite.mInterval = 0.2f;
            turnAround(parent.direction);
        }
        public override void move(Vector2 aVector, float aSpeed){
            parent.direction = DirectionOperator.convertToDirection(aVector);
            parent.mWalker.move(aVector, aSpeed);
            mIsMoved = true;
        }
        public override void update(){
            if(mIsMoved){
                //移動した
                mIsMoved = false;
                return;
            }
            //移動しなかった
            parent.changeState(new StandingState(parent));
        }
        public override bool turnAround(Direction aDirection){
            parent.mCharaSprite.setSprites(parent.mSprites[parent.getSpritesIndex(aDirection)]);
            return true;
        }
    }
}
