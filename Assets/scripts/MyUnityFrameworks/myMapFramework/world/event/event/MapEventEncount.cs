using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventEncount : MapEvent {
    public string mMapName;
    public string mEncountKey;
    public MapEventEncount(Arg aData) {
        mMapName = aData.get<string>("mapName");
        mEncountKey = aData.get<string>("encountKey");
    }
    public MapEventEncount(string aMapName,string aEncountKey) {
        mMapName = aMapName;
        mEncountKey = aEncountKey;
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        aOperator.mDelegate.onEncount(mMapName, mEncountKey, (aResult) => {
            aCallback(new Arg(new Dictionary<string, object>() { { "result", aResult } }));
;        });
    }
}
