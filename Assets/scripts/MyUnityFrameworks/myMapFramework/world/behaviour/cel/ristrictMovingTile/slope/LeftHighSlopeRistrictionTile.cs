using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHighSlopeRistrictionTile : RistrictionSlopeTile {
    /// <summary>Y方向に加算する値の、X方向の値に対する割合</summary>
    [SerializeField] public float mAddingYPercentgae;
    public override RistrictMovingData getMovingData(Vector3 aStartPoint, Vector3 aMoveVector) {
        RistrictMovingData tAns = new RistrictMovingData();

        //このtile内部での移動ベクトル
        Vector3 tMovingInSelfVector = new Vector3(aMoveVector.x, -aMoveVector.x * mAddingYPercentgae, aMoveVector.z);

        float tRate = calculateRateOfMovingInSelf(aStartPoint, tMovingInSelfVector);

        if (tRate <= 0) {
            tAns.mInternalVector = new Vector3[0];
            tAns.mOutsideVector = aMoveVector;
            tAns.mLastInternalDirection = tMovingInSelfVector;
            return tAns;
        }
        tAns.mInternalVector = new Vector3[1] { tMovingInSelfVector * tRate };
        tAns.mOutsideVector = (1f - tRate) * aMoveVector;
        tAns.mLastInternalDirection = tMovingInSelfVector;
        return tAns;
    }
}
