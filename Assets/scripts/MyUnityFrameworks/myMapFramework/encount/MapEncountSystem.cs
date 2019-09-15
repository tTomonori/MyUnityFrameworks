using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMapEncountSystem {
    //<summary>カウントリセット時の最低値</summary>
    static public float mRestCountMin = 400;
    //<summary>カウントリセット時の最大値</summary>
    static public float mRestCountMax = 1200;
    //<summary>現在のカウント</summary>
    static private float mCount;
    //<summary>指定数カウントを進めるとエンカウントするならtrueを返す</summary>
    static public bool simulate(float aCount) {
        return mCount - aCount < 0;
    }
    //<summary>カウントを進める(エンカウント発火時はカウントをリセットしてtrueを返す)</summary>
    static public bool count(float aCount) {
        mCount -= aCount;
        if (mCount > 0) return false;
        reset();
        return true;
    }
    //<summary>カウントをリセットする</summary>
    static public void reset() {
        mCount = UnityEngine.Random.Range(mRestCountMin, mRestCountMax);
    }
    //<summary>残りのカウント設定</summary>
    static public void setCount(float aCount) {
        mCount = aCount;
    }
}
