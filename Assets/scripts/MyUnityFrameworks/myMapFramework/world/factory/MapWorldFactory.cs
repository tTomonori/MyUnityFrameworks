using System.Collections;
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
        mWorld = initWorld();

        //階層の数
        mWorld.mStratumNum = aData.mStratums.Count;
        //マスの入れ物
        mWorld.mStratums = new MyBehaviour[mWorld.mStratumNum];
        //カメラ生成
        buildCamera(mWorld.mStratumNum);
        //マス生成
        for (int i = 0; i < mWorld.mStratumNum; ++i) {
            buildStratum(i);
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

        MapWorld tCreatedWorld = mWorld;
        mWorld = null;
        mData = null;
        return tCreatedWorld;
    }
    /// <summary>worldを生成しコンテナを追加</summary>
    static private MapWorld initWorld() {
        MapWorld tWorld = MyBehaviour.create<MapWorld>();
        tWorld.name = "world";
        //tWorld.gameObject.AddComponent<SortingGroup>();
        //カメラ
        tWorld.mCameraContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mCameraContainer.name = "cameraContainer";
        tWorld.mCameraContainer.transform.SetParent(tWorld.transform, false);
        //階層
        tWorld.mStratumContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mStratumContainer.name = "stratumContainer";
        tWorld.mStratumContainer.transform.SetParent(tWorld.transform, false);
        //マス
        //tWorld.mCellContainers = new MyBehaviour[];
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