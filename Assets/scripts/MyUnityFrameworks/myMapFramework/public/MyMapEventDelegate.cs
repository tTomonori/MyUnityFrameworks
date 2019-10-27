using System.Collections;
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
    /// エンカウント
    /// </summary>
    /// <param name="aMapName">エンカウトしたマップのマップ名</param>
    /// <param name="aEncountKey">エンカウントKey</param>
    /// <param name="onEnd">戦闘終了時コールバック</param>
    void onEncount(string aMapName, string aEncountKey, Action<BattleEventResult> aOnEnd);
    /// <summary>
    /// 外部処理のイベント発火
    /// </summary>
    /// <param name="aData">イベントデータ</param>
    /// <param name="onEnd">終了時コールバック(MapEvent:このイベントを実行,string:nextEventsプロパティ内のこのプロパティのイベントを実行,null:実行しない)</param>
    void onEvent(Arg aData, Action<object> aOnEnd);
    /// <summary>
    /// マップ移動通知
    /// </summary>
    /// <param name="aMoveMapEvent">マップ移動イベント情報</param>
    void onMoveMap(MapEventMoveMap aMoveMapEvent);
    /// <summary>
    /// マップ移動時のフェードアウト開始通知
    /// </summary>
    /// <param name="aOnEnd">フェードアウト演出終了時に呼ぶ</param>
    void onMoveMapFadeOut(Action aOnEnd);
    /// <summary>
    /// マップ移動時のフェードイン開始通知
    /// </summary>
    /// <param name="aOnEnd">フェードイン演出終了時に呼ぶ</param>
    void onMoveMapFadeIn(Action aOnEnd);
}
