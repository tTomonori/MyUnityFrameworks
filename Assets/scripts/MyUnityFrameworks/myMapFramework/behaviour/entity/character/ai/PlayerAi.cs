using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    private class PlayerAi : Ai{
        public PlayerAi(MapCharacter aParent):base(aParent){
            mInput = aParent.gameObject.GetComponent<MapPlayerCharacter>();
        }
        private MapPlayerCharacter mInput;
        protected override void update(){
            if (mInput.mMoveDirection != null)
                move(calculateDistance((Vector2)mInput.mMoveDirection, 2));
            if (mInput.mInputA)
                examine();
        }
    }
}
