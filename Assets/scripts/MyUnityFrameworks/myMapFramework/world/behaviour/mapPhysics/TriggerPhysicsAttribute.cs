using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPhysicsAttribute : MapPhysicsAttribute {
    //<summary>トリガー属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        pass,stop,collide
    }
}
