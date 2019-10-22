using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    //<summary>キャラクター生成</summary>
    static public MapCharacter createCharacter(MapFileData.Npc aData) {
        MapCharacter tCharacter = MyBehaviour.createObjectFromResources<MapCharacter>(MyMap.mMapResourcesDirectory + "/character/" + aData.mPath);
        //名前
        tCharacter.mName = aData.mName;
        tCharacter.name = "character:" + aData.mName;
        //向き
        tCharacter.mCharacterImage.setDirection(aData.mDirection);
        //ai
        tCharacter.setAi(createAi(aData.mAi));
        //state
        tCharacter.transitionState(new MapCharacter.StandingState());
        //movingData
        tCharacter.mMovingData = new MovingData();
        tCharacter.mMovingData.mSpeed = 2.5f;
        tCharacter.mMovingData.mDeltaDistance = 0.3f;


        return tCharacter;
    }
    //<summary>キャラのAIを生成</summary>
    static public MapCharacter.Ai createAi(MyTag aAiData) {
        switch (aAiData.mTagName) {
            case "walkAroundCircle"://円形範囲内を歩き回る
                return new MapCharacter.WalkAroundCircleAi(float.Parse(aAiData.mArguments[0]));
            case "player"://プレイヤー操作
                return new MapCharacter.PlayerAi();
            case "keyboard"://キーボード操作
                return new MapCharacter.KeyboardAi();
        }
        throw new System.Exception("MapWorldFactory-CharactorFactory : 不正なAI名「" + aAiData.mTagName + "」");
    }
    //<summary>キャラクターを生成してworldに追加</summary>
    static private void buildCharacter(MapFileData.Npc aData) {
        MapCharacter tCharacter = createCharacter(aData);
        tCharacter.transform.SetParent(mWorld.mCharacterContainer.transform, false);
        tCharacter.setMapPosition(new Vector2(aData.mX, aData.mY), aData.mHeight);
        tCharacter.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(tCharacter.mHeight)]);
        mWorld.mCharacters.Add(tCharacter);
    }
}
