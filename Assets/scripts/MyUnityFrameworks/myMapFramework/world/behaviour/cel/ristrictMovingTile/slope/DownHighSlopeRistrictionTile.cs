﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownHighSlopeRistrictionTile : RistrictionSlopeTile {
    /// <summary>Y方向の移動距離の増加減少倍率</summary>
    [SerializeField] public float mYSpeedRate;
    public override RistrictMovingData getMovingData(Vector3 aStartPoint, Vector3 aMoveVector) {
        RistrictMovingData tAns = new RistrictMovingData();

        //このtile内部での移動ベクトル
        Vector3 tMovingInSelfVector = new Vector3(aMoveVector.x, aMoveVector.z * mYSpeedRate, aMoveVector.z);

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
