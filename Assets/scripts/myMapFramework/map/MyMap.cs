using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MyMap {
    static public MapEventHandler mEventHandler;
    static public MyMapController mController;
    static public Camera mCamera;
    static public GameObject mDisplay;

    static private MyMapCamera mMyCamera;
    static private MapPlayerCharacter mPlayer;
    static private MapWorld mWorld;

    static public void load(Arg aSaveData){
        mWorld = MyBehaviour.create<MapWorld>();
        mWorld.transform.SetParent(mDisplay.transform, false);
        mWorld.load(aSaveData.get<string>("mapName"));
        mPlayer = mWorld.createPlayer(aSaveData.get<Arg>("player"));

        //カメラ設定
        mMyCamera = mCamera.gameObject.AddComponent<MyMapCamera>();
        mMyCamera.setShootingMode(new MyMapCamera.ShootingTarget(mPlayer));
    }
}
