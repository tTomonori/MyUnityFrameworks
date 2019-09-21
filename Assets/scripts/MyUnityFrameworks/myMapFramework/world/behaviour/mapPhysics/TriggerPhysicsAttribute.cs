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
    private MapTrigger mTrigger;
    //<summary>この属性が付与されているbehaviourがいる階層</summary>
    public override MapStratumLevel getStratumLevel() {
        if (mTrigger == null)
            mTrigger = GetComponent<MapTrigger>();
        return mTrigger.mStratumLevel;
    }
    //<summary>引数のentityがこのtriggerに侵入できるか</summary>
    public MapPhysics.CollisionType canCollide(EntityPhysicsAttribute aEntityPhysics) {
        return mTrigger.canCollide(aEntityPhysics.mEntity);
    }
}
