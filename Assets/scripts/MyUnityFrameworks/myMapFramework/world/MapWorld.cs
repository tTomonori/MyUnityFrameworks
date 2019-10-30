using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorld : MyBehaviour {
    /// <summary>マップデータファイルのパス</summary>
    public string mMapPath;
    /// <summary>ロードしたマップのデータ</summary>
    public MapFileData mFileData;
    /// <summary>ロードしたセーブデータ</summary>
    public MapSaveFileData mSaveData;
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
    /// <summary>マップ周りの壁の入れ物</summary>
    public MyBehaviour mEndContainer;
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
    public List<MapCamera> mCameras = new List<MapCamera>();
    /// <summary>イベント</summary>
    public Dictionary<string, MapEvent> mEvents;

    /// <summary>イベント処理システム</summary>
    public MapEventSystem mEventSystem;
    /// <summary>マップ</summary>
    public MyMap mMap;

    /// <summary>指定座標のCellを取得</summary>
    public MapCell getCell(int aX, int aY, int aZ) {
        if (aX < 0 || mSize.x <= aX) return null;
        if (aY < 0 || mSize.y <= aY) return null;
        if (aZ < 0 || mSize.z <= aZ) return null;
        return mCells[aX, aY, aZ];
    }
    /// <summary>プレイヤーのMapCharacterを取得</summary>
    public MapCharacter getPlayer() {
        foreach (MapCharacter tCharacter in mCharacters) {
            if (tCharacter.isPlayer())
                return tCharacter;
        }
        return null;
    }
    /// <summary>指定名のキャラを取得</summary>
    public MapCharacter getCharacter(string aName) {
        foreach (MapCharacter tCharacter in mCharacters) {
            if (tCharacter.mName == aName)
                return tCharacter;
        }
        return null;
    }

    /// <summary>予約名パスをフラグで使うパスに変換</summary>
    public string toFlagPath(string aPath) {
        if (aPath.StartsWith("worldMap/", System.StringComparison.Ordinal)) {
            return "myMap/world/" + mMapPath + "/" + aPath.Substring(9);
        } else if (aPath.StartsWith("localMap/", System.StringComparison.Ordinal)) {
            return "myMap/local/" + mMapPath + "/" + aPath.Substring(9);
        } else {
            return aPath;
        }
    }
    /// <summary>予約名パスをフラグで使うパスに変換</summary>
    public MyFlagItem toFlagPath(MyFlagItem aItem) {
        if (aItem is MyFlagSingleItem) {
            MyFlagSingleItem tSingle = ((MyFlagSingleItem)aItem).copy();
            tSingle.mPath = toFlagPath(tSingle.mPath);
            return tSingle;
        } else if (aItem is MyFlagAndItems) {
            MyFlagAndItems tAnd = ((MyFlagAndItems)aItem).copy();
            for (int i = 0; i < tAnd.mItems.Length; ++i) {
                tAnd.mItems[i].mPath = toFlagPath(tAnd.mItems[i].mPath);
            }
            return tAnd;
        } else if (aItem is MyFlagOrItems) {
            MyFlagAndItems tOr = ((MyFlagAndItems)aItem).copy();
            for (int i = 0; i < tOr.mItems.Length; ++i) {
                tOr.mItems[i].mPath = toFlagPath(tOr.mItems[i].mPath);
            }
            return tOr;
        }
        throw new System.Exception("MyMap : FlagItemのパス変換失敗「" + aItem.GetType().ToString() + "」");
    }
    /// <summary>フラグチェック</summary>
    public bool checkFlag(MyFlagItem aItem) {
        MyFlagItem tItem = toFlagPath(aItem);
        return mMap.mFlag.check(tItem);
    }

    public void updateWorld() {
        MapWorldUpdater.updateWorld(this);
        //カメラ更新
        foreach(MapCamera tCamera in mCameras) {
            tCamera.updateCamera();
        }
    }
}
