using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewMapMain : MonoBehaviour {
    MyPad mPad;
    MyMap mMap;
    MyMapController mController;
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
        mMap.load("newDebug");
        //contoroller
        mController = new MyMapController();
        mMap.mController = mController;




    }

    // Update is called once per frame
    void Update() {
        mController.mInputVector = mPad.mTailVec * 0.001f;
    }
}
