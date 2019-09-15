using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStratum : MyBehaviour {
    //<summary>階層レベル</summary>
    public int mStratumLevel;
    //<summary>マスの入れ物</summary>
    public MyBehaviour mMapCells;
    //<summary>物の入れ物</summary>
    public MyBehaviour mOrnaments;
    //<summary>キャラの入れ物</summary>
    public MyBehaviour mCharacters;
    //<summary>トリガーの入れ物</summary>
    public MyBehaviour mTriggers;

    private void Awake() {
        //マスの入れ物
        mMapCells = MyBehaviour.create<MyBehaviour>();
        mMapCells.transform.SetParent(this.transform, false);
        mMapCells.name = "cells";
        //物の入れ物
        mOrnaments = MyBehaviour.create<MyBehaviour>();
        mOrnaments.transform.SetParent(this.transform, false);
        mOrnaments.name = "ornaments";
        //キャラの入れ物
        mCharacters = MyBehaviour.create<MyBehaviour>();
        mCharacters.transform.SetParent(this.transform, false);
        mCharacters.name = "charactors";
        //トリガーの入れ物
        mTriggers = MyBehaviour.create<MyBehaviour>();
        mTriggers.transform.SetParent(this.transform, false);
        mTriggers.name = "triggers";
    }
}
