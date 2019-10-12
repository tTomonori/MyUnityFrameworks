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
    static private void moveEntityInCell(EntityInCell aEntity, float aX, float aY, int aHeight) {
        //移動
        aEntity.transform.SetParent(mWorld.mEntityInCellContainer.transform, false);
        aEntity.mMapPosition = new Vector2(aX + aEntity.positionX, aY + aEntity.positionY);
        aEntity.mHeight = aHeight;
        aEntity.mScaffoldLevel = MapWorldUpdater.getScaffoldLevel(aEntity.mMapPosition, aEntity.mHeight, mWorld);
        aEntity.applyPosition();
        //画像イベント適用
        MapWorldUpdater.applyImageEvent(aEntity);
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
        tOrnament.mMapPosition = new Vector2(aData.mX, aData.mY);
        tOrnament.mHeight = aData.mHeight;
        tOrnament.mScaffoldLevel = MapWorldUpdater.getScaffoldLevel(tOrnament.mMapPosition, tOrnament.mHeight, mWorld);
        tOrnament.applyPosition();
        //画像イベント適用
        MapWorldUpdater.applyImageEvent(tOrnament);
    }
}

