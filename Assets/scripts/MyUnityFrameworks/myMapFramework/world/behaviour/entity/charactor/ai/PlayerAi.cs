using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public class PlayerAi : Ai {
        private MyMapController mController;
        public override void update() {
            if (mController == null) {
                mController = parent.GetComponentInParent<MyMap>().mController;
            }
            //移動
            if (mController.mInputVector != null) {
                Vector2 tInputVector = (Vector2)mController.mInputVector;
                parent.mState.move(new Vector3(tInputVector.x, 0, tInputVector.y), tInputVector.magnitude);
            }
            //話しかける
            if (mController.mInputA)
                parent.mState.speak();
        }
        public override string save() {
            return "<player>";
        }
    }
}
