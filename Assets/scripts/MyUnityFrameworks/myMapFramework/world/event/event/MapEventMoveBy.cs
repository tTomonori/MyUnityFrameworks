using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventMoveBy : MapEvent {
    public string mTarget;
    public float mSpeed;
    public Vector3 mVector;
    public MapEventMoveBy(Arg aData) {
        mTarget = aData.get<string>("target");
        mSpeed = aData.ContainsKey("speed") ? aData.get<float>("speed") : MyMap.mDefaultMoveSpeed;
        mVector = aData.get<Vector3>("vector");
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        aOperator.getAi(mTarget).moveBy(mVector, mSpeed, () => {
            aCallback(null);
        });
    }
}
