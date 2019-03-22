using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface MyMapEventHandler{
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
}
