using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventForwardDelegate : MapEvent {
    public Arg mData;
    public Dictionary<string, MapEvent> mEventDic = new Dictionary<string, MapEvent>();
    public MapEventForwardDelegate(Arg aData) {
        mData = aData;
        if (!aData.ContainsKey("nextEvents")) return;
        //このイベントの次に実行するイベントを初期化
        foreach (KeyValuePair<string, object> tPair in aData.get<Arg>("nextEvents").dictionary) {
            mEventDic.Add(tPair.Key, MapEvent.create((Arg)tPair.Value));
        }
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        aOperator.mDelegate.onEvent(mData, (a) => {
            if (a == null) {
                aCallback(null);
                return;
            }
            if (a is string) {
                mEventDic[(string)a].run(aOperator, aCallback);
                return;
            }
            if (a is MapEvent) {
                ((MapEvent)a).run(aOperator, aCallback);
                return;
            }
            Debug.LogWarning("MapEventForwardDelegate : 未定義の戻り値「" + a.GetType().ToString() + "」");
            aCallback(null);
        });
    }
}
