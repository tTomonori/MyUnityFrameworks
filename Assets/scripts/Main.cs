using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Collider c = GameObject.Find("map").GetComponent<BoxCollider>();
        ColliderEditer.CubeEndPoint p = c.minimumCircumscribedCubeEndPointWorld();
        Debug.Log(p.top);
    }
}