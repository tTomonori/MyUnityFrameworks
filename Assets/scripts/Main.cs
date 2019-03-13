using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MyMap map = GameObject.Find("map").gameObject.GetComponent<MyMap>();
        //map.load("debug");
        map.load(new Arg(MyJson.deserializeResourse("save/save")));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
