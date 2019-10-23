using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTreeSpeaker : MapSpeaker {
    private bool mAnimating = false;
    [SerializeField] public MyBehaviour mLeaf;
    public override bool canReply(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        return !mAnimating;
    }
    public override void speak(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        if (mAnimating) return;
        shake();
    }
    private void shake() {
        mAnimating = true;
        mLeaf.moveBy(new Vector3(0.2f, 0, 0), 0.1f, () => {
            mLeaf.moveBy(new Vector3(-0.4f, 0, 0), 0.2f, () => {
                mLeaf.moveBy(new Vector3(0.2f, 0, 0), 0.1f, () => {
                    mAnimating = false;
                });
            });
        });
    }
}
