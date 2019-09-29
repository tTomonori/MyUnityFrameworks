using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHighSlopeRistrictionTile : RistrictionSlopeTile {
    /// <summary>Y方向に加算する値の、X方向の値に対する割合</summary>
    [SerializeField] public float mAddingYPercentgae;
    public override RistrictMovingData getMovingData(Vector2 aStartPoint, Vector2 aMoveVector) {
        RistrictMovingData tAns = new RistrictMovingData();
        if (aStartPoint.x == 0) {
            tAns.mInternalVector = new Vector2[1] { aMoveVector };
            tAns.mOutsideVector = Vector2.zero;
        }
        float tXDirection = 0;
        if (aStartPoint.x < 0) {
            //左端までのx方向距離
            tXDirection = aStartPoint.x - mColliderEndPoint.left;
        } else if (0 < aStartPoint.x) {
            //右端までのx方向距離
            tXDirection = mColliderEndPoint.right - aStartPoint.x;
        }

        if (tXDirection < Mathf.Abs(aMoveVector.x)) {
            //このtileの範囲外に出る
            tAns.mInternalVector = new Vector2[1] { new Vector2(tXDirection, aMoveVector.y * tXDirection / aMoveVector.x - (tXDirection * mAddingYPercentgae)) };
            tAns.mOutsideVector = aMoveVector * (aMoveVector.x - tXDirection) / aMoveVector.x;
            return tAns;
        }
        //このtileから出ない
        tAns.mInternalVector = new Vector2[1] { new Vector2(aMoveVector.x, aMoveVector.y - aMoveVector.x * mAddingYPercentgae) };
        tAns.mOutsideVector = Vector2.zero;
        return tAns;
    }
}
