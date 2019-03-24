using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MyDebugLog {
    static public void Log(object o){
        Debug.Log(toString(o));
    }
    static public string toString(object o){
        if(o is Vector2){
            return "Vector2(x: " + ((Vector2)o).x.ToString() + ",y: " + ((Vector2)o).y.ToString() + ")";
        }
        if(o is Vector3){
            return "Vector3(x: " + ((Vector3)o).x.ToString() + ",y: " + ((Vector3)o).y.ToString() + ",z: " + ((Vector3)o).z.ToString() + ")";
        }
        if(o is Arg){
            string s = "";
            foreach(KeyValuePair<string,object> tPair in (Dictionary<string,object>)((Arg)o).dictionary){
                s += "{" + tPair + ":";
                s += toString(tPair.Value);
                s += "}";
            }
            return "Arg{" + s + "}";
        }
        return o.ToString();
    }
}
