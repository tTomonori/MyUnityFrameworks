using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaygroundMain : MyBehaviour {

    // Use this for initialization
    void Start() {
        SpriteRenderer tRen = GameObject.Find("g").GetComponent<SpriteRenderer>();
        tRen.sprite = SpriteCutter.setRatio(tRen.sprite, 4, 2);
        //tRen.sprite = SpriteCutter.Create(tRen.sprite.texture, new Rect(75, 0, 150, 300), tRen.sprite.pivot, tRen.sprite.pixelsPerUnit);
    }

    // Update is called once per frame
    void Update() {

    }
}
