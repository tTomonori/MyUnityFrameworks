using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStratum : MyBehaviour {
    //<summary>マスの入れ物</summary>
    public MyBehaviour mTiles;
    /// <summary>+0.5階層のマスの入れ物</summary>
    public MyBehaviour mHalfHeightTiles;
    //<summary>影の入れ物</summary>
    public MyBehaviour mShadows;

    private void Awake() {
        //マスの入れ物
        mTiles = MyBehaviour.create<MyBehaviour>();
        mTiles.transform.SetParent(this.transform, false);
        mTiles.name = "tiles";
        //+0.5階層のマスの入れ物
        mHalfHeightTiles = MyBehaviour.create<MyBehaviour>();
        mHalfHeightTiles.transform.SetParent(this.transform, false);
        mHalfHeightTiles.name = "halfHeightTiles";
        //影の入れ物
        mShadows = MyBehaviour.create<MyBehaviour>();
        mShadows.transform.SetParent(this.transform, false);
        mShadows.name = "shadows";
    }
}
