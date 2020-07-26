using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaygroundMain : MyBehaviour {

    // Use this for initialization
    void Start() {
        GameObject.Find("g").GetComponent<MyBehaviour>().opacityBy(-1,10,()=> {
            //GameObject.Find("g").GetComponent<MyBehaviour>().opacityBy(1, 1);

            });
    }

    // Update is called once per frame
    void Update() {

    }
}
