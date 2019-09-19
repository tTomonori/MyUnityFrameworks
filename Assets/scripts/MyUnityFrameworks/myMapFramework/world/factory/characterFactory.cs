using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    //<summary>キャラクター生成</summary>
    static public MapCharacter createCharacter(MapFileData.Npc aData) {
        MapCharacter tCharacter = MyBehaviour.create<MapCharacter>();
        tCharacter.mImage = MyBehaviour.createObjectFromResources<MapCharacterImage>(MyMap.mMapResourcesDirectory + "/character/" + aData.mPath);
        tCharacter.mImage.transform.SetParent(tCharacter.transform, false);

        //向き
        tCharacter.mImage.setDirection(aData.mDirection);
        //ai
        tCharacter.setAi(createAi(aData.mAi));
        tCharacter.setAi(new MapCharacter.PlayerAi());
        //state
        tCharacter.transitionState(new MapCharacter.StandingState());
        //collider
        BoxCollider2D tBox = tCharacter.gameObject.AddComponent<BoxCollider2D>();
        tBox.size = new Vector2(0.6f, 0.3f);
        tBox.offset = new Vector2(0, 0.15f);
        //attribute
        EntityPhysicsAttribute tAttribute = tCharacter.gameObject.AddComponent<EntityPhysicsAttribute>();
        tAttribute.mAttribute = EntityPhysicsAttribute.Attribute.walking;
        //movingData
        tCharacter.mMovingData = new MovingData();
        tCharacter.mMovingData.mSpeed = 1.5f;
        tCharacter.mMovingData.mDeltaDistance = 0.3f;

        return tCharacter;
    }
    //<summary>キャラのAIを生成</summary>
    static public MapCharacter.Ai createAi(MyTag aAiData) {
        switch (aAiData.mTagName) {
            case "walkAroundCircle"://円形範囲内を歩き回る
                return new MapCharacter.WalkAroundCircleAi(float.Parse(aAiData.mArguments[0]));
        }
        throw new System.Exception("MapWorldFactory-CharactorFactory : 不正なAI名「" + aAiData.mTagName + "」");
    }
    //<summary>キャラクターを生成してworldに追加</summary>
    static private void buildCharacter(MapFileData.Npc aData) {
        MapCharacter tCharacter = createCharacter(aData);
        mWorld.addCharacter(tCharacter, aData.mName, aData.mX, aData.mY, aData.mStratum);
    }
}
