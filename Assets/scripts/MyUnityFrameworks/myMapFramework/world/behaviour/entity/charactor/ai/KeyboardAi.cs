using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public class KeyboardAi : Ai {
        public override void update() {
            //移動
            Vector3 tDirectionVector = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
                tDirectionVector += new Vector3(0, 0, 1);
            if (Input.GetKey(KeyCode.DownArrow))
                tDirectionVector += new Vector3(0, 0, -1);
            if (Input.GetKey(KeyCode.LeftArrow))
                tDirectionVector += new Vector3(-1, 0, 0);
            if (Input.GetKey(KeyCode.RightArrow))
                tDirectionVector += new Vector3(1, 0, 0);

            if (tDirectionVector != Vector3.zero)
                parent.mState.move(tDirectionVector);

            //話しかける
            if (Input.GetKeyDown(KeyCode.Z))
                parent.mState.speak();
        }
        public override string save() {
            return "<keyboard>";
        }
    }
}
