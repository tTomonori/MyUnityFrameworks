using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPhysicsAttribute : MapPhysicsAttribute {
    //<summary>トリガー属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        pass, stop, collide
    }

    //この属性が付与されているtrigger
    [SerializeField] public MapTrigger mTrigger;
    public override MapBehaviour mBehaviour {
        get { return mTrigger; }
        set { mTrigger = (MapTrigger)value; }
    }

    //<summary>引数のentityがこのtriggerに侵入できるか</summary>
    public MapPhysics.CollisionType canBeEntered(EntityPhysicsAttribute aEntityPhysics) {
        return mTrigger.canCollide(aEntityPhysics.mEntity);
    }
}
