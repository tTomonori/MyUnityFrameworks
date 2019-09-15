using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSlopeTile : MapBehaviour {
    //<summary>傾斜の方向</summary>
    [SerializeField] public SlopeDirection mSlopeDirection;

    public enum SlopeDirection {
        upHigh,downHigh,leftHigh,rightHigh
    }
}
