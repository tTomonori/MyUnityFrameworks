using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapCellFactory {
    /// <summary>
    /// 1マスを生成
    /// </summary>
    /// <returns>マス</returns>
    /// <param name="aCellData">マスのデータ</param>
    public static MapCell create(MapFileData.Cell aCellData) {
        MapCell tCell = MyBehaviour.create<MapCell>();
        //tileを生成
        int tTileLength = aCellData.mTile.Count;
        for(int i = 0; i < tTileLength; i++) {
            string tTilePath = aCellData.mTile[i];
            MapTile tTile = MyBehaviour.createObjectFromResources<MapTile>(MyMap.mMapResourcesDirectory + "/tile/" + tTilePath);
            tTile.transform.SetParent(tCell.transform, false);
            tTile.positionZ = MapZOrderCalculator.calculateOrderOfTile(tTileLength - 1 - i);
        }
        //エンカウント

        return tCell;
    }
}
