using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEncoutSystem {
    /// <summary>カウントリセット時の最低値</summary>
    public float mMinCount = 700;
    /// <summary>カウントリセット時の最大値</summary>
    public float mMaxCount = 3000;
    /// <summary>残りのカウント</summary>
    public float mCount = 1000;
    /// <summary>エンカウント発火時にtrue</summary>
    public bool mIsFire {
        get { return mCount <= 0; }
    }
    /// <summary>エンカウントkey</summary>
    public string mEncountKey = "";
    /// <summary>残りカウント設定</summary>
    public void setCount(float aCount) {
        mCount = aCount;
    }
    /// <summary>残りカウントリセット</summary>
    public void resetCount() {
        mCount = Random.Range(mMinCount, mMaxCount);
    }
    /// <summary>カウントをギリギリ残した状態にする</summary>
    public void lastCount() {
        mCount = 100;
    }
    /// <summary>カウントを進める(カウントがゼロになったらtrue)</summary>
    public bool count(float aCount,string aEncountKey) {
        mCount -= aCount;
        mEncountKey = aEncountKey;
        return mCount <= 0;
    }
}
