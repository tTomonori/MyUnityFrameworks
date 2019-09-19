using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public MovingData mMovingData;
    private MapCharacter.Ai mAi;
    private MapCharacter.State mState;
    public MapCharacterImage mImage;

    //<summary>MapWorld内に配置された直後に呼ばれる</summary>
    public override void placed() {
        mMovingData.mPrePosition = mMapPosition;
    }
    //状態遷移
    public void transitionState(MapCharacter.State aState) {
        if(mState!=null)
            mState.exit();
        aState.parent = this;
        mState = aState;
        mState.enter();
    }
    //Ai設定
    public void setAi(MapCharacter.Ai aAi) {
        aAi.parent = this;
        mAi = aAi;
    }

    //更新
    public void updateInternalState() {
        mAi.update();
        mState.update();
    }
    //プレイヤーが操作するキャラかどうか
    public bool isPlayer() {
        return mAi is MapCharacter.PlayerAi;
    }
}
