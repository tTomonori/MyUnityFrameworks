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
        //ロード
        MapCell tCell = MyBehaviour.createObjectFromResources<MapCell>(MyMap.mMapResourcesDirectory + "/tile/" + tPrefabPath);
        //属性にcellを割り当て
        foreach (TilePhysicsAttribute tAttribute in tCell.GetComponentsInChildren<TilePhysicsAttribute>()) {
            tAttribute.mBehaviour = tCell;
        }
        //drawOffset
        tCell.mDrawOffsetHeight += aCellData.mDrawOffsetHeight;
        //エンカウント

        return tCell;
    }
    //指定座標のcellを生成してworldに追加(平面のマス)
    private static void buildCell(int aX, int aY, int aStratumLevel) {
        int tY = mData.mStratums[aStratumLevel].mFeild.Count - 1 - aY;
        int tChipNum = mData.mStratums[aStratumLevel].mFeild[tY][aX];
        MapFileData.Cell tCellData = mData.mChip.get(tChipNum);
        if (tCellData == null) return;
        MapCell tCell = createCell(tCellData);
        //マスに含まれるオブジェクトの移動
        foreach (EntityInCell tEntity in tCell.GetComponentsInChildren<EntityInCell>()) {
            moveEntityInCell(tEntity, aX, aY, aStratumLevel);
        }
        //階層に追加
        tCell.transform.SetParent(mWorld.mStratums[aStratumLevel].transform, false);
        tCell.changeLayer(MyMap.mStratumLayerNum[aStratumLevel / 2]);
        mWorld.mCells[aX, aY, aStratumLevel] = tCell;
        //奇数階層の場合は1つ上の階層とも衝突させる
        tCell.mCollideUpperStratum = aStratumLevel % 2 == 1;
        //足場の高さ
        if (tCell.mHideLower || aStratumLevel < 2) {
            tCell.mScaffoldHeight = aStratumLevel / 2;
        } else {
            tCell.mScaffoldHeight = MapWorldUpdater.getScaffoldHeight(new Vector3Int(aX, aY, aStratumLevel / 2 - 1), mWorld);
        }
        //座標設定
        tCell.setPosition(new Vector2(aX, aY), aStratumLevel / 2);
    }
}

