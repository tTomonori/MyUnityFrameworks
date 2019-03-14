using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    private class StandingState : State{
        public StandingState(MapCharacter aParent):base(aParent){}
        public override void enter(){
            parent.mCharaSprite.mOrder = new int[1] { 1 };
            parent.mCharaSprite.mInterval = 0;
            turnAround(parent.direction);
        }
        public override void move(Vector2 aVector, float aSpeed){
            parent.changeState(new WalkingState(parent));
            parent.mState.move(aVector, aSpeed);
        }
        public override bool turnAround(Direction aDirection){
            parent.mCharaSprite.setSprites(parent.mSprites[parent.getSpritesIndex(aDirection)]);
            return true;
        }
    }
}
