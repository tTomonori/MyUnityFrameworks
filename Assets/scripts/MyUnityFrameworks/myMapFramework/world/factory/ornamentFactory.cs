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
    static private void moveEntityInCell(EntityInCell aEntity, float aX, float aY, int aStratumLevel) {
        //移動
        aEntity.transform.SetParent(mWorld.mEntityInCellContainer.transform, false);

        aEntity.setPosition(new Vector2(aX + aEntity.positionX, aY + aEntity.positionY), aStratumLevel);
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

        tOrnament.setPosition(new Vector2(aData.mX, aData.mY), aData.mHeight);
    }
}

