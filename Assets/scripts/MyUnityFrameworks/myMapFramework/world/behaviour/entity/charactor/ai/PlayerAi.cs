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
            if (mController.mInputVector != null)
                parent.mState.move((Vector2)mController.mInputVector,((Vector2)mController.mInputVector).magnitude);
            //話しかける
            if (mController.mInputA)
                parent.mState.speak();
        }
    }
}
