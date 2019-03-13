using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapBehaviour {
    private class PlayerAi : Ai{
        public PlayerAi(MapCharacter aParent):base(aParent){
        }
        public override void update(){
            float tX = 0;
            float tY = 0;
            if (Input.GetKey(KeyCode.UpArrow))
                tY += 1;
            if (Input.GetKey(KeyCode.DownArrow))
                tY -= 1;
            if (Input.GetKey(KeyCode.LeftArrow))
                tX -= 1;
            if (Input.GetKey(KeyCode.RightArrow))
                tX += 1;
            if (tX != 0 || tY != 0)
                parent.mState.move(new Vector2(tX, tY),2);
        }
    }
}
