using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>指定方向に振り向く</summary>
public class MapEventTurn : MapEvent {
    public Vector2 mDirection;
    public string mTarget;
    public MapEventTurn(Arg aData) {
        mDirection = aData.get<Vector2>("direction");
        mTarget = aData.get<string>("target");
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        aOperator.getAi(mTarget).turn(mDirection);
        aCallback(null);
    }
}
