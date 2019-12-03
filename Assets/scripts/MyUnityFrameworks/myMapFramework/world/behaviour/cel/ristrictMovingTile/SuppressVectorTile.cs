using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuppressVectorTile : RistrictMovingTile {
    [SerializeField] public Vector3 mRate = new Vector3(1, 1, 1);
    public override RistrictMovingData getMovingData(Vector3 aStartPoint, Vector3 aMoveVector) {
        RistrictMovingData tAns = new RistrictMovingData();

        //このtile内部での移動ベクトル
        Vector3 tMovingInSelfVector = new Vector3(aMoveVector.x * mRate.x, aMoveVector.y * mRate.y, aMoveVector.z * mRate.z);

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
