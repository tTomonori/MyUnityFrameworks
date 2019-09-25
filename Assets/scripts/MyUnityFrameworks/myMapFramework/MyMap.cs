using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMap : MyBehaviour {
    ///<summary>マップリソースのディレクトリ(resources/ + X)</summary>
    public static string mMapResourcesDirectory;
    ///<summary>直立した物体を表示するlayerの番号</summary>
    public static int mStandLayerNum = 9;
    /// <summary>マスを表示するlayerの番号</summary>
    public static int[] mStratumLayerNum = new int[10] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

    ///<summary>操作入力用クラス</summary>
    public MyMapController mController;
    ///<summary>イベント通知先</summary>
    public MyMapEventDelegate mDelegate;
    ///<summary>フラグ</summary>
    public MapFlag mFlag;

    ///<summary>世界オブジェクト</summary>
    public MapWorld mWorld;
    ///<summary>マップデータ</summary>
    public MapFileData mMapData;
    ///<summary>マップ読み込み</summary>
    public void load(string aFilePath) {
        //マップデータ読み込み
        mMapData = new MapFileData(aFilePath);
        //ワールドを再生成
        if (mWorld != null)
            mWorld.delete();
        mWorld = MapWorldFactory.create(mMapData);
        mWorld.name = "world";
        mWorld.transform.SetParent(this.transform, false);
    }
    ///<summary>更新</summary>
    public void updateMap() {

    }
}
