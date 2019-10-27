using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public MapFileData.Npc mFileData;
    /// <summary>移動処理で使うデータ</summary>
    public MovingData mMovingData;
    [SerializeField] public MapCharacterImage mCharacterImage;
    public override MapEntityImage mImage {
        get { return mCharacterImage; }
        set { mCharacterImage = (MapCharacterImage)value; }
    }

    private MapCharacter.Ai mAi;
    private MapCharacter.State mState;

    /// <summary>AI復元する時に必要となるデータ</summary>
    public string saveAi() {
        return mAi.save();
    }
    /// <summary>Stateを復元する時に必要となるデータ</summary>
    public string saveState() {
        return mState.save();
    }

    //<summary>MapWorld内に配置された直後に呼ばれる</summary>
    public override void placed() {
        mMovingData.mPrePosition = mMapPosition;
        mMovingData.mDeltaPrePosition = mMovingData.mPrePosition;
        MapHeightUpdateSystem.initHeight(this);
        MapTriggerUpdater.initTriggerDataOfMovingData(this);
    }
    //状態遷移
    public void transitionState(MapCharacter.State aState) {
        if (mState != null)
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
        return mAi is MapCharacter.PlayerAi || mOriginalAi is MapCharacter.PlayerAi;
    }
    //操作状態
    public Operation getOperation() {
        if (mAi is JackedAi) return Operation.jacked;
        if (mState is StandingState) return Operation.free;
        if (mState is WalkingState) return Operation.free;
        return Operation.busy;
    }
    public enum Operation {
        free, jacked, busy
    }
}
