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
    //<summary>移動前の座標</summary>
    public Vector2 mPrePosition;
    //<summary>移動前の階層</summary>
    public int mPreStratumLevel;
}
