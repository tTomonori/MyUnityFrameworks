using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMap : MyBehaviour {
    ///<summary>マップリソースのディレクトリ(resources/ + X)</summary>
    public static string mMapResourcesDirectory;
    /// <summary>階層を表示するlayerの番号</summary>
    public static int[] mStratumLayerNum = new int[10] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
    /// <summary>キャラの移動速度のデフォルト値</summary>
    public static float mDefaultMoveSpeed = 1.5f;

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

    /// <summary>プレイヤーのキャラクターデータ</summary>
    public MapFileData.Npc mPlayerData;

    ///<summary>マップ読み込み</summary>
    public void load(string aFilePath) {
        if (mEncountSystem == null)
            mEncountSystem = new MapEncoutSystem();
        //ワールドを再生成
        if (mWorld != null)
            mWorld.delete();
        mWorld = MapWorldFactory.create(aFilePath);
        mWorld.mMap = this;
        mWorld.name = "world";
        mWorld.transform.SetParent(this.transform, false);
    }
    ///<summary>セーブデータ読み込み</summary>
    public void loadSaveData(string aFilePath) {
        //セーブデータ読み込み
        MapSaveFileData tSaveData = new MapSaveFileData(aFilePath);

        mEncountSystem = new MapEncoutSystem();
        mEncountSystem.setCount(tSaveData.mEncountCount);

        //ワールドを再生成
        if (mWorld != null)
            mWorld.delete();
        mWorld = MapWorldFactory.createFromSave(aFilePath);
        mWorld.mMap = this;
        mWorld.name = "world";
        mWorld.transform.SetParent(this.transform, false);
    }
    /// <summary>マップ移動</summary>
    public void moveMap(MapEventMoveMap aMoveEvent) {
        //マップ再生成
        load(aMoveEvent.mMapPath);
        //移動先座標計算
        if (aMoveEvent.mEndSide.mPercentagePosition != null) {
            MapCharacter tCharacter = MapWorldFactory.createCharacter(mPlayerData);
            aMoveEvent.mEndSide.calculatePositionFromPercentagePosition(tCharacter.mAttribute.mCollider);
            tCharacter.delete();
        }
        //プレイヤー追加
        mPlayerData.mX = aMoveEvent.mEndSide.mPosition.x;
        mPlayerData.mY = aMoveEvent.mEndSide.mPosition.y;
        mPlayerData.mHeight = aMoveEvent.mEndSide.mPosition.z;
        mPlayerData.mDirection = aMoveEvent.mEndSide.mMoveInVector;
        MapWorldFactory.addCharacter(mPlayerData, mWorld);
        //マップ移動後イベント実行
        mWorld.mEventSystem.addMoveMapEventEndSide(aMoveEvent.mEndSide, mWorld.getPlayer());
        mWorld.updateWorld();
    }
    ///<summary>更新</summary>
    private void Update() {
        mWorld.updateWorld();
        mController.resetInput();
    }
    /// <summary>保存</summary>
    public MapSaveFileData save() {
        return MapSaveSystem.save(this);
    }

    static public Sprite mSquareMask {
        get { return Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/system/squareMask"); }
    }
    static public Sprite mTriangleMask {
        get { return Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/system/triangleMask"); }
    }
}
