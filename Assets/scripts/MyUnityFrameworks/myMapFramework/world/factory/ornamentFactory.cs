using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    /// <summary>
    /// マスに含まれるオブジェクトの移動などを行う
    /// </summary>
    /// <param name="aEntity">オブジェクト</param>
    /// <param name="aX">マスのx座標</param>
    /// <param name="aY">マスのy座標</param>
    /// <param name="aStratumLevel">マスの階層レベル</param>
    /// <param name="aPositionZ">オブジェクトのマスに対する相対Z座標</param>
    static private void adaptEntityInTile(EntityInTile aEntity, float aX, float aY, int aStratumLevel, float aLocalPositionY) {
        //移動
        aEntity.transform.SetParent(mWorld.mEntityInTileContainer.transform, false);

        aEntity.setPosition(new Vector2(aX, aY + aLocalPositionY), aStratumLevel);

        //float oPositionZ;
        //aEntity.gameObject.AddComponent<SortingGroup>().sortingOrder = MapZOrderCalculator.calculateOrderOfEntityInTile(aX, aY, aStratumLevel, aPositionZ,out oPositionZ);
        //aEntity.positionZ = oPositionZ;
        //aEntity.changeLayer(MyMap.mStratumLayerNum[aStratumLevel]);
    }



    //<summary>物生成</summary>
    static public MapOrnament createOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = MyBehaviour.createObjectFromResources<MapOrnament>(MyMap.mMapResourcesDirectory + "/ornament/" + aData.mPath);

        return tOrnament;
    }
    //<summary>物を生成してworldに追加</summary>
    static private void buildOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = createOrnament(aData);
        tOrnament.transform.SetParent(mWorld.mOrnamentContainer.transform, false);
        tOrnament.changeLayer(MyMap.mStandLayerNum);

        tOrnament.setPosition(new Vector2(aData.mX, aData.mY), aData.mHeight);
    }
}

