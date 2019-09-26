using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorld : MyBehaviour {
    ///<summary>カメラの入れ物</summary>
    public MyBehaviour mCameraContainer;
    ///<summary>階層の入れ物</summary>
    public MyBehaviour mStratumContainer;
    ///<summary>マスの入れ物</summary>
    public MyBehaviour[] mStratums;
    ///<summary>characterの入れ物</summary>
    public MyBehaviour mCharacterContainer;
    ///<summary>ornamentの入れ物</summary>
    public MyBehaviour mOrnamentContainer;
    ///<summary>マスに設定されたオブジェクトの入れ物</summary>
    public MyBehaviour mEntityInCellContainer;
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
