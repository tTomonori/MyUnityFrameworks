using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static partial class MyMap {
    static public MyMapEventHandler mEventHandler;
    static public MyMapController mController;
    static public Camera mCamera;
    static public GameObject mDisplay;

    static private MyMapCamera mMyCamera;
    static private MapPlayerCharacter mPlayer;
    static private MapWorld mWorld;

    //セーブデータをロード
    static public void load(Arg aSaveData){
        mWorld = MyBehaviour.create<MapWorld>();
        mWorld.name = "world";
        mWorld.transform.SetParent(mDisplay.transform, false);
        mWorld.load(aSaveData.get<string>("mapName"));
        mPlayer = mWorld.createPlayer(aSaveData.get<Arg>("player"));

        //エンカウント
        MyMapEncountSystem.setCount(aSaveData.get<float>("encount"));

        //カメラ設定
        mMyCamera = mCamera.gameObject.AddComponent<MyMapCamera>();
        mMyCamera.setShootingMode(new MyMapCamera.ShootingTarget(mPlayer));
    }
    static private MapWorld createWorld(string aMapName){
        MapWorld tWorld = MyBehaviour.create<MapWorld>();
        tWorld.name = "world";
        tWorld.load(aMapName);
        return tWorld;
    }
    //マップを移動
    static public void moveMap(MoveMapOptions aOptions,Action aCallback){
        //プレイヤーを移動させる
        if(aOptions.mPlayerOption!=null){
            mPlayer.transform.SetParent(mDisplay.transform);
        }
        //マップの生成し直し
        mWorld.delete();
        mWorld = createWorld(aOptions.mMapName);
        mWorld.transform.SetParent(mDisplay.transform, false);
        //プレイヤーを配置
        if(aOptions.mPlayerOption!=null){
            mWorld.setPlayer(mPlayer, aOptions.mPlayerOption.mPosition, aOptions.mPlayerOption.mStratumNum);
        }
        aCallback();
    }
    //マップ移動の設定
    public class MoveMapOptions{
        public string mMapName;//移動先マップ名
        public PlayerOption mPlayerOption;//プレイヤーの設定
        public class PlayerOption{
            public Vector2 mPosition;
            public int mStratumNum;
        }
    }
}
