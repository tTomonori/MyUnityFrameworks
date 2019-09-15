using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMapController {
    //<summary>入力1</summary>
    public bool mInputA = false;
    //<summary>入力1長押し</summary>
    public bool mInputAd = false;
    //<summary>入力2</summary>
    public bool mInputB = false;
    //<summary>入力2長押し</summary>
    public bool mInputBd = false;
    //<summary>ベクトル入力</summary>
    public Vector2? mInputVector = null;
    //<summary>入力リセット</summary>
    public void resetInput() {
        mInputA = false;
        mInputAd = false;
        mInputB = false;
        mInputBd = false;
        mInputVector = null;
    }
}
