using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapSaveSystem {
    /// <summary>マップの状態を保存</summary>
    static public MapSaveFileData save(MyMap aMap) {
        MapSaveFileData tSaveData = new MapSaveFileData();

        //ファイルパス
        tSaveData.mFilePath = aMap.mWorld.mMapPath;
        //エンカウント
        tSaveData.mEncountCount = aMap.mEncountSystem.mCount;
        //ornament
        foreach(MapOrnament tOrnament in aMap.mWorld.mOrnamentContainer.GetComponentsInChildren<MapOrnament>()) {
            tSaveData.mOrnaments.Add(saveOrnament(tOrnament));
        }
        //character(player含む)
        foreach (MapCharacter tCharacter in aMap.mWorld.mCharacterContainer.GetComponentsInChildren<MapCharacter>()) {
            tSaveData.mNpcs.Add(saveNpc(tCharacter));
        }

        return tSaveData;
    }
    /// <summary>ornamentの保存データ生成</summary>
    static public MapSaveFileData.SavedOrnament saveOrnament(MapOrnament aOrnament) {
        MapSaveFileData.SavedOrnament tSaveData = new MapSaveFileData.SavedOrnament(aOrnament.mFileData);
        //座標
        MapPosition tPosition = aOrnament.mMapPosition;
        tSaveData.mX = tPosition.x;
        tSaveData.mY = tPosition.y;
        tSaveData.mHeight = tPosition.h;
        //その他データ
        tSaveData.mSave = aOrnament.save();

        return tSaveData;
    }
    /// <summary>characterの保存データ生成</summary>
    static public MapSaveFileData.SavedNpc saveNpc(MapCharacter aCharacter) {
        MapSaveFileData.SavedNpc tSaveData = new MapSaveFileData.SavedNpc(aCharacter.mFileData);
        //座標
        MapPosition tPosition = aCharacter.mMapPosition;
        tSaveData.mX = tPosition.x;
        tSaveData.mY = tPosition.y;
        tSaveData.mHeight = tPosition.h;
        //向き
        tSaveData.mDirection = aCharacter.mMovingData.mLastDirection;
        //ai
        tSaveData.mAiString = aCharacter.saveAi();
        //state
        tSaveData.mStateString = aCharacter.saveState();

        return tSaveData;
    }
}
