using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapEventSystem {
    public MapWorld mWorld;
    private List<Operator> mWaitingOperators = new List<Operator>();
    public MapEventSystem(MapWorld aWorld) {
        mWorld = aWorld;
    }
    /// <summary>実行待機中のイベントを実行</summary>
    public void runWaitingEvents() {
        foreach (Operator tOperator in mWaitingOperators) {
            tOperator.run();
        }
        mWaitingOperators.Clear();
    }
    /// <summary>
    /// エンカウント発火
    /// </summary>
    /// <returns>イベントの実行ができたらtrue</returns>
    /// <param name="aEncountKey">エンカウントKey</param>
    public bool encount(string aEncountKey) {
        MapEventEncount tEncount = new MapEventEncount(mWorld.mMapName, aEncountKey);
        MapEventRoot tRoot = new MapEventRoot(new List<string>(), true, false, tEncount);
        Operator tOperator = new Operator(this, tRoot);
        tOperator.mInvoker = mWorld.getPlayer();

        return addOperator(tOperator);
    }
    /// <summary>
    /// イベントを待機リストに追加
    /// </summary>
    /// <returns>イベント発火可能ならtrue</returns>
    /// <param name="aEventKey">worldが持つイベントのKey</param>
    /// <param name="aInvoker">起動者</param>
    /// <param name="aInvoked">イベント所持者</param>
    public bool addEvent(string aEventKey, MapCharacter aInvoker, MapBehaviour aInvoked) {
        Operator tOperator = new Operator(this, mWorld.mEvents[aEventKey]);
        tOperator.mInvoker = aInvoker;
        tOperator.mInvoked = aInvoked;

        return addOperator(tOperator);
    }
    /// <summary>
    /// イベントを待機リストに追加
    /// </summary>
    /// <returns>イベント発火可能ならtrue</returns>
    /// <param name="aEvent">実行するイベント</param>
    /// <param name="aInvoker">起動者</param>
    /// <param name="aInvoked">イベント所持者</param>
    public bool addEvent(MapEvent aEvent, MapCharacter aInvoker, MapBehaviour aInvoked) {
        Operator tOperator = new Operator(this, aEvent);
        tOperator.mInvoker = aInvoker;
        tOperator.mInvoked = aInvoked;

        return addOperator(tOperator);
    }
    /// <summary>operatorを待機リストに追加(追加できたら(実行可能なら)true)</summary>
    public bool addOperator(Operator aOperator) {
        //実行不可
        if (!aOperator.jackRequared()) return false;
        //実行可
        mWaitingOperators.Add(aOperator);
        return true;
    }
    /// <summary>プレイヤーのAIをjackする</summary>
    private MapCharacter.JackedAi jackPlayer() {
        return mWorld.getPlayer().jack();
    }
    private MapCharacter.JackedAi jack(string aName) {
        MapCharacter tCharacter = mWorld.getCharacter(aName);
        return tCharacter?.jack();
    }
}

public enum BattleEventResult {
    win, lose, escape, miss, eventEnd1, eventEnd2
}