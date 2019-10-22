using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventRoot : MapEvent {
    /// <summary>AIジャックするキャラの名前のリスト</summary>
    public List<string> mRequareAi;
    /// <summary>プレイヤーのAIをジャックするか</summary>
    public bool mJackPlayer;
    /// <summary>実行するイベント</summary>
    public MapEvent mEvent;

    public MapEventRoot(Arg aData) {
        mRequareAi = aData.get<List<string>>("requareAi");
        mJackPlayer = (aData.ContainsKey("jackPlayer")) ? aData.get<bool>("jackPlayer") : true;
        mEvent = MapEvent.create(aData.get<Arg>("event"));
    }
    public MapEventRoot(List<string> aRequareAi,bool aJackPlayer,MapEvent aEvent) {
        mRequareAi = aRequareAi;
        mJackPlayer = aJackPlayer;
        mEvent = aEvent;
    }
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        mEvent.run(aOperator, aCallback);
    }
}
