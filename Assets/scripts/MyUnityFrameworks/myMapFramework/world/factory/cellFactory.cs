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
            EntityTempData tData = new EntityTempData();
            tData.mEntity = tEntity;
            tData.mX = aX;
            tData.mY = aY;
            tData.mHeight = aStratumLevel / 2;
            mEntityInCellDataList.Add(tData);
        }
        //階層に追加
        tCell.transform.SetParent(mWorld.mStratums[aStratumLevel].mCells.transform, false);
        tCell.changeLayer(MyMap.mStratumLayerNum[aStratumLevel / 2]);
        mWorld.mCells[aX, aY, aStratumLevel] = tCell;
        //足場の高さレベル
        tCell.mScaffoldSurfaceLevel = aStratumLevel / 2;
        tCell.mScaffoldSurfaceLevel2 = aStratumLevel / 2;
        tCell.mScaffoldLevel = aStratumLevel / 2;
        //足場の高さレベルの調整が必要か
        if (tCell.mScaffoldType == MapCell.ScaffoldType.leftHighSlope || tCell.mScaffoldType == MapCell.ScaffoldType.rightHighSlope) {
            mYRequireAdjustmentScaffoldLevel.Add(aY - aStratumLevel / 2);
            mYRequireAdjustmentScaffoldLevel.Add(aY - aStratumLevel / 2 - 1);
        }
        //奇数階層の場合は1つ上の階層とも衝突させる
        tCell.mCollideUpperStratum = aStratumLevel % 2 == 1;
        //座標設定
        tCell.setPosition(new Vector2(aX, aY), aStratumLevel / 2);
    }
}

