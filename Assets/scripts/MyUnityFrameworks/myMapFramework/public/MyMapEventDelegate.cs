﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface MyMapEventDelegate {
    /// <summary>
    /// マップでの行動履歴報告
    /// </summary>
    /// <param name="aName">件名</param>
    /// <param name="aData">データ</param>
    void report(string aName, Arg aData);
    /// <summary>
    /// マップ移動イベント発火通知
    /// </summary>
    /// <param name="aData">イベントデータ</param>
    /// <param name="aOnFadeOutEnd">フェードアウト演出完了時に呼ぶ</param>
    void onMoveMap(Arg aData, Action aOnFadeOutEnd);
    /// <summary>
    /// マップ移動イベント時の新しいマップ生成完了通知
    /// </summary>
    /// <param name="aOnFadeInEnd">フェードイン演出終了時に呼ぶ</param>
    void onCreatedMap(Action aOnFadeInEnd);
    /// <summary>
    /// 戦闘イベント発火通知
    /// </summary>
    /// <param name="aData">戦闘データ</param>
    /// <param name="aEndBattle">戦闘終了時に呼ぶ(敗北:0, 勝利:1, 逃走:2, 3~:任意)</param>
    void onBattleStart(Arg aData, Action<int> aEndBattle);
    /// <summary>
    /// 外部で処理するイベントの発火通知
    /// </summary>
    /// <param name="aData">イベントデータ</param>
    /// <param name="aCallback">イベント終了時に呼ぶ</param>
    void onFireOuterEvent(Arg aData, Action<string> aCallback);
}
