using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestEventDelegate : MyMapEventDelegate {
    /// <summary>
    /// マップでの行動履歴報告
    /// </summary>
    /// <param name="aName">件名</param>
    /// <param name="aData">データ</param>
    public void report(string aName, Arg aData) {

    }
    /// <summary>
    /// エンカウント
    /// </summary>
    /// <param name="aMapName">エンカウトしたマップのマップ名</param>
    /// <param name="aEncountKey">エンカウントKey</param>
    /// <param name="onEnd">戦闘終了時コールバック</param>
    public void onEncount(string aMapName, string aEncountKey, Action<BattleEventResult> aOnEnd) {
        Debug.Log("encount:(" + aMapName + "," + aEncountKey + ")");
        MyBehaviour.setTimeoutToIns(3f, () => {
            Debug.Log("end encount battle");
            aOnEnd(BattleEventResult.win);
        });
    }
    /// <summary>
    /// 外部処理のイベント発火
    /// </summary>
    /// <param name="aData">イベントデータ</param>
    /// <param name="onEnd">終了時コールバック(MapEvent:このイベントを実行,string:nextEventsプロパティ内のこのプロパティのイベントを実行,null:実行しない)</param>
    public void onEvent(Arg aData, Action<object> aOnEnd) {
        switch (aData.get<string>("name")) {
            case "conversation":
                Debug.Log("conversation");
                MyBehaviour.setTimeoutToIns(2f, () => {
                    Debug.Log("end conversation");
                    aOnEnd(null);
                });
                break;
            default:
                Debug.Log("定義されていない外部処理イベント「" + aData.get<string>("name") + "」");
                aOnEnd(null);
                break;
        }
    }
    /// <summary>
    /// マップ移動通知
    /// </summary>
    /// <param name="aMoveMapEvent">マップ移動イベント情報</param>
    public void onMoveMap(MapEventMoveMap aMoveMapEvent) {

    }
    /// <summary>
    /// マップ移動時のフェードアウト開始通知
    /// </summary>
    /// <param name="aOnEnd">フェードアウト演出終了時に呼ぶ</param>
    public void onMoveMapFadeOut(Action aOnEnd) {
        MyBehaviour.setTimeoutToIns(1f, aOnEnd);
    }
    /// <summary>
    /// マップ移動時のフェードイン開始通知(=マップ再生成完了通知)
    /// </summary>
    /// <param name="aOnEnd">フェードイン演出終了時に呼ぶ</param>
    public void onMoveMapFadeIn(Action aOnEnd) {
        MyBehaviour.setTimeoutToIns(1f, aOnEnd);
    }
}
