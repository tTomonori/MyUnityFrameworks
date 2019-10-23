using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventRoot : MapEvent {
    /// <summary>AIジャックするキャラの名前のリスト</summary>
    public List<string> mRequareAi;
    public bool mJackInvoker;
    public bool mJackInvoked;
    /// <summary>実行するイベント</summary>
    public MapEvent mEvent;

    public MapEventRoot(Arg aData) {
        if (aData.ContainsKey("requareAi"))
            mRequareAi = aData.get<List<string>>("requareAi");
        else
            mRequareAi = new List<string>();
        mJackInvoker = (aData.ContainsKey("jackInvoker")) ? aData.get<bool>("jackInvoker") : true;
        mJackInvoked = (aData.ContainsKey("jackInvoked")) ? aData.get<bool>("jackInvoked") : true;
        mEvent = MapEvent.create(aData.get<Arg>("event"));
    }
    public MapEventRoot(List<string> aRequareAi, bool aJackInvoker, bool aJackInvoked, MapEvent aEvent) {
        mRequareAi = aRequareAi;
        mJackInvoker = aJackInvoker;
        mJackInvoked = aJackInvoked;
        mEvent = aEvent;
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        mEvent.run(aOperator, aCallback);
    }
}
