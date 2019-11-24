using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScaffoldRigide : MyBehaviour {
    public ScaffoldRigideAttribute mAttribute;
    public enum ScaffoldRigideAttribute {
        normal,fly,dug
    }
}
