using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeTilePhysicsAttribute : TilePhysicsAttribute {
    //<summary>傾斜の方向</summary>
    [SerializeField] public SlopeDirection mSlopeDirection;

    public enum SlopeDirection {
        upHigh, downHigh, leftHigh, rightHigh,none
    }

    //<summary>引数のentityがこの傾斜に侵入できるか</summary>
    public bool canInvade(EntityPhysicsAttribute aEntity) {
        int tMyLevel = this.getStratumLevel().mLevel;
        int tOpponentLeve = aEntity.getStratumLevel().mLevel;
        //同じ階層にいるなら通過可能
        if (tMyLevel == tOpponentLeve)
            return true;

        Side tSide = getRelativeSide(aEntity);
        if (tOpponentLeve < tMyLevel) {
            //下の階層から侵入
            if (tSide == Side.lowSide) return true;
            return false;
        } else {
            //上の階層から侵入
            if (tSide == Side.highSide) return true;
            return false;
        }
    }

    //<summary>引数のattributeが傾斜に対してどの位置にいるか</summary>
    public Side getRelativeSide(MapPhysicsAttribute aAttribute) {
        ColliderDistance2D tDistance = this.mCollider.Distance(aAttribute.mCollider);
        Vector2 tIntrusionDirection = Quaternion.Euler(0, 0, -this.mCollider.transform.rotation.z) * tDistance.normal;
        switch (mSlopeDirection) {
            case SlopeDirection.upHigh:
                if (tIntrusionDirection.isParallelConsiderdError(new Vector2(1, 0)))
                    return Side.railSide;
                if (tIntrusionDirection.y < 0)
                    return Side.highSide;
                else
                    return Side.lowSide;
            case SlopeDirection.downHigh:
                if (tIntrusionDirection.isParallelConsiderdError(new Vector2(1, 0)))
                    return Side.railSide;
                if (tIntrusionDirection.y < 0)
                    return Side.lowSide;
                else
                    return Side.highSide;
            case SlopeDirection.leftHigh:
                if (tIntrusionDirection.isParallelConsiderdError(new Vector2(0, 1)))
                    return Side.railSide;
                if (tIntrusionDirection.x < 0)
                    return Side.lowSide;
                else
                    return Side.highSide;
            case SlopeDirection.rightHigh:
                if (tIntrusionDirection.isParallelConsiderdError(new Vector2(0, 1)))
                    return Side.railSide;
                if (tIntrusionDirection.x < 0)
                    return Side.highSide;
                else
                    return Side.lowSide;
            default:
                throw new System.Exception("SlopeTilePhysicsAttribute : 未定義の傾斜方向「" + mSlopeDirection.ToString() + "」");
        }
    }
    //<summary>指定座標の高さを返す(この傾斜の最低点を0最高点を1とする)</summary>
    public float getHeight(Vector2 aPosition) {
        float tHeight = 0;
        switch (mSlopeDirection) {
            case SlopeDirection.upHigh:
                tHeight = aPosition.y - mTile.mCell.mMapPosition.y + 0.5f;
                break;
            case SlopeDirection.downHigh:
                tHeight = -aPosition.y + mTile.mCell.mMapPosition.y - 0.5f;
                break;
            case SlopeDirection.leftHigh:
                tHeight = -aPosition.x + mTile.mCell.mMapPosition.x - 0.5f;
                break;
            case SlopeDirection.rightHigh:
                tHeight = aPosition.x - mTile.mCell.mMapPosition.x + 0.5f;
                break;
            default:
                throw new System.Exception("SlopeTilePhysicsAttribute : 未定義の傾斜方向「" + mSlopeDirection.ToString() + "」");
        }
        return mTile.mCell.mStratumLevel.mLevel / 2 + tHeight;
    }

    //傾斜に対する位置
    public enum Side {
        highSide, lowSide, railSide
    }
}
