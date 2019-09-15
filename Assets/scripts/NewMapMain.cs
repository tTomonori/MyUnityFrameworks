using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapMain : MonoBehaviour {
    MyMap mMap;
    // Start is called before the first frame update
    void Start() {
        MyMap.mMapResourcesDirectory = "mymap";
        mMap = GameObject.Find("map").GetComponent<MyMap>();
        mMap.load("newDebug");
    }

    // Update is called once per frame
    void Update() {

    }
}
