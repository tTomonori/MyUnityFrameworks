using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MyDebugLog {
    static public void log(object o){
        if(o is Vector2){
            Debug.Log("Vector2(x: " + ((Vector2)o).x.ToString() + ",y: " + ((Vector2)o).y.ToString() + ")");
            return;
        }
        if(o is Vector3){
            Debug.Log("Vector3(x: " + ((Vector3)o).x.ToString() + ",y: " + ((Vector3)o).y.ToString() + ",z: " + ((Vector3)o).z.ToString() + ")");
            return;
        }
    }
}
