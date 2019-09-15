using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public MovingData mMovingData;
    private MapCharacter.Ai mAi;
    private MapCharacter.State mState;
    public MapCharacterImage mImage;

    //状態遷移
    private void transitionState(MapCharacter.State aState) {
        mState.exit();
        mState = aState;
        mState.enter();
    }
}
