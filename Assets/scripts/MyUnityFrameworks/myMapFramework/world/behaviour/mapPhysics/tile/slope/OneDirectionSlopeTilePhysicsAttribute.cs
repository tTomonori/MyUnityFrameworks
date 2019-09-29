using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 傾斜の方向が一方向
/// colliderは矩形
/// colliderが矩形が欠けた形の場合は、欠けた部分から侵入不可であること前提
/// </summary>
public class OneDirectionSlopeTilePhysicsAttribute : SlopeTilePhysicsAttribute {
    ///<summary>傾斜の方向</summary>
    [SerializeField] public SlopeDirection mSlopeDirection;
    /// <summary>高い側の高さの小数部分(0~1)</summary>
    [SerializeField] public float mHighSideHeight = 1;
    /// <summary>低い側の高さの小数部分(0~1)</summary>
    [SerializeField] public float mLowSideHeight = 0;
    public enum SlopeDirection {
        upHigh, downHigh, leftHigh, rightHigh, none
    }
    /// <summary>低い側の高さ(絶対座標)</summary>
    public float mAbsoluteLowSideHeight {
        get { return mCell.mHeight + mLowSideHeight; }
    }
    /// <summary>高い側の高さ(絶対座標)</summary>
    public float mAbsoluteHighSideHeight {
        get { return mCell.mHeight + mHighSideHeight; }
    }
    public override float getHeight() {
        return mCell.mHeight + 0.5f;
    }
    public override bool canBeEntered(Vector2 aPosition, float aHeight) {
        switch (getRelativeSide(aPosition)) {
            case Side.lowSide:
                return mAbsoluteLowSideHeight - aHeight < 1;
            case Side.highSide:
                return aHeight - mAbsoluteHighSideHeight < 1;
            case Side.railSide:
                return mAbsoluteLowSideHeight <= aHeight && aHeight <= mAbsoluteHighSideHeight;
            case Side.none:
                Debug.LogWarning("OneDirectionSlopeTilePhysicsAttribute : 傾斜方向が未設定です");
                return true;
            case Side.inner:
                return true;
        }
        return false;
    }
    public override float getPointHeight(Vector2 aPosition, out bool oIsIn) {
        Collider2DEditer.RectangleEndPoint tPoint = mColliderEndPoint;
        switch (mSlopeDirection) {
            case SlopeDirection.upHigh:
                if (aPosition.y < tPoint.down) {
                    oIsIn = false;
                    return mAbsoluteLowSideHeight;
                }
                if (tPoint.up < aPosition.y) {
                    oIsIn = false;
                    return mAbsoluteHighSideHeight;
                }
                oIsIn = tPoint.left <= aPosition.x && aPosition.x <= tPoint.right;
                float tDirectionFromBottom = aPosition.y - tPoint.down;
                return mAbsoluteLowSideHeight + (mHighSideHeight - mLowSideHeight) * tDirectionFromBottom / mColliderSize.y;
            case SlopeDirection.downHigh:
                if (aPosition.y < tPoint.down) {
                    oIsIn = false;
                    return mAbsoluteHighSideHeight;
                }
                if (tPoint.up < aPosition.y) {
                    oIsIn = false;
                    return mAbsoluteLowSideHeight;
                }
                oIsIn = tPoint.left <= aPosition.x && aPosition.x <= tPoint.right;
                float tDirectionFromUp = tPoint.up - aPosition.y;
                return mAbsoluteLowSideHeight + (mHighSideHeight - mLowSideHeight) * tDirectionFromUp / mColliderSize.y;
            case SlopeDirection.leftHigh:
                if (aPosition.x < tPoint.left) {
                    oIsIn = false;
                    return mAbsoluteHighSideHeight;
                }
                if (tPoint.right < aPosition.x) {
                    oIsIn = false;
                    return mAbsoluteLowSideHeight;
                }
                oIsIn = tPoint.down <= aPosition.y && aPosition.y <= tPoint.up;
                float tDirectionFromRight = tPoint.right - aPosition.x;
                return mAbsoluteLowSideHeight + (mHighSideHeight - mLowSideHeight) * tDirectionFromRight / mColliderSize.x;
            case SlopeDirection.rightHigh:
                if (aPosition.x < tPoint.left) {
                    oIsIn = false;
                    return mAbsoluteLowSideHeight;
                }
                if (tPoint.right < aPosition.x) {
                    oIsIn = false;
                    return mAbsoluteHighSideHeight;
                }
                oIsIn = tPoint.down <= aPosition.y && aPosition.y <= tPoint.up;
                float tDirectionFromLeft = aPosition.x - tPoint.left;
                return mAbsoluteLowSideHeight + (mHighSideHeight - mLowSideHeight) * tDirectionFromLeft / mColliderSize.x;
        }
        Debug.LogWarning("OneDirectionSlopeTilePhysicsAttribute : 傾斜方向が未設定です");
        oIsIn = (tPoint.left <= aPosition.x && aPosition.x <= tPoint.right) && (tPoint.down <= aPosition.y && aPosition.y <= tPoint.up);
        return mCell.mHeight;
    }

    //<summary>引数の座標が傾斜に対してどの位置にいるか</summary>
    public Side getRelativeSide(Vector2 aPosition) {
        Collider2DEditer.RectangleEndPoint tPoint = mColliderEndPoint;
        switch (mSlopeDirection) {
            case SlopeDirection.upHigh:
                if (aPosition.y < tPoint.down) return Side.lowSide;
                if (tPoint.up < aPosition.y) return Side.highSide;
                if (aPosition.x < tPoint.left) return Side.railSide;
                if (tPoint.right < aPosition.x) return Side.railSide;
                return Side.inner;
            case SlopeDirection.downHigh:
                if (aPosition.y < tPoint.down) return Side.highSide;
                if (tPoint.up < aPosition.y) return Side.lowSide;
                if (aPosition.x < tPoint.left) return Side.railSide;
                if (tPoint.right < aPosition.x) return Side.railSide;
                return Side.inner;
            case SlopeDirection.leftHigh:
                if (aPosition.x < tPoint.left) return Side.highSide;
                if (tPoint.right < aPosition.x) return Side.lowSide;
                if (aPosition.y < tPoint.down) return Side.railSide;
                if (tPoint.up < aPosition.y) return Side.railSide;
                return Side.inner;
            case SlopeDirection.rightHigh:
                if (aPosition.x < tPoint.left) return Side.lowSide;
                if (tPoint.right < aPosition.x) return Side.highSide;
                if (aPosition.y < tPoint.down) return Side.railSide;
                if (tPoint.up < aPosition.y) return Side.railSide;
                return Side.inner;
        }
        Debug.LogWarning("OneDirectionSlopeTilePhysicsAttribute : 傾斜方向が未設定です");
        return Side.none;
    }
    public enum Side {
        lowSide, highSide, railSide, inner, none
    }
}
