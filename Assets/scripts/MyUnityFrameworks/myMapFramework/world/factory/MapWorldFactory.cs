﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    //生成に使っているマップデータ
    static private MapFileData mData;
    //生成中のMapWorld
    static private MapWorld mWorld;
    //<summary>ワールドを作成</summary>
    static public MapWorld create(MapFileData aData) {
        //マップデータを記憶
        mData = aData;
        mWorld = initWorld(new Vector3Int(aData.mStratums[0].mFeild[0].Count, aData.mStratums[0].mFeild.Count, aData.mStratums.Count));

        //カメラ生成
        buildCamera(mWorld.mSize.z);
        //フィールド生成
        buildField();
        //影生成
        List<MapFileData.Shadow> tShadowData = mData.mShadow;
        int tShadowNum = tShadowData.Count;
        for(int i = 0; i < tShadowNum; ++i) {
            buildShadow(tShadowData[i]);
        }
        //ornament生成
        List<MapFileData.Ornament> tOrnamentData = mData.mOrnaments;
        int tOrnamentNum = tOrnamentData.Count;
        for (int i = 0; i < tOrnamentNum; ++i) {
            buildOrnament(tOrnamentData[i]);
        }
        //character(npc)生成
        List<MapFileData.Npc> tNpcData = mData.mNpc;
        int tNpcNum = tNpcData.Count;
        for (int i = 0; i < tNpcNum; ++i) {
            buildCharacter(tNpcData[i]);
        }
        //trigger生成
        List<MapFileData.Trigger> tTriggerData = mData.mTrigger;
        int tTriggerNum = tTriggerData.Count;
        for (int i = 0; i < tTriggerNum; ++i) {
            buildTrigger(tTriggerData[i]);
        }

        //生成完了
        foreach (MapBehaviour tBehaviour in mWorld.GetComponentsInChildren<MapBehaviour>())
            tBehaviour.placed();

        MapWorld tCreatedWorld = mWorld;
        mWorld = null;
        mData = null;
        return tCreatedWorld;
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
        return tWorld;
    }

    /// <summary>カメラを生成してworldに追加</summary>
    static private void buildCamera(int aStratumNum) {
        Camera tCamera = MyBehaviour.create<Camera>();
        tCamera.name = "mapCamera";
        tCamera.clearFlags = CameraClearFlags.Skybox;
        tCamera.orthographic = true;
        //tCamera.depth = 0;
        tCamera.cullingMask = (1 << MyMap.mStratumLayerNum[0]);
        for (int i = 1; i < aStratumNum; ++i)
            tCamera.cullingMask |= (1 << MyMap.mStratumLayerNum[i]);
        tCamera.transform.SetParent(mWorld.mCameraContainer.transform, false);
        tCamera.transform.localPosition = new Vector3(0, 0, -10);
    }
}