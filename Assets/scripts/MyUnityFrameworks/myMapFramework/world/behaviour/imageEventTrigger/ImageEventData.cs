using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEventData {
    public float mShadow = 0;
    public Vector2 mShift = Vector2.zero;
    public void shade(float aShadow) {
        if (mShadow < aShadow)
            mShadow = aShadow;
    }
    public void shift(Vector2 aShift) {
        mShift += aShift;
    }
}
