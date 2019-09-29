using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownHighSlopeRistrictionTile : RistrictionSlopeTile {
    /// <summary>Y方向の移動距離の増加減少倍率</summary>
    [SerializeField] public float mYSpeedRate;
    public override RistrictMovingData getMovingData(Vector2 aStartPoint, Vector2 aMoveVector) {
        RistrictMovingData tAns = new RistrictMovingData();
        if (aStartPoint.y == 0) {
            tAns.mInternalVector = new Vector2[1] { aMoveVector };
            tAns.mOutsideVector = Vector2.zero;
        }
        float tYDirection = 0;
        if (aStartPoint.x < 0) {
            //下端までのx方向距離
            tYDirection = aStartPoint.y - mColliderEndPoint.down;
        } else if (0 < aStartPoint.x) {
            //上端までのx方向距離
            tYDirection = mColliderEndPoint.up - aStartPoint.y;
        }

        float tMoveYDirection = aMoveVector.y * mYSpeedRate;
        if (tYDirection < Mathf.Abs(tMoveYDirection)) {
            //このtileの範囲外に出る
            tAns.mInternalVector = new Vector2[1] { new Vector2(aMoveVector.x * tYDirection / tMoveYDirection, tYDirection) };
            tAns.mOutsideVector = aMoveVector * tYDirection / tMoveYDirection;
            return tAns;
        }
        //このtileから出ない
        tAns.mInternalVector = new Vector2[1] { new Vector2(aMoveVector.x, tMoveYDirection) };
        tAns.mOutsideVector = Vector2.zero;
        return tAns;
    }
}
