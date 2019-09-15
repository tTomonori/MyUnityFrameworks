using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    /// <summary>
    /// 1マスを生成
    /// </summary>
    /// <returns>マス</returns>
    /// <param name="aCellData">マスのデータ</param>
    public static MapCell createCell(MapFileData.Cell aCellData) {
        MapCell tCell = MyBehaviour.create<MapCell>();
        //tileを生成
        int tTileLength = aCellData.mTile.Count;
        for (int i = 0; i < tTileLength; i++) {
            string tTilePath = aCellData.mTile[i];
            MapTile tTile = MyBehaviour.createObjectFromResources<MapTile>(MyMap.mMapResourcesDirectory + "/tile/" + tTilePath);
            tTile.transform.SetParent(tCell.transform, false);
            tTile.positionZ = MapZOrderCalculator.calculateOrderOfTile(tTileLength - 1 - i);
        }
        //エンカウント

        return tCell;
    }
    //cellを生成
    private static MapCell createCell(int aChipNum) {
        MapFileData.Cell tData = mData.mChip.get(aChipNum);
        if (tData == null) return null;
        return createCell(tData);
    }
    //指定座標のcellを生成してworldに追加
    private static void buildCell(int aX,int aY,int aStratumLevel) {
        int tY = mData.mStratums[aStratumLevel].mFeild.Count - 1 - aY;
        int tChipNum = mData.mStratums[aStratumLevel].mFeild[tY][aX];
        MapCell tCell = createCell(tChipNum);
        if (tCell == null) return;
        mWorld.addCell(tCell, aX, aY, aStratumLevel);
    }
}

