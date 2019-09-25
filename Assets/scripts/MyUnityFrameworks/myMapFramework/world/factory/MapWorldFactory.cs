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

        //階層,マス
        mWorld.mStratumNum = aData.mStratums.Count;
        //平面マスの入れ物
        mWorld.mCellContainers = new MyBehaviour[mWorld.mStratumNum];
        //直立マスの入れ物
        int tYLength = aData.mStratums[0].mFeild.Count;
        mWorld.mStandCellContainers = new MyBehaviour[tYLength];
        for (int i = 0; i < tYLength; ++i) {
            MyBehaviour tCellContainer = MyBehaviour.create<MyBehaviour>();
            tCellContainer.name = "standCell" + i.ToString();
            tCellContainer.transform.SetParent(mWorld.mStandTileContainer.transform, false);
        }
        //カメラ生成
        for (int i = 0; i < mWorld.mStratumNum; ++i) {
            buildCamera(i);
        }
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
        tWorld.gameObject.AddComponent<SortingGroup>();
        //カメラ
        tWorld.mCameraContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mCameraContainer.name = "cameraContainer";
        tWorld.mCameraContainer.transform.SetParent(tWorld.transform, false);
        //階層
        tWorld.mStandContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mStandContainer.name = "stratumContainer";
        tWorld.mStandContainer.transform.SetParent(tWorld.transform, false);
        tWorld.mStandContainer.gameObject.AddComponent<SortingGroup>().sortingOrder = 0;
        //マス
        //tWorld.mCellContainers = new MyBehaviour[];
        //stand
        tWorld.mStandContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mStandContainer.name = "standContainer";
        tWorld.mStandContainer.transform.SetParent(tWorld.transform, false);
        tWorld.mStandContainer.gameObject.AddComponent<SortingGroup>().sortingOrder = 1;
        //character
        tWorld.mCharacterContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mCharacterContainer.name = "characterContainer";
        tWorld.mCharacterContainer.transform.SetParent(tWorld.mStandContainer.transform, false);
        //entity
        tWorld.mOrnamentContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mOrnamentContainer.name = "ornamentContainer";
        tWorld.mOrnamentContainer.transform.SetParent(tWorld.mStandContainer.transform, false);
        //垂直なマス
        tWorld.mStandTileContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mStandTileContainer.name = "standTileContainer";
        tWorld.mStandTileContainer.transform.SetParent(tWorld.mStandContainer.transform, false);
        //entityInTile
        tWorld.mEntityInTileContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mEntityInTileContainer.name = "entityInTileContainer";
        tWorld.mEntityInTileContainer.transform.SetParent(tWorld.mStandContainer.transform, false);
        //trigger
        tWorld.mTriggerContainer = MyBehaviour.create<MyBehaviour>();
        tWorld.mTriggerContainer.name = "triggerContainer";
        tWorld.mTriggerContainer.transform.SetParent(tWorld.transform, false);
        return tWorld;
    }

    /// <summary>カメラを生成してworldに追加</summary>
    static private void buildCamera(int aTargetStratumLevel) {
        Camera tCamera = MyBehaviour.create<Camera>();
        tCamera.name = "camera TargetStratum " + aTargetStratumLevel.ToString();
        tCamera.clearFlags = (aTargetStratumLevel == 0) ? CameraClearFlags.Skybox : CameraClearFlags.Depth;
        tCamera.orthographic = true;
        tCamera.depth = aTargetStratumLevel;
        tCamera.cullingMask = (1 << MyMap.mStandLayerNum);
        tCamera.cullingMask |= (1 << MyMap.mStratumLayerNum[aTargetStratumLevel]);
        tCamera.transform.SetParent(mWorld.mCameraContainer.transform, false);
    }
}