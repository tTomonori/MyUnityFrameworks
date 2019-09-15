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
        tCharacter.mImage.setDirection(new Vector2(1, 0));
        //ai


        return tCharacter;
    }
    //<summary>キャラクターを生成してworldに追加</summary>
    static private void buildCharacter(MapFileData.Npc aData) {
        MapCharacter tCharacter = createCharacter(aData);
        mWorld.addCharacter(tCharacter, aData.mName, aData.mX, aData.mY, aData.mStratum);
    }
}
