using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScaffoldLevelAdjuster {
    /// <summary>既にグループに割り振られていればtrue</summary>
    static private bool[,] mAllocated;
    static private List<ScaffoldLevelGroup> mGroupList;
    static private MapWorld mWorld;
    /// <summary>
    /// 足場の高さレベルを調整
    /// </summary>
    /// <param name="aWorld">MapWorld</param>
    /// <param name="aY">高さが0に位置のY座標</param>
    static public void adjust(MapWorld aWorld, int aY) {
        mAllocated = new bool[aWorld.mSize.x, aWorld.mSize.z];
        mGroupList = null;
        mWorld = aWorld;
        //グループ分け
        for (int tX = 0; tX < aWorld.mSize.x; ++tX) {
            int tH = 0;
            int tY = aY;
            for (; tH * 2 < aWorld.mSize.z; ++tH, ++tY) {
                if (mAllocated[tX, tH]) continue;
                ScaffoldLevelGroup tGroup = gatherGroup(new Vector2Int(tX, tY), tH);
                if (tGroup == null) continue;
                addGroup(tGroup);
            }
        }
        //高さレベル設定
        for (int i = 0; i < mGroupList.Count; ++i) {
            mGroupList[i].setLevel(i);
        }

        mAllocated = null;
        mGroupList = null;
        mWorld = null;
    }
    /// <summary>
    /// 足場の高さが同じグループを集める
    /// </summary>
    /// <returns>足場の高さが同じグループ</returns>
    /// <param name="aLeftPosition">グループ内でXが最も小さいcellの座標</param>
    static private ScaffoldLevelGroup gatherGroup(Vector2Int aLeftPosition, int aHeight) {
        //最も左のcell取得
        Scaffold tScaffold = getScaffold(aLeftPosition, aHeight);
        if (tScaffold == null) {
            mAllocated[aLeftPosition.x, aHeight / 2] = true;
            return null;
        }

        ScaffoldLevelGroup tGroup = new ScaffoldLevelGroup();
        tGroup.mLeftX = aLeftPosition.x;
        tGroup.mRightX = aLeftPosition.x;
        tGroup.mScaffoldList = new List<Scaffold>() { tScaffold };

        int tY = aLeftPosition.y;
        int tH = aHeight;
        if (tScaffold.getScaffoldType() == MapCell.ScaffoldType.leftHighSlope) {
            ++tY;
            ++tH;
        }
        //右側へと調べていく
        for (; ; ) {
            if (tScaffold.getScaffoldType() == MapCell.ScaffoldType.leftHighSlope) {
                mAllocated[tGroup.mRightX, tH - 1] = true;
                --tY;
                --tH;
            } else if (tScaffold.getScaffoldType() == MapCell.ScaffoldType.rightHighSlope) {
                mAllocated[tGroup.mRightX, tH] = true;
                ++tY;
                ++tH;
            } else {
                mAllocated[tGroup.mRightX, tH] = true;
            }
            tScaffold = getScaffold(new Vector2Int(tGroup.mRightX + 1, tY), tH);
            if (tScaffold == null) break;
            if (mAllocated[tGroup.mRightX + 1, tH]) break;
            tGroup.mRightX++;
            tGroup.mScaffoldList.Add(tScaffold);
        }
        return tGroup;
    }
    /// <summary>指定座標の足場を取得</summary>
    static private Scaffold getScaffold(Vector2Int aPosition, int aHeight) {
        //マップサイズの範囲外
        if (mWorld.mSize.x <= aPosition.x) return null;
        //LeftHigh
        MapCell tUpper = mWorld.getCell(aPosition.x, aPosition.y, aHeight * 2 - 1);
        MapCell tLower = mWorld.getCell(aPosition.x, aPosition.y - 1, aHeight * 2 - 1);
        if ((tUpper != null && tUpper.mScaffoldType == MapCell.ScaffoldType.leftHighSlope) ||
            (tLower != null && tLower.mScaffoldType == MapCell.ScaffoldType.leftHighSlope)) {
            ScaffoldSlope tSlope = new ScaffoldSlope();
            if (tUpper != null && tUpper.mScaffoldType == MapCell.ScaffoldType.leftHighSlope)
                tSlope.mUpperCell = tUpper;
            if (tLower != null && tLower.mScaffoldType == MapCell.ScaffoldType.leftHighSlope)
                tSlope.mUpperCell = tLower;
            tSlope.mFlatCell = mWorld.getCell(aPosition.x, aPosition.y - 1, aHeight * 2 - 1);
            return tSlope;
        }
        //RightHigh
        tUpper = mWorld.getCell(aPosition.x, aPosition.y + 1, aHeight * 2 + 1);
        tLower = mWorld.getCell(aPosition.x, aPosition.y, aHeight * 2 + 1);
        if ((tUpper != null && tUpper.mScaffoldType == MapCell.ScaffoldType.rightHighSlope) ||
            (tLower != null && tLower.mScaffoldType == MapCell.ScaffoldType.rightHighSlope)) {
            ScaffoldSlope tSlope = new ScaffoldSlope();
            if (tUpper != null && tUpper.mScaffoldType == MapCell.ScaffoldType.rightHighSlope)
                tSlope.mUpperCell = tUpper;
            if (tLower != null && tLower.mScaffoldType == MapCell.ScaffoldType.rightHighSlope)
                tSlope.mLowerCell = tLower;
            tSlope.mFlatCell = mWorld.getCell(aPosition.x, aPosition.y, aHeight * 2);
            return tSlope;
        }
        //flat
        tUpper = mWorld.getCell(aPosition.x, aPosition.y, aHeight * 2 + 1);
        tLower = mWorld.getCell(aPosition.x, aPosition.y, aHeight * 2);
        if (tUpper != null && tUpper.mScaffoldType == MapCell.ScaffoldType.flat) {
            ScaffoldFlatPiled tPiled = new ScaffoldFlatPiled();
            tPiled.mCell = tLower;
            tPiled.mCellHalfHeight = tUpper;
            return tPiled;
        }
        if (tLower != null) {
            ScaffoldFlat tFlat = new ScaffoldFlat();
            tFlat.mCell = tLower;
            return tFlat;
        }
        return null;
    }
    /// <summary>グループのリストの足場の高さレベルの上下関係を考慮し追加</summary>
    static private void addGroup(ScaffoldLevelGroup aGroup) {
        if (mGroupList == null) {
            mGroupList = new List<ScaffoldLevelGroup>();
            mGroupList.Add(aGroup);
            return;
        }
        ScaffoldLevelComparison tLastComparison = ScaffoldLevelComparison.canJoin;
        int tIndex = 0;
        //低い側から比較していく
        for (; tIndex < mGroupList.Count; ++tIndex) {
            tLastComparison = compareLevelOfGroup(aGroup, mGroupList[tIndex]);
            switch (tLastComparison) {
                case ScaffoldLevelComparison.upper:
                case ScaffoldLevelComparison.canJoin:
                case ScaffoldLevelComparison.canJoinUpper:
                case ScaffoldLevelComparison.canJoinLower:
                    continue;
                case ScaffoldLevelComparison.lower:
                    break;
            }
            break;
        }
        switch (tLastComparison) {
            case ScaffoldLevelComparison.upper:
            case ScaffoldLevelComparison.canJoinUpper:
                mGroupList.Add(aGroup);
                return;
            case ScaffoldLevelComparison.canJoin:
                mGroupList[tIndex - 1].join(aGroup);
                return;
            case ScaffoldLevelComparison.canJoinLower:
            case ScaffoldLevelComparison.lower:
                break;
        }
        //高い側から比較していく
        for (--tIndex; 0 < tIndex; --tIndex) {
            tLastComparison = compareLevelOfGroup(aGroup, mGroupList[tIndex]);
            switch (tLastComparison) {
                case ScaffoldLevelComparison.upper:
                case ScaffoldLevelComparison.canJoin:
                case ScaffoldLevelComparison.canJoinUpper:
                    break;
                case ScaffoldLevelComparison.canJoinLower:
                case ScaffoldLevelComparison.lower:
                    continue;
            }
            break;
        }
        switch (tLastComparison) {
            case ScaffoldLevelComparison.upper:
            case ScaffoldLevelComparison.canJoinUpper:
                mGroupList.Insert(tIndex + 1, aGroup);
                return;
            case ScaffoldLevelComparison.canJoin:
                mGroupList[tIndex].join(aGroup);
                return;
            case ScaffoldLevelComparison.canJoinLower:
            case ScaffoldLevelComparison.lower:
                mGroupList.Insert(0, aGroup);
                return;
        }
    }
    /// <summary>二つのグループの上下関係を比較する</summary>
    static private ScaffoldLevelComparison compareLevelOfGroup(ScaffoldLevelGroup aGroup1, ScaffoldLevelGroup aGroup2) {
        if (aGroup1.mLeftX <= aGroup2.mRightX) {
            if (aGroup2.getHeight(aGroup1.mLeftX) < aGroup1.getHeight(aGroup1.mLeftX))
                return ScaffoldLevelComparison.upper;
            else
                return ScaffoldLevelComparison.lower;
        } else {
            float aDifference = aGroup1.getHeight(aGroup1.mLeftX) - aGroup2.getHeight(aGroup2.mRightX);
            if (aDifference < 0) return ScaffoldLevelComparison.canJoinLower;
            else if (0 < aDifference) return ScaffoldLevelComparison.canJoinUpper;
            return ScaffoldLevelComparison.canJoin;
        }
    }

    private enum ScaffoldLevelComparison {
        upper, lower, canJoin, canJoinUpper, canJoinLower
    }
    private class ScaffoldLevelGroup {
        public int mLeftX;
        public int mRightX;
        public List<Scaffold> mScaffoldList;
        public float getHeight(int aX) {
            return mScaffoldList[aX - mLeftX].getHeight();
        }
        /// <summary>引数のグループを自分に結合する</summary>
        public void join(ScaffoldLevelGroup aGroup) {
            //空きマスにScaffoldEmptyを詰める
            ScaffoldEmpty tEmpty = new ScaffoldEmpty();
            tEmpty.mHeight = aGroup.mScaffoldList[0].getHeight();
            for(int i = aGroup.mLeftX - mRightX - 1; 0 < i; --i) {
                mScaffoldList.Add(tEmpty);
            }
            //結合
            foreach (Scaffold tScaffold in aGroup.mScaffoldList) {
                mScaffoldList.Add(tScaffold);
            }
            mRightX = aGroup.mRightX;
        }
        /// <summary>グループ内のcellに高さレベル設定</summary>
        public void setLevel(float aLevel) {
            foreach (Scaffold tScaffold in mScaffoldList) {
                tScaffold.setLevel(aLevel);
            }
        }
    }
    private abstract class Scaffold {
        public abstract float getHeight();
        public abstract MapCell.ScaffoldType getScaffoldType();
        public abstract void setLevel(float aLevel);
    }
    private class ScaffoldEmpty : Scaffold {
        public float mHeight;
        public override float getHeight() {
            return mHeight;
        }
        public override MapCell.ScaffoldType getScaffoldType() {
            return MapCell.ScaffoldType.none;
        }
        public override void setLevel(float aLevel) {
        }
    }
    private class ScaffoldFlat : Scaffold {
        public MapCell mCell;
        public override float getHeight() {
            return mCell.mHeight;
        }
        public override MapCell.ScaffoldType getScaffoldType() {
            return MapCell.ScaffoldType.flat;
        }
        public override void setLevel(float aLevel) {
            mCell.mScaffoldSurfaceLevel = aLevel;
            mCell.mScaffoldSurfaceLevel2 = aLevel;
            mCell.mScaffoldLevel = aLevel;
            mCell.applyPosition();
        }
    }
    private class ScaffoldFlatPiled : Scaffold {
        public MapCell mCell;
        public MapCell mCellHalfHeight;
        public override float getHeight() {
            return mCellHalfHeight.mHeight;
        }
        public override MapCell.ScaffoldType getScaffoldType() {
            return MapCell.ScaffoldType.flat;
        }
        public override void setLevel(float aLevel) {
            if (mCell != null) {
                mCell.mScaffoldSurfaceLevel = aLevel;
                mCell.mScaffoldSurfaceLevel2 = aLevel;
                mCell.mScaffoldLevel = aLevel;
                mCell.applyPosition();
            }
            mCellHalfHeight.mScaffoldSurfaceLevel = aLevel;
            mCellHalfHeight.mScaffoldSurfaceLevel2 = aLevel;
            mCellHalfHeight.mScaffoldLevel = aLevel;
            mCellHalfHeight.applyPosition();
        }
    }
    private class ScaffoldSlope : Scaffold {
        //坂のcellの上側
        public MapCell mUpperCell;
        //坂のcellの下側
        public MapCell mLowerCell;
        //坂のcellの下側の平面cell
        public MapCell mFlatCell;
        public override float getHeight() {
            if (mUpperCell != null)
                return mUpperCell.mHeight + 0.5f;
            return mLowerCell.mHeight + 0.5f;
        }
        public override MapCell.ScaffoldType getScaffoldType() {
            if (mUpperCell != null)
                return mUpperCell.mScaffoldType;
            return mLowerCell.mScaffoldType;
        }
        public override void setLevel(float aLevel) {
            if (mUpperCell != null) {
                mUpperCell.mScaffoldSurfaceLevel2 = aLevel;
                mUpperCell.applyPosition();
            }
            if (mLowerCell != null) {
                mLowerCell.mScaffoldSurfaceLevel = aLevel;
                mLowerCell.mScaffoldLevel = aLevel;
                mLowerCell.applyPosition();
            }
            if (mFlatCell != null) {
                mFlatCell.mScaffoldSurfaceLevel = aLevel;
                mFlatCell.mScaffoldSurfaceLevel2 = aLevel;
                mFlatCell.mScaffoldLevel = aLevel;
                mFlatCell.applyPosition();
            }
        }
    }
}
