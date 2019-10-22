using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MapEvent {
    /// <summary>MapEventを作成(MapEventRootを作成する場合はcreateRoot()を使うこと)</summary>
    static public MapEvent create(Arg aData) {
        switch (aData.get<string>("type")) {
            case "list":
                return new MapEventList(aData);
            case "group":
                return new MapEventGroup(aData);
            case "encount":
                return new MapEventEncount(aData);
            case "delegate":
                return new MapEventForwardDelegate(aData);
        }
        throw new System.Exception("MapEvent : 不正なイベント名「" + aData.get<string>("type") + "」");
    }
    /// <summary>MapEventを作成</summary>
    static public MapEvent createRoot(Arg aData) {
        if(aData.ContainsKey("type")&& aData.get<string>("type") != "root") {
            //root以外作成
            return create(aData);
        }
        //root作成
        return new MapEventRoot(aData);
    }
    /// <summary>
    /// イベント実行
    /// </summary>
    /// <param name="aOperator">イベント処理するoperator</param>
    /// <param name="aCallback">イベント処理終了時コールバック</param>
    public abstract void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback);
}
