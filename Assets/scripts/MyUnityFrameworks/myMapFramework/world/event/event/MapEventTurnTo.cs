using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>指定名のキャラの方に振り向く</summary>
public class MapEventTurnTo : MapEvent {
    public string mTarget;
    public string mTo;
    public MapEventTurnTo(Arg aData) {
        mTarget = aData.get<string>("target");
        mTo = aData.get<string>("to");
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        Vector2 tDirection = aOperator.getCharacter(mTo).mMapPosition.vector2 - aOperator.getCharacter(mTarget).mMapPosition.vector2;
        aOperator.getAi(mTarget).turn(tDirection);
        aCallback(null);
    }
}
