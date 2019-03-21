using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject map = GameObject.Find("map");
        Arg tSave = new Arg(MyJson.deserializeResourse("save/save"));

        MyMap.mDisplay = map;

        MyMapInputPad pad = GameObject.Find("pad").GetComponent<MyMapInputPad>();
        MyMap.MyMapController controller = new MyMap.MyMapController();
        pad.mController = controller;
        MyMap.mController = controller;

        MyMap.mCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        MyMap.mEventHandler = new TestHandler();

        MyMap.load(tSave);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class TestHandler:MapEventHandler{
}