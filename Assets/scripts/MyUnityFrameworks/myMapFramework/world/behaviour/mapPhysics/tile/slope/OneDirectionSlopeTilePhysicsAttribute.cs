using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 傾斜の方向が一方向
/// colliderは矩形
/// colliderが矩形が欠けた形の場合は、欠けた部分は侵入不可であること前提
/// </summary>
public class OneDirectionSlopeTilePhysicsAttribute : SlopeTilePhysicsAttribute {
    ///<summary>傾斜の方向</summary>
    [SerializeField] public SlopeDirection mSlopeDirection;
    public enum SlopeDirection {
        upHigh, downHigh, leftHigh, rightHigh, none
    }
    /// <summary>低い側の高さ(マップ座標)</summary>
    public float mLowSideHeight {
        get { return mBehaviour.mMapPosition.y + mColliderEndPoint.bottom; }
    }
    /// <summary>高い側の高さ(マップ座標)</summary>
    public float mHighSideHeight {
        get { return mBehaviour.mMapPosition.y + mColliderEndPoint.top; }
    }
    /// <summary>坂の中間の高さ(マップ座標)</summary>
    public float mMiddleHeight {
        get { return mBehaviour.mMapPosition.y + (mColliderEndPoint.bottom + mColliderEndPoint.top) / 2f; }
    }
    /// <summary>
    /// 指定座標からこの坂道に侵入できるか
    /// </summary>
    /// <returns>侵入できるならtrue</returns>
    /// <param name="aPosition">絶対座標</param>
    public override bool canBeEntered(MapPosition aPosition) {
        switch (getRelativeSide(aPosition)) {
            case Side.lowSide:
                return true;
            case Side.highSide:
                return mHighSideHeight <= aPosition.y;
            case Side.railSide:
                return mLowSideHeight < aPosition.y;
            case Side.none:
                Debug.LogWarning("OneDirectionSlopeTilePhysicsAttribute : 傾斜方向が未設定です");
                return true;
            case Side.inner:
                return true;
        }
        return false;
    }

    //<summary>引数の座標が傾斜に対してどの位置にいるか</summary>
    public Side getRelativeSide(MapPosition aPosition) {
        ColliderEditer.CubeEndPoint tPoint = mColliderEndPointMap;
        switch (mSlopeDirection) {
            case SlopeDirection.upHigh:
                if (aPosition.z < tPoint.front) return Side.lowSide;
                if (tPoint.back < aPosition.z) return Side.highSide;
                if (aPosition.x < tPoint.left) return Side.railSide;
                if (tPoint.right < aPosition.x) return Side.railSide;
                return Side.inner;
            case SlopeDirection.downHigh:
                if (aPosition.z < tPoint.front) return Side.highSide;
                if (tPoint.back < aPosition.y) return Side.lowSide;
                if (aPosition.x < tPoint.left) return Side.railSide;
                if (tPoint.right < aPosition.x) return Side.railSide;
                return Side.inner;
            case SlopeDirection.leftHigh:
                if (aPosition.x < tPoint.left) return Side.highSide;
                if (tPoint.right < aPosition.x) return Side.lowSide;
                if (aPosition.y < tPoint.front) return Side.railSide;
                if (tPoint.back < aPosition.y) return Side.railSide;
                return Side.inner;
            case SlopeDirection.rightHigh:
                if (aPosition.x < tPoint.left) return Side.lowSide;
                if (tPoint.right < aPosition.x) return Side.highSide;
                if (aPosition.y < tPoint.front) return Side.railSide;
                if (tPoint.back < aPosition.y) return Side.railSide;
                return Side.inner;
        }
        Debug.LogWarning("OneDirectionSlopeTilePhysicsAttribute : 傾斜方向が未設定です");
        return Side.none;
    }
    public enum Side {
        lowSide, highSide, railSide, inner, none
    }
}
