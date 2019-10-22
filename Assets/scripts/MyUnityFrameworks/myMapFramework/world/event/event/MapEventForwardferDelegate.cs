using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventForwardDelegate : MapEvent {
    public Arg mData;
    public MapEventForwardDelegate(Arg aData) {
        mData = aData;
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        throw new Exception("未実装");
    }
}
