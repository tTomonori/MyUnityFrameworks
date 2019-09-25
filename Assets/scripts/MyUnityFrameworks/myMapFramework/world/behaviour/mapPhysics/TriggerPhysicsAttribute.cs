using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPhysicsAttribute : MapPhysicsAttribute {
    //<summary>トリガー属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        pass,stop,collide
    }

    //この属性が付与されているtrigger
    public new MapTrigger mBehaviour;

    //<summary>引数のentityがこのtriggerに侵入できるか</summary>
    public MapPhysics.CollisionType canBeEntered(EntityPhysicsAttribute aEntityPhysics) {
        return mBehaviour.canCollide(aEntityPhysics.mBehaviour);
    }
}
