using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorld : MyBehaviour {
    ///<summary>カメラの入れ物</summary>
    public MyBehaviour mCameraContainer;
    ///<summary>階層の入れ物</summary>
    public MyBehaviour mField;
    ///<summary>マスの入れ物</summary>
    public MapStratum[] mStratums;
    /// <summary>マス</summary>
    public MapCell[,,] mCells;
    ///<summary>characterの入れ物</summary>
    public MyBehaviour mCharacterContainer;
    ///<summary>ornamentの入れ物</summary>
    public MyBehaviour mOrnamentContainer;
    ///<summary>マスに設定されたオブジェクトの入れ物</summary>
    public MyBehaviour mEntityInCellContainer;
    /// <summary>トリガーの入れ物</summary>
    public MyBehaviour mTriggerContainer;

    /// <summary>マップの大きさ</summary>
    public Vector3Int mSize;

    //<summary>キャラのリスト</summary>
    public List<MapCharacter> mCharacters = new List<MapCharacter>();

    public MapCell getCell(int aX,int aY,int aZ) {
        if (aX < 0 || mSize.x <= aX) return null;
        if (aY < 0 || mSize.y <= aY) return null;
        if (aZ < 0 || mSize.z <= aZ) return null;
        return mCells[aX, aY, aZ];
    }

    private void Update() {
        MapWorldUpdater.updateWorld(this);
    }
}
