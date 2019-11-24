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
    private static void buildCell(int aX, int aY, int aZ) {
        int tZ = mWorld.mSize.z - 1 - aZ;
        MapCell tCell = new MapCell();

        MapTile tTile;
        //平面階層
        int tChipNum = mData.mStratums[aY].mFeild[tZ][aX];
        MapFileData.Tile tTileData = mData.mChip.get(tChipNum);
        if (tTileData != null) {
            tTile = createTile(tTileData);
            //階層に追加
            tTile.transform.SetParent(mWorld.mStratums[aY].mTiles.transform, false);
            tTile.changeLayer(MyMap.mStratumLayerNum[aY]);
            tCell.mTile = tTile;
            //座標設定
            tTile.mMapPosition = new MapPosition(new Vector3(aX, aY, aZ));
            //encount
            if (tTileData.mEncountKey != "") {
                tCell.mEncountKey = tTileData.mEncountKey;
                tCell.mEncountFrequency = tTileData.mEncountFrequency;
            }
        }
        //+0.5階層
        tChipNum = mData.mStratums[aY].mHalfHeightFeild[tZ][aX];
        tTileData = mData.mChip.get(tChipNum);
        if (tTileData != null) {
            tTile = createTile(tTileData);
            //階層に追加
            tTile.transform.SetParent(mWorld.mStratums[aY].mHalfHeightTiles.transform, false);
            tTile.changeLayer(MyMap.mStratumLayerNum[aY]);
            tCell.mHalfHeightTile = tTile;
            //座標設定
            tTile.mMapPosition = new MapPosition(new Vector3(aX, aY, aZ));
            //encount
            if (tTileData.mEncountKey != "") {
                tCell.mEncountKey = tTileData.mEncountKey;
                tCell.mEncountFrequency = tTileData.mEncountFrequency;
            }
        }

        mWorld.mCells[aX, aY, aZ] = tCell;
        //2D描画でのY方向サイズ更新
        if (tCell.mTile != null || tCell.mHalfHeightTile != null) {
            if (mWorld.mOrthographySizeY < aY + aZ)
                mWorld.mOrthographySizeY = aY + aZ;
        }
    }
    /// <summary>マップの周りの壁を生成して配置</summary>
    static private void buildEnd() {
        MapTile tTile;
        //上
        tTile = createEnd(new Vector3(mWorld.mSize.x + 2, mWorld.mSize.y, 1));
        tTile.name = "upEnd";
        tTile.mMapPosition = new MapPosition(new Vector3(mWorld.mSize.x / 2f - 0.5f, 0, mWorld.mSize.z));
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
        //下
        tTile = createEnd(new Vector3(mWorld.mSize.x + 2, mWorld.mSize.y, 1));
        tTile.name = "downEnd";
        tTile.mMapPosition = new MapPosition(new Vector3(mWorld.mSize.x / 2f - 0.5f, 0, -1));
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
        //左
        tTile = createEnd(new Vector3(1, mWorld.mSize.y, mWorld.mSize.z + 2));
        tTile.name = "leftEnd";
        tTile.mMapPosition = new MapPosition(new Vector3(-1, 0, mWorld.mSize.z / 2f - 0.5f));
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
        //右
        tTile = createEnd(new Vector3(1, mWorld.mSize.y, mWorld.mSize.z + 2));
        tTile.name = "rightEnd";
        tTile.mMapPosition = new MapPosition(new Vector3(mWorld.mSize.x, 0, mWorld.mSize.z / 2f - 0.5f));
        tTile.transform.SetParent(mWorld.mEndContainer.transform, false);
    }
    /// <summary>マップ周りに配置する壁を生成</summary>
    static private MapTile createEnd(Vector3 aSize) {
        MapTile tTile = MyBehaviour.create<MapTile>();
        //physicsBehaviour生成
        MapPhysicsBehaviour tPhysics = tTile.createChild<MapPhysicsBehaviour>("physics");
        tTile.mPhysicsBehaviour = tPhysics;
        //属性
        TileGroundPhysicsAttribute tAttribute = tPhysics.gameObject.AddComponent<TileGroundPhysicsAttribute>();
        tAttribute.mAttribute = TileGroundPhysicsAttribute.Attribute.end;
        tAttribute.mTile = tTile;
        //collider
        BoxCollider tCollider = tPhysics.gameObject.AddComponent<BoxCollider>();
        tCollider.size = aSize;
        tCollider.center = new Vector3(0, aSize.y / 2f, 0);
        return tTile;
    }
}

