using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public class KeyboardAi : Ai {
        public override void update() {
            //移動
            Vector2 tDirectionVector = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow))
                tDirectionVector += new Vector2(0, 1);
            if (Input.GetKey(KeyCode.DownArrow))
                tDirectionVector += new Vector2(0, -1);
            if (Input.GetKey(KeyCode.LeftArrow))
                tDirectionVector += new Vector2(-1, 0);
            if (Input.GetKey(KeyCode.RightArrow))
                tDirectionVector += new Vector2(1, 0);

            if (tDirectionVector != Vector2.zero)
                parent.mState.move(tDirectionVector);

            //話しかける
            if (Input.GetKeyDown(KeyCode.Z))
                parent.mState.speak();
        }
    }
}
