using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collider2DCreator {
    static public void addCollider(GameObject aObject,Arg aData){
        switch(aData.get<string>("type")){
            case "box":
                BoxCollider2D tBox = aObject.AddComponent<BoxCollider2D>();
                tBox.size = new Vector2(aData.get<float>("width"), aData.get<float>("height"));
                break;
        }
    }
}
