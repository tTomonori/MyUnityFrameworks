using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    private class PlayerAi : Ai{
        public PlayerAi(MapCharacter aParent):base(aParent){
            mInput = aParent.gameObject.GetComponent<MapPlayerCharacter>();
        }
        private MapPlayerCharacter mInput;
        public override void update(){
            if (mInput.mMoveDirection != null)
                parent.mState.move((Vector2)mInput.mMoveDirection, 2);
        }
    }
}
