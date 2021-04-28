using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaygroundMain : MyBehaviour {

    // Use this for initialization
    void Start() {
        MyBehaviour tRen = GameObject.Find("g").GetComponent<MyBehaviour>();
        tRen.sinMoveWithSpeed(new Vector3(3, 3, 0), new Vector3(-3, 3, 0), 0, 2, 1, () => { });
    }

    // Update is called once per frame
    void Update() {

    }
}
