using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingData {
    /// <summary>移動方向</summary>
    public Vector3 mDirection;
    /// <summary>移動速度</summary>
    public float mSpeed;
    /// <summary>最長移動距離</summary>
    public float mMaxMoveDistance;
    /// <summary>当たり判定を貫通しないように移動できる距離</summary>
    public float mDeltaDistance;

    /// <summary>移動前の座標</summary>
    public MapPosition mPrePosition;
    /// <summary>最後の移動方向</summary>
    public Vector3 mLastDirection;

    /// <summary>同フレーム内で移動できる残りの距離</summary>
    public float mRemainingDistance;
    /// <summary>移動処理1回前の座標</summary>
    public MapPosition mDeltaPrePosition;
    /// <summary>移動処理1回前に衝突していたtrigger</summary>
    public List<MapTrigger> mCollidedTriggers;

    /// <summary>trueなら話しかけるor調べる行動を行う</summary>
    public bool mSpeak;
    /// <summary>話かけるor調べる距離</summary>
    public float mSpeakDistance = 1f;
}
