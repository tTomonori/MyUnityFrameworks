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
    public static MapTile createTile(MapFileData.Tile aTileData) {
        string tPrefabPath = aTileData.mCell;
        //ロード
        MapTile tTile = MyBehaviour.createObjectFromResources<MapTile>(MyMap.mMapResourcesDirectory + "/tile/" + tPrefabPath);
        //属性にcellを割り当て
        foreach (TilePhysicsAttribute tAttribute in tTile.GetComponentsInChildren<TilePhysicsAttribute>()) {
            tAttribute.mBehaviour = tTile;
        }
        //エンカウント

        return tTile;
    }
    //指定座標のcellを生成してworldに追加
    private static void buildCell(int aX, int aY, int aH) {
        int tY = mWorld.mSize.y - 1 - aY;
        MapCell tCell = new MapCell();

        MapTile tTile;
        MapFileData.Ornament tData;
        //平面階層
        int tChipNum = mData.mStratums[aH].mFeild[tY][aX];
        MapFileData.Tile tTileData = mData.mChip.get(tChipNum);
        if (tTileData != null) {
            tTile = createTile(tTileData);
            //accessory
            tData = tTileData.mOrnamentInTile;
            if (tData != null) {
                tData.toInTileData(new Vector3(aX, aY, aH));
                mOrnamentInTileData.Add(tData);
            }
            //階層に追加
            tTile.transform.SetParent(mWorld.mStratums[aH].mTiles.transform, false);
            tTile.changeLayer(MyMap.mStratumLayerNum[aH]);
            tCell.mTile = tTile;
            //drawOffset
            if (tTileData.mDrawOffsetH != 0) {
                tTile.mDrawOffsetData.mHeight = tTileData.mDrawOffsetH;
                tTile.mLieBehaviourPileLevel += (tTile.mDrawOffsetData.mHeight > 0) ? -1 : 1;
            }
            tTile.mLieBehaviourPileLevel += -5;
            //座標設定
            tTile.setMapPosition(new Vector2(aX, aY), aH);
            //encount
            if (tTileData.mEncountKey != "") {
                tCell.mEncountKey = tTileData.mEncountKey;
                tCell.mEncountFrequency = tTileData.mEncountFrequency;
            }
        }
        //+0.5階層
        tChipNum = mData.mStratums[aH].mHalfHeightFeild[tY][aX];
        tTileData = mData.mChip.get(tChipNum);
        if (tTileData != null) {
            tTile = createTile(tTileData);
            //accessory
            tData = tTileData.mOrnamentInTile;
            if (tData != null) {
                tData.toInTileData(new Vector3(aX, aY, aH));
                mOrnamentInTileData.Add(tData);
            }
            //階層に追加
            tTile.transform.SetParent(mWorld.mStratums[aH].mHalfHeightTiles.transform, false);
            tTile.changeLayer(MyMap.mStratumLayerNum[aH]);
            tCell.mHalfHeightTile = tTile;
            //drawOffset
            if (tTileData.mDrawOffsetH != 0) {
                tTile.mDrawOffsetData.mHeight = tTileData.mDrawOffsetH;
                tTile.mLieBehaviourPileLevel += (tTile.mDrawOffsetData.mHeight > 0) ? -1 : 1;
            }
            tTile.mLieBehaviourPileLevel += -4;
            //座標設定
            tTile.setMapPosition(new Vector2(aX, aY), aH);
            //encount
            if (tTileData.mEncountKey != "") {
                tCell.mEncountKey = tTileData.mEncountKey;
                tCell.mEncountFrequency = tTileData.mEncountFrequency;
            }
        }

        mWorld.mCells[aX, aY, aH] = tCell;
    }
}

