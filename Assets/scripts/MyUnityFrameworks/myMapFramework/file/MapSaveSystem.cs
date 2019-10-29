using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapSaveSystem {
    /// <summary>マップの状態を保存</summary>
    static public MapSaveFileData save(MyMap aMap) {
        MapSaveFileData tSaveData = new MapSaveFileData();

        //マップ名
        tSaveData.mMapName = aMap.mWorld.mFileData.mMapName;
        //ファイルパス
        tSaveData.mFilePath = aMap.mWorld.mMapPath;
        //エンカウント
        tSaveData.mEncountCount = aMap.mEncountSystem.mCount;
        //stratum
        tSaveData.mStratums = aMap.mWorld.mFileData.mStratums;
        //chip
        tSaveData.mChip = aMap.mWorld.mFileData.mChip;
        //shadow
        tSaveData.mShadows = aMap.mWorld.mFileData.mShadows;
        //trigger
        tSaveData.mTriggers = aMap.mWorld.mFileData.mTriggers;
        //event
        tSaveData.mEvents = aMap.mWorld.mFileData.mEvents;
        //entrance
        tSaveData.mEntrances = aMap.mWorld.mFileData.mEntrances;
        //ornament
        foreach (MapOrnament tOrnament in aMap.mWorld.mOrnamentContainer.GetComponentsInChildren<MapOrnament>()) {
            tSaveData.mOrnaments.Add(saveOrnament(tOrnament));
        }
        //character(player含む)
        foreach (MapCharacter tCharacter in aMap.mWorld.mCharacterContainer.GetComponentsInChildren<MapCharacter>()) {
            tSaveData.mCharacters.Add(saveCharacter(tCharacter));
        }

        return tSaveData;
    }
    /// <summary>ornamentの保存データ生成</summary>
    static public MapSaveFileData.Ornament saveOrnament(MapOrnament aOrnament) {
        MapSaveFileData.Ornament tSaveData = new MapSaveFileData.Ornament(aOrnament.mFileData);
        //座標
        MapPosition tPosition = aOrnament.mMapPosition;
        tSaveData.mX = tPosition.x;
        tSaveData.mY = tPosition.y;
        tSaveData.mHeight = tPosition.h;
        //その他データ
        tSaveData.mArg = aOrnament.save();

        return tSaveData;
    }
    /// <summary>characterの保存データ生成</summary>
    static public MapSaveFileData.Character saveCharacter(MapCharacter aCharacter) {
        MapSaveFileData.Character tSaveData = new MapSaveFileData.Character(aCharacter.mFileData);
        //座標
        MapPosition tPosition = aCharacter.mMapPosition;
        tSaveData.mX = tPosition.x;
        tSaveData.mY = tPosition.y;
        tSaveData.mHeight = tPosition.h;
        //向き
        tSaveData.mDirection = aCharacter.mCharacterImage.getDirection();
        //ai
        tSaveData.mAiString = aCharacter.saveAi();
        //state
        tSaveData.mStateString = aCharacter.saveState();

        return tSaveData;
    }
}
