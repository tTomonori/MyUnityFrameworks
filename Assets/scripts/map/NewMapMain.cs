using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewMapMain : MonoBehaviour {
    MyPad mPad;
    MyMap mMap;
    MyMapController mController;
    TestEventDelegate mDelegate;
    // Start is called before the first frame update
    void Start() {
        //pad
        mPad = GameObject.Find("pad").GetComponent<MyPad>();
        //map
        MyMap.mMapResourcesDirectory = "mymap";
        mMap = GameObject.Find("map").GetComponent<MyMap>();
        //delegate
        mDelegate = new TestEventDelegate();
        mMap.mDelegate = mDelegate;
        //player
        MapFileData.Character tPlayerData = new MapFileData.Character(new Arg());
        tPlayerData.mName = "player";
        tPlayerData.mMoveSpeed = 2.5f;
        tPlayerData.mPath = "player/player";
        tPlayerData.mAiString= "<player>";
        mMap.mPlayerData = tPlayerData;

        //mMap.load("meshMap");
        mMap.loadSaveData("save/mapSaveData");

        //contoroller
        mController = new MyMapController();
        mMap.mController = mController;




    }

    void Update() {
        //入力
        mController.mInputVector = mPad.mTailVec * 0.001f;
        mController.mInputA = mPad.mIsTapped;

        //セーブ
        if (Input.GetKeyDown(KeyCode.S)) {
            MapSaveFileData tSave = mMap.save();
            MyJson.serializeToFile(tSave.createDic().dictionary, "Assets/resources/save/mapSaveData.json", true);
        }
    }
}