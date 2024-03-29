﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public static partial class MapWorldFactory {
    //生成に使っているマップデータ
    static private MapFileData mData;
    //生成中のMapWorld
    static private MapWorld mWorld;
    //<summary>ワールドを作成</summary>
    static public MapWorld create(string aFilePath, MyMap aMap) {
        MapFileData tData = new MapFileData(aFilePath);

        //マップデータを記憶
        mData = tData;
        mWorld = initWorld(new Vector3Int(mData.mStratums[0].mFeild[0].Count, mData.mStratums.Count, mData.mStratums[0].mFeild.Count));
        mWorld.mMap = aMap;
        mWorld.mMapName = mData.mMapName;
        mWorld.mFileData = mData;

        //マップファイルへのパス
        mWorld.mMapPath = aFilePath;

        //生成
        createFromFileData();

        //生成完了
        foreach (MapBehaviour tBehaviour in mWorld.GetComponentsInChildren<MapBehaviour>())
            tBehaviour.placed();

        MapWorld tCreatedWorld = mWorld;
        mWorld = null;
        mData = null;
        return tCreatedWorld;
    }
    //<summary>セーブデータからワールドを作成</summary>
    static public MapWorld createFromSave(string aFilePath, MyMap aMap) {
        MapSaveFileData tSaveData = new MapSaveFileData(aFilePath);

        //マップデータを記憶
        mData = tSaveData;
        mWorld = initWorld(new Vector3Int(mData.mStratums[0].mFeild[0].Count, mData.mStratums.Count, mData.mStratums[0].mFeild.Count));
        mWorld.mMap = aMap;
        mWorld.mMapName = mData.mMapName;
        mWorld.mFileData = mData;
        mWorld.mSaveData = tSaveData;

        //マップファイルへのパス
        mWorld.mMapPath = tSaveData.mFilePath;

        //生成
        createFromFileData();

        //生成完了
        foreach (MapBehaviour tBehaviour in mWorld.GetComponentsInChildren<MapBehaviour>())
            tBehaviour.placed();

        MapWorld tCreatedWorld = mWorld;
        mWorld = null;
        mData = null;
        return tCreatedWorld;
    }
    static private void createFromFileData() {
        //カメラ生成
        initCamera();
        //フィールド生成
        buildField();
        //壁生成
        buildEnd();
        //影生成
        List<MapFileData.Shadow> tShadowData = mData.mShadows;
        int tShadowNum = tShadowData.Count;
        for (int i = 0; i < tShadowNum; ++i) {
            //buildShadow(tShadowData[i]);
        }
        //ornament生成
        List<MapFileData.Ornament> tOrnamentData = mData.mOrnaments;
        int tOrnamentNum = tOrnamentData.Count;
        for (int i = 0; i < tOrnamentNum; ++i) {
            buildOrnament(tOrnamentData[i]);
        }
        //character(npc)生成
        List<MapFileData.Character> tCharacterData = mData.mCharacters;
        int tCharacterNum = tCharacterData.Count;
        for (int i = 0; i < tCharacterNum; ++i) {
            buildCharacter(tCharacterData[i]);
        }
        //trigger生成
        List<MapFileData.Trigger> tTriggerData = mData.mTriggers;
        int tTriggerNum = tTriggerData.Count;
        for (int i = 0; i < tTriggerNum; ++i) {
            buildTrigger(tTriggerData[i]);
        }
        //event
        createEvent();

        //生成完了
        foreach (MapBehaviour tBehaviour in mWorld.GetComponentsInChildren<MapBehaviour>())
            tBehaviour.placed();

    }
    /// <summary>worldを生成しコンテナを追加</summary>
    static private MapWorld initWorld(Vector3Int aSize) {
        MapWorld tWorld = MyBehaviour.create<MapWorld>();
        tWorld.name = "world";
        tWorld.mSize = aSize;
        //tWorld.gameObject.AddComponent<SortingGroup>();
        //カメラ
        tWorld.mCameraContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mCameraContainer.name = "cameraContainer";
        tWorld.mCameraContainer.transform.SetParent(tWorld.transform, false);
        //フィールド
        tWorld.mField = MyBehaviour.create<MyBehaviour>();
        tWorld.mField.name = "field";
        tWorld.mField.transform.SetParent(tWorld.transform, false);
        //階層
        tWorld.mStratums = new MapStratum[aSize.z];
        //マス
        tWorld.mCells = new MapCell[aSize.x, aSize.y, aSize.z];
        //マップ周りの壁壁
        tWorld.mEndContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mEndContainer.name = "endContainer";
        tWorld.mEndContainer.transform.SetParent(tWorld.transform, false);
        //character
        tWorld.mCharacterContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mCharacterContainer.name = "characterContainer";
        tWorld.mCharacterContainer.transform.SetParent(tWorld.transform, false);
        //ornament
        tWorld.mOrnamentContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mOrnamentContainer.name = "ornamentContainer";
        tWorld.mOrnamentContainer.transform.SetParent(tWorld.transform, false);
        //entityInCell
        tWorld.mEntityInCellContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mEntityInCellContainer.name = "entityInCellContainer";
        tWorld.mEntityInCellContainer.transform.SetParent(tWorld.transform, false);
        //trigger
        tWorld.mTriggerContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mTriggerContainer.name = "triggerContainer";
        tWorld.mTriggerContainer.transform.SetParent(tWorld.transform, false);
        //event処理システム
        tWorld.mEventSystem = new MapEventSystem(tWorld);
        return tWorld;
    }
    /// <summary>イベントを生成してDictionaryに追加</summary>
    static private void createEvent() {
        mWorld.mEvents = new Dictionary<string, MapEvent>();
        MapFileData.Event tEvents = mData.mEvents;
        foreach (KeyValuePair<string, object> tPair in (Dictionary<string, object>)tEvents.mDic) {
            Arg tData = tEvents.get(tPair.Key);
            mWorld.mEvents.Add(tPair.Key, MapEvent.createRoot(tData));
        }
    }
    /// <summary>フラグを確認して生成するか決定</summary>
    static private bool flagCreate(MapFileData.Behaviour aData) {
        //削除フラグ確認
        MyFlagItem tItem = aData.mDeleteFlag;
        if (tItem != null) {
            if (mWorld.checkFlag(tItem))
                return false;
        }
        //生成フラグ確認
        tItem = aData.mCreateFlag;
        if (tItem != null) {
            return mWorld.checkFlag(tItem);
        }
        return true;
    }
}