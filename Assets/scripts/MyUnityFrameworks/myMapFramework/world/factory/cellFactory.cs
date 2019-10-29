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
        string tPrefabPath = aTileData.mTile;
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
        //平面階層
        int tChipNum = mData.mStratums[aH].mFeild[tY][aX];
        MapFileData.Tile tTileData = mData.mChip.get(tChipNum);
        if (tTileData != null) {
            tTile = createTile(tTileData);
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
    /// <summary>マップの周りの壁を生成して配置</summary>
    static private void buildEnd() {
        MapTile tTile;
        //上
        tTile = createEnd(new Vector2(mWorld.mSize.x + 2, 1));
        tTile.name = "upEnd";
        tTile.setMapPosition(new Vector2(mWorld.mSize.x / 2f - 0.5f, mWorld.mSize.y), 0);
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
        //下
        tTile = createEnd(new Vector2(mWorld.mSize.x + 2, 1));
        tTile.name = "downEnd";
        tTile.setMapPosition(new Vector2(mWorld.mSize.x / 2f - 0.5f, -1), 0);
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
        //左
        tTile = createEnd(new Vector2(1, mWorld.mSize.y + 2));
        tTile.name = "leftEnd";
        tTile.setMapPosition(new Vector2(-1, mWorld.mSize.y / 2f - 0.5f), 0);
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
        //右
        tTile = createEnd(new Vector2(1, mWorld.mSize.y + 2));
        tTile.name = "rightEnd";
        tTile.setMapPosition(new Vector2(mWorld.mSize.x, mWorld.mSize.y / 2f - 0.5f), 0);
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
    }
    /// <summary>マップ周りに配置する壁を生成</summary>
    static private MapTile createEnd(Vector2 aSize) {
        MapTile tTile = MyBehaviour.create<MapTile>();
        tTile.mCollideHeight = mWorld.mSize.z + 1;
        TileGroundPhysicsAttribute tAttribute = tTile.gameObject.AddComponent<TileGroundPhysicsAttribute>();
        tAttribute.mAttribute = TileGroundPhysicsAttribute.Attribute.end;
        tAttribute.mTile = tTile;
        BoxCollider2D tCollider = tTile.gameObject.AddComponent<BoxCollider2D>();
        tCollider.size = aSize;
        return tTile;
    }
}

