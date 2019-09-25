using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    /// <summary>
    /// 1マスを生成
    /// </summary>
    /// <returns>マス</returns>
    /// <param name="aCellData">マスのデータ</param>
    public static MapCell createCell(MapFileData.Cell aCellData) {
        string tPrefabPath = aCellData.mCell;
        if (tPrefabPath == null) {
            return MyBehaviour.create<MapCell>();
        }
        //ロード
        MapCell tCell = MyBehaviour.createObjectFromResources<MapCell>(MyMap.mMapResourcesDirectory + "/tile/" + tPrefabPath);
        //属性にcellを割り当て
        foreach (TilePhysicsAttribute tAttribute in tCell.GetComponentsInChildren<TilePhysicsAttribute>()) {
            tAttribute.mBehaviour = tCell;
        }
        //エンカウント

        return tCell;
    }
    //cellを生成
    private static MapCell createCell(int aChipNum) {
        MapFileData.Cell tData = mData.mChip.get(aChipNum);
        if (tData == null) return MyBehaviour.create<MapCell>();
        return createCell(tData);
    }
    //指定座標のcellを生成してworldに追加(平面のマス)
    private static void buildLieCell(int aX, int aY, int aStratumLevel) {
        int tY = mData.mStratums[aStratumLevel].mFeild.Count - 1 - aY;
        int tChipNum = mData.mStratums[aStratumLevel].mFeild[tY][aX];
        MapCell tCell = createCell(mData.mChip.get(tChipNum));
        //座標設定
        tCell.position = new Vector3(aX, aY);
        tCell.positionZ = MapZOrderCalculator.calculateOrderOfLieCell(aX, aY, aStratumLevel);
        tCell.setHeight(aStratumLevel);
        //EntityInTileを移動
        foreach (EntityInTile tEntity in tCell.GetComponentsInChildren<EntityInTile>()) {
            adaptEntityInTile(tEntity, aX, aY, aStratumLevel, tEntity.positionZ);
        }
        //階層に追加
        tCell.transform.SetParent(mWorld.mCellContainers[aStratumLevel].transform, false);
        tCell.changeLayer(MyMap.mStratumLayerNum[aStratumLevel]);
    }
    //指定座標のcellを生成してworldに追加(直立のマス)
    private static void buildStandCell(int aX, int aY, int aStratumLevel) {
        int tY = mData.mStratums[aStratumLevel].mFeild.Count - 1 - aY;
        int tChipNum = mData.mStratums[aStratumLevel].mFeild[tY][aX];
        MapCell tCell = createCell(mData.mChip.get(tChipNum));
        //座標設定
        tCell.position = new Vector3(aX, aY);
        float oPositionZ;
        tCell.gameObject.AddComponent<SortingGroup>().sortingOrder = MapZOrderCalculator.calculateOrderOfStandCells(aY, aStratumLevel, out oPositionZ);
        tCell.positionZ = oPositionZ;
        tCell.setHeight(aStratumLevel);
        //EntityInTileを移動
        foreach (EntityInTile tEntity in tCell.GetComponentsInChildren<EntityInTile>()) {
            adaptEntityInTile(tEntity, aX, aY, aStratumLevel, tEntity.positionY);
        }
        //standTileに追加
        tCell.transform.SetParent(mWorld.mStandCellContainers[aY - aStratumLevel].transform, false);
        tCell.changeLayer(MyMap.mStandLayerNum);
    }
}

