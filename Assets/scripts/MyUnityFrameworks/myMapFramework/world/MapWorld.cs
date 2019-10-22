using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorld : MyBehaviour {
    /// <summary>マップ名</summary>
    public string mMapName;
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
    /// <summary>イベント</summary>
    public Dictionary<string, MapEvent> mEvents;

    /// <summary>イベント処理システム</summary>
    public MapEventSystem mEventSystem;
    /// <summary>マップ</summary>
    public MyMap mMap;

    /// <summary>指定座標のCellを取得</summary>
    public MapCell getCell(int aX,int aY,int aZ) {
        if (aX < 0 || mSize.x <= aX) return null;
        if (aY < 0 || mSize.y <= aY) return null;
        if (aZ < 0 || mSize.z <= aZ) return null;
        return mCells[aX, aY, aZ];
    }
    /// <summary>プレイヤーのMapCharacterを取得</summary>
    public MapCharacter getPlayer() {
        foreach(MapCharacter tCharacter in mCharacters) {
            if (tCharacter.isPlayer())
                return tCharacter;
        }
        return null;
    }
    /// <summary>指定名のキャラを取得</summary>
    public MapCharacter getCharacter(string aName) {
        foreach (MapCharacter tCharacter in mCharacters) {
            if (tCharacter.name == "character:" + aName)
                return tCharacter;
        }
        return null;
    }

    private void Update() {
        MapWorldUpdater.updateWorld(this);
    }
}
