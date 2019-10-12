using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    /// <summary>cellに含まれるornamentのリスト フィールド生成後に追加用に退避させたもの</summary>
    static private List<EntityTempData> mEntityInCellDataList;
    /// <summary>足場の高さレベルの調整が必要なcellの高さ0の位置のY座標</summary>
    static private List<int> mYRequireAdjustmentScaffoldLevel;
    /// <summary>上のcellと描画順を合わせる為に必要になるデータのリスト</summary>
    static private List<WaitingSetOffsetCellData> mWaitingSetOffsetCell;
    /// <summary>フィールド(階層,マス)を生成</summary>
    static public void buildField() {
        mEntityInCellDataList = new List<EntityTempData>();
        mYRequireAdjustmentScaffoldLevel = new List<int>();
        mWaitingSetOffsetCell = new List<WaitingSetOffsetCellData>();
        for (int i = 0; i < mWorld.mSize.z; ++i) {
            buildStratum(i);
        }
        adjustScaffoldLevel();
        moveEntityInCellFromList();
        applyDrawOffsetY();
        mEntityInCellDataList = null;
        mYRequireAdjustmentScaffoldLevel = null;
        mWaitingSetOffsetCell = null;
    }
    /// <summary>cellに足場の高さレベル設定</summary>
    static private void adjustScaffoldLevel() {
        //Yの値の被りをなくす
        List<int> tYList = new List<int>();
        foreach (int tY in mYRequireAdjustmentScaffoldLevel) {
            bool tIsNew = true;
            foreach (int tAddedY in tYList) {
                if (tAddedY == tY) {
                    tIsNew = false;
                    break;
                }
            }
            if (tIsNew)
                tYList.Add(tY);
        }
        //足場の高さレベル調整
        foreach (int tY in tYList) {
            ScaffoldLevelAdjuster.adjust(mWorld, tY);
        }
    }
    /// <summary>drawOffsetYを適用する</summary>
    static private void applyDrawOffsetY() {
        MapCell tCell;
        MapCell tUpCell;
        MapCell.DrawOffsetData tOffsetData;
        foreach (WaitingSetOffsetCellData tData in mWaitingSetOffsetCell) {
            tCell = mWorld.mCells[tData.mPosition.x, tData.mPosition.y, tData.mPosition.z];
            //Y座標が上のcellに合わせる
            tUpCell = mWorld.getCell(tData.mPosition.x, tData.mPosition.y + tData.mOffsetY, tData.mPosition.z);
            if (tUpCell != null) {
                tOffsetData = new MapCell.DrawOffsetData();
                tOffsetData.mPositionY = tUpCell.mMapPosition.y;
                tOffsetData.mScaffoldLevel = tUpCell.mScaffoldLevel;
                tCell.mDrawOffsetData = tOffsetData;
                tCell.applyPosition();
                continue;
            }

            //offsetYで指定された座標のcellがなかった場合
            //高さが下のcellに合わせる
            tUpCell = mWorld.getCell(tData.mPosition.x, tData.mPosition.y, tData.mPosition.z - tData.mOffsetY * 2);
            if (tUpCell != null) {
                tOffsetData = new MapCell.DrawOffsetData();
                tOffsetData.mPositionY = tUpCell.mMapPosition.y;
                tOffsetData.mScaffoldLevel = tUpCell.mScaffoldLevel + 1;//画面上で真下のcellの一つ上の足場高さレベル
                tCell.mDrawOffsetData = tOffsetData;
                tCell.applyPosition();
                continue;
            }
            //左右方向に坂を探索して合わせる
            bool tSearchLeft = true;
            bool tSearchRight = true;
            MapCell tSearchedCell = null;
            for (int i = 1; ; ++i) {
                //左側を探す
                if (tSearchLeft) {
                    tUpCell = mWorld.getCell(tData.mPosition.x - i, tData.mPosition.y, tData.mPosition.z - tData.mOffsetY * 2 + 1);
                    if(tUpCell!=null&&tUpCell.mScaffoldType== MapCell.ScaffoldType.rightHighSlope) {
                        //坂のcell発見
                        tSearchedCell = tUpCell;
                        break;
                    }
                    tUpCell = mWorld.getCell(tData.mPosition.x - i, tData.mPosition.y, tData.mPosition.z);
                    if(tUpCell==null||tUpCell.mScaffoldType!= MapCell.ScaffoldType.flat) {
                        //平地続きじゃない
                        tSearchLeft = false;
                    }
                }
                //右側を探す
                if (tSearchRight) {
                    tUpCell = mWorld.getCell(tData.mPosition.x + i, tData.mPosition.y, tData.mPosition.z - tData.mOffsetY * 2 + 1);
                    if (tUpCell != null && tUpCell.mScaffoldType == MapCell.ScaffoldType.leftHighSlope) {
                        //坂のcell発見
                        tSearchedCell = tUpCell;
                        break;
                    }
                    tUpCell = mWorld.getCell(tData.mPosition.x + i, tData.mPosition.y, tData.mPosition.z);
                    if (tUpCell == null || tUpCell.mScaffoldType != MapCell.ScaffoldType.flat) {
                        //平地続きじゃない
                        tSearchRight = false;
                    }
                }
                if (!tSearchLeft && !tSearchRight)
                    break;
            }
            //見つけた坂に合わせる
            if (tSearchedCell != null) {
                tOffsetData = new MapCell.DrawOffsetData();
                tOffsetData.mPositionY = tData.mPosition.y + tData.mOffsetY;
                tOffsetData.mScaffoldLevel = tSearchedCell.mScaffoldSurfaceLevel;//坂の上側の足場高さレベル
                tCell.mDrawOffsetData = tOffsetData;
                tCell.applyPosition();
                continue;
            }
        }
    }
    /// <summary>フィールド生成時に退避させたcellに含まれるornamentをフィールドに移す</summary>
    static private void moveEntityInCellFromList() {
        foreach (EntityTempData tData in mEntityInCellDataList) {
            moveEntityInCell(tData.mEntity, tData.mX, tData.mY, tData.mHeight);
        }
    }
    /// <summary>EntityInCellをフィールドに移す時に必要になるデータ</summary>
    public struct EntityTempData {
        public EntityInCell mEntity;
        public float mX;
        public float mY;
        public int mHeight;
    }
    /// <summary>上のcellと描画順を合わせる為に必要になるデータ</summary>
    public struct WaitingSetOffsetCellData {
        public Vector3Int mPosition;
        public int mOffsetY;
    }
}

