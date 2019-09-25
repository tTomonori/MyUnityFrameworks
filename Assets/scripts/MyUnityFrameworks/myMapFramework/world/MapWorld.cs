using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorld : MyBehaviour {
    ///<summary>カメラの入れ物</summary>
    public MyBehaviour mCameraContainer;
    ///<summary>階層の入れ物(マスの入れ物の入れ物)</summary>
    public MyBehaviour mStratumContainer;
    ///<summary>マスの入れ物</summary>
    public MyBehaviour[] mCellContainers;
    ///<summary>entityなど直立した物体の入れ物</summary>
    public MyBehaviour mStandContainer;
    ///<summary>characterの入れ物</summary>
    public MyBehaviour mCharacterContainer;
    ///<summary>ornamentの入れ物</summary>
    public MyBehaviour mOrnamentContainer;
    ///<summary>マスに設定されたオブジェクトの入れ物</summary>
    public MyBehaviour mEntityInTileContainer;
    ///<summary>垂直なマスの入れ物の入れ物</summary>
    public MyBehaviour mStandTileContainer;
    ///<summary>垂直なマスの入れ物</summary>
    public MyBehaviour[] mStandCellContainers;
    /// <summary>トリガーの入れ物</summary>
    public MyBehaviour mTriggerContainer;

    /// <summary>階層の数</summary>
    public int mStratumNum;

    //<summary>キャラのリスト</summary>
    public List<MapCharacter> mCharacters = new List<MapCharacter>();

    private void Update() {
        MapWorldUpdater.updateWorld(this);
    }
}
