using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStratum : MyBehaviour {
    //<summary>マスの入れ物</summary>
    public MyBehaviour mCells;
    //<summary>影の入れ物</summary>
    public MyBehaviour mShadows;

    private void Awake() {
        //マスの入れ物
        mCells = MyBehaviour.create<MyBehaviour>();
        mCells.transform.SetParent(this.transform, false);
        mCells.name = "cells";
        //物の入れ物
        mShadows = MyBehaviour.create<MyBehaviour>();
        mShadows.transform.SetParent(this.transform, false);
        mShadows.name = "shadows";
    }
}
