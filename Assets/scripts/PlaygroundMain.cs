using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaygroundMain : MyBehaviour {

    // Use this for initialization
    void Start() {
        MySoundPlayer.playSe("cancel3");
        setTimeout(1, () => { MySoundPlayer.playSe("nandesyou"); });
    }

    // Update is called once per frame
    void Update() {

    }
}
