using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMap : MyBehaviour {
    ///<summary>マップリソースのディレクトリ(resources/ + X)</summary>
    public static string mMapResourcesDirectory;
    /// <summary>階層を表示するlayerの番号</summary>
    public static int[] mStratumLayerNum = new int[10] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

    ///<summary>操作入力用クラス</summary>
    public MyMapController mController;
    ///<summary>イベント通知先</summary>
    public MyMapEventDelegate mDelegate;
    ///<summary>フラグ</summary>
    public MapFlag mFlag;
    /// <summary>エンカウント</summary>
    public MapEncoutSystem mEncountSystem;

    ///<summary>世界オブジェクト</summary>
    public MapWorld mWorld;
    ///<summary>マップデータ</summary>
    public MapFileData mMapData;
    ///<summary>マップ読み込み</summary>
    public void load(string aFilePath) {
        mEncountSystem = new MapEncoutSystem();
        //マップデータ読み込み
        mMapData = new MapFileData(aFilePath);
        //ワールドを再生成
        if (mWorld != null)
            mWorld.delete();
        mWorld = MapWorldFactory.create(mMapData);
        mWorld.mMap = this;
        mWorld.name = "world";
        mWorld.transform.SetParent(this.transform, false);
    }
    ///<summary>更新</summary>
    public void updateMap() {

    }

    static public Sprite mSquareMask {
        get { return Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/system/squareMask"); }
    }
    static public Sprite mTriangleMask {
        get {return Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/system/triangleMask"); }
    }
}
