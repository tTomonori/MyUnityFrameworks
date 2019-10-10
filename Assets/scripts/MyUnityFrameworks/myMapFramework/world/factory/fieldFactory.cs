using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    /// <summary>cellに含まれるornamentのリスト フィールド生成後に追加用に退避させたもの</summary>
    static private List<EntityTempData> mEntityInCellDataList;
    /// <summary>足場の高さレベルの調整が必要なcellの高さ0の位置のY座標</summary>
    static private List<int> mYRequireAdjustmentScaffoldLevel;
    /// <summary>フィールド(階層,マス)を生成</summary>
    static public void buildField() {
        mEntityInCellDataList = new List<EntityTempData>();
        mYRequireAdjustmentScaffoldLevel = new List<int>();
        for (int i = 0; i < mWorld.mSize.z; ++i) {
            buildStratum(i);
        }
        adjustScaffoldLevel();
        moveEntityInCellFromList();
        mEntityInCellDataList = null;
        mYRequireAdjustmentScaffoldLevel = null;
    }
    /// <summary>cellに足場の高さレベル設定</summary>
    static private void adjustScaffoldLevel() {
        //Yの値の被りをなくす
        List<int> tYList = new List<int>();
        foreach(int tY in mYRequireAdjustmentScaffoldLevel) {
            bool tIsNew = true;
            foreach(int tAddedY in tYList) {
                if (tAddedY == tY) {
                    tIsNew = false;
                    break;
                }
            }
            if (tIsNew)
                tYList.Add(tY);
        }
        //足場の高さレベル調整
        foreach(int tY in tYList) {
            ScaffoldLevelAdjuster.adjust(mWorld, tY);
        }
    }
    /// <summary>フィールド生成時に退避させたcellに含まれるornamentをフィールドに移す</summary>
    static private void moveEntityInCellFromList() {
        foreach(EntityTempData tData in mEntityInCellDataList) {
            moveEntityInCell(tData.mEntity, tData.mX, tData.mY, tData.mHeight);
        }
    }
    public struct EntityTempData {
        public EntityInCell mEntity;
        public float mX;
        public float mY;
        public int mHeight;
    }
}

