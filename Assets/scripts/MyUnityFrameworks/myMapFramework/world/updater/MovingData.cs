using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingData {
    //<summary>移動方向</summary>
    public Vector2 mDirection;
    //<summary>移動速度</summary>
    public float mSpeed;
    //<summary>最長移動距離</summary>
    public float mMaxMoveDistance;
    //<summary>当たり判定を貫通しないように移動できる距離</summary>
    public float mDeltaDistance;
    //<summary>移動前の座標</summary>
    public Vector2 mPrePosition;
    //<summary>移動前の高さ</summary>
    public float mPreHeight;
    //<summary>最後に触れていた階層移動属性のオブジェクト</summary>
    public SlopeTilePhysicsAttribute mCollidedSlope;

    //<summary>同フレーム内で移動できる残りの距離</summary>
    public float mRemainingDistance;
}
