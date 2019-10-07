using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHighSlopeRistrictionTile : RistrictionSlopeTile {
    /// <summary>Y方向に加算する値の、X方向の値に対する割合</summary>
    [SerializeField] public float mAddingYPercentgae;
    public override RistrictMovingData getMovingData(Vector2 aStartPoint, Vector2 aMoveVector) {
        RistrictMovingData tAns = new RistrictMovingData();

        //このtile内部での移動ベクトル
        Vector2 tMovingInSelfVector = new Vector2(aMoveVector.x, aMoveVector.y + aMoveVector.x * mAddingYPercentgae);

        float tRate = calculateRateOfMovingInSelf(aStartPoint, tMovingInSelfVector);

        if (tRate <= 0) {
            tAns.mInternalVector = new Vector2[0];
            tAns.mOutsideVector = aMoveVector;
            tAns.mLastInternalDirection = tMovingInSelfVector;
            return tAns;
        }
        tAns.mInternalVector = new Vector2[1] { tMovingInSelfVector * tRate };
        tAns.mOutsideVector = (1f - tRate) * aMoveVector;
        tAns.mLastInternalDirection = tMovingInSelfVector;
        return tAns;
    }
}
