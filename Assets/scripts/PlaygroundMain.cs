using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaygroundMain : MyBehaviour {

    // Use this for initialization
    void Start() {
        MyBehaviour tRen = GameObject.Find("g").GetComponent<MyBehaviour>();
        Debug.Log(tRen.transform.localRotation);
        MyBehaviour.setTimeoutToIns(1, () => {
            tRen.rotateZToWithSpeed(100, 10);
        });
    }

    // Update is called once per frame
    void Update() {

    }
}
