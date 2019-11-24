using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGroundPhysicsAttribute : TilePhysicsAttribute {
    //<summary>地形属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        none,
        end,
        air,
        flat,
        water,
        magma,
        edge,
        wall
    }
}
