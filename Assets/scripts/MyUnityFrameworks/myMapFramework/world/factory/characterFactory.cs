using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    ///<summary>キャラクター生成</summary>
    static public MapCharacter createCharacter(MapFileData.Character aData) {
        MapCharacter tCharacter = MyBehaviour.createObjectFromResources<MapCharacter>(MyMap.mMapResourcesDirectory + "/character/" + aData.mPath);
        //名前
        tCharacter.mName = aData.mName;
        tCharacter.name = "character:" + aData.mName;
        //向き
        tCharacter.mCharacterRenderBehaviour.mImage.setDirection(aData.mDirection);
        //ai
        tCharacter.setAi(createAi(aData.mAi));
        //state
        tCharacter.transitionState(createState(aData.mState));
        //movingData
        tCharacter.mMovingData = new MovingData();
        tCharacter.mMovingData.mSpeed = aData.mMoveSpeed;
        Vector3 tColliderSize = tCharacter.mEntityPhysicsBehaviour.mAttriubteCollider.minimumCircumscribedCube();
        tCharacter.mMovingData.mDeltaDistance = Mathf.Min(tColliderSize.x, tColliderSize.z);
        //speaker
        if (aData.mIsSpeaker) {
            MapKeyEventSpeaker tSpeaker = tCharacter.mEntityPhysicsBehaviour.mAttriubteCollider.gameObject.AddComponent<MapKeyEventSpeaker>();
            tSpeaker.mBehaviour = tCharacter;
            tSpeaker.mSpeakDefault = aData.mSpeakDefault;
            tSpeaker.mSpeakFromUp = aData.mSpeakFromUp;
            tSpeaker.mSpeakFromDown = aData.mSpeakFromDown;
            tSpeaker.mSpeakFromLeft = aData.mSpeakFromLeft;
            tSpeaker.mSpeakFromRight = aData.mSpeakFromRight;
        }

        return tCharacter;
    }
    ///<summary>キャラのAIを生成</summary>
    static public MapCharacter.Ai createAi(MyTag aAiData) {
        switch (aAiData.mTagName) {
            case "walkAroundCircle"://円形範囲内を歩き回る
                return new MapCharacter.WalkAroundCircleAi(aAiData);
            case "player"://プレイヤー操作
                return new MapCharacter.PlayerAi();
            case "keyboard"://キーボード操作
                return new MapCharacter.KeyboardAi();
        }
        throw new System.Exception("MapWorldFactory-CharactorFactory : 不正なAI名「" + aAiData.mTagName + "」");
    }
    static public MapCharacter.State createState(MyTag aStateTag) {
        //未設定なら棒立ち状態を返す
        if (aStateTag == null) return new MapCharacter.StandingState();
        switch (aStateTag.mTagName) {
            case "kari":
                break;
        }
        return new MapCharacter.StandingState();
    }
    ///<summary>キャラクターを生成してworldに追加</summary>
    static private MapCharacter buildCharacter(MapFileData.Character aData) {
        //生成フラグ確認
        if (!flagCreate(aData)) return null;

        MapCharacter tCharacter = createCharacter(aData);
        tCharacter.mFileData = aData;
        tCharacter.transform.SetParent(mWorld.mCharacterContainer.transform, false);
        tCharacter.mMapPosition = new MapPosition(aData.mPosition);
        tCharacter.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(aData.mY)]);
        mWorld.mCharacters.Add(tCharacter);
        return tCharacter;
    }
    /// <summary>
    /// キャラクターを生成して追加
    /// </summary>
    /// <param name="aCharacterData">追加するキャラクターのデータ</param>
    /// <param name="aWorld">キャラクターを追加するworld</param>
    static public void addCharacter(MapFileData.Character aCharacterData, MapWorld aWorld) {
        mWorld = aWorld;
        buildCharacter(aCharacterData).placed();
        mWorld = null;
    }
}
