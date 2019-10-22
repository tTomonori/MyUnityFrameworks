using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventGroup : MapEvent {
    public List<MapEvent> mEventList;
    public MapEventGroup(Arg aData) {
        foreach (Arg tData in aData.get<List<Arg>>("group")) {
            mEventList.Add(MapEvent.create(tData));
        }
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        int tNum = mEventList.Count;
        int tEndNum = 0;
        Action f = () => {
            ++tEndNum;
            if (tNum == tEndNum)
                aCallback(new Arg());
        };
        foreach (MapEvent tEvent in mEventList) {
            tEvent.run(aOperator, (aArg) => {
                f();
            });
        }
    }
}
