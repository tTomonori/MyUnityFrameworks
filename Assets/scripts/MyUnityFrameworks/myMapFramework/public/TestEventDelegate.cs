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
    public void onEncount(string aMapName, string aEncountKey, Action<BattleEventResult> onEnd) {
        Debug.Log("encount:(" + aMapName + "," + aEncountKey + ")");
        MyBehaviour.setTimeoutToIns(3f, () => {
            Debug.Log("end encount battle");
            onEnd(BattleEventResult.win);
        });
    }
}
