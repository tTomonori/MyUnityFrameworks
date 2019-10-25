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
        //MyMap.mMapResourcesDirectory = "mymap";
        //MapCharaterImageGroup g = MyBehaviour.create<MapCharaterImageGroup>();
        //EntityImageData d = MyBehaviour.createObjectFromResources<EntityImageData>(MyMap.mMapResourcesDirectory+"/character/player/testEntityImage");
        //g.make(d);
        //d.gameObject.AddComponent<MyBehaviour>().delete();

        //pad
        mPad = GameObject.Find("pad").GetComponent<MyPad>();
        //map
        MyMap.mMapResourcesDirectory = "mymap";
        mMap = GameObject.Find("map").GetComponent<MyMap>();
        //delegate
        mDelegate = new TestEventDelegate();
        mMap.mDelegate = mDelegate;

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

        //カメラ(仮)
        mMap.mWorld.mCameraContainer.position2D = GameObject.Find("character:player").GetComponent<MyBehaviour>().position2D;
    }
}