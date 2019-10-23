using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventList : MapEvent {
    public List<MapEvent> mEventList = new List<MapEvent>();
    public MapEventList(Arg aData) {
        foreach(Arg tData in aData.get<List<Arg>>("list")) {
            mEventList.Add(MapEvent.create(tData));
        }
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        int tLength = mEventList.Count;
        int i = 0;
        Action f = () => { };
        f = () => {
            if (tLength <= i) {
                aCallback(new Arg());
                return;
            }
            MapEvent tEvent = mEventList[i];
            tEvent.run(aOperator, (aArg) => {
                ++i;
                f();
             });
        };
        f();
    }
}
