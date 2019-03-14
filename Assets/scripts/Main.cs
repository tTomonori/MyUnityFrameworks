using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MyMap map = GameObject.Find("map").gameObject.GetComponent<MyMap>();
        //map.load("debug");
        map.load(new Arg(MyJson.deserializeResourse("save/save")));

        MyMapInputPad pad = GameObject.Find("pad").GetComponent<MyMapInputPad>();
        MyMap.MyMapController controller = new MyMap.MyMapController(map);
        pad.mController = controller;
        map.mController = controller;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
