using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePhysicsAttribute : MapPhysicsAttribute {
    //<summary>地形属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        none,
        air,
        flat,
        water,
        magma,
        wall,
        bridge
    }
}
