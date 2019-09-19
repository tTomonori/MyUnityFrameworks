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
    //<summary>このbehaviourが引数のbehaviourと衝突するか</summary>
    public override CollisionType canCollide(MapPhysicsAttribute aBehaviour) {
        if (aBehaviour is TilePhysicsAttribute)
            return CollisionType.pass;
        if (aBehaviour is EntityPhysicsAttribute)
            return canCollide((EntityPhysicsAttribute)aBehaviour);
        if (aBehaviour is TriggerPhysicsAttribute)
            return CollisionType.pass;

        throw new System.Exception("TriggerPhysicsAttribute : 「" + aBehaviour.GetType().ToString() + "」型との衝突判定が定義されてない");
    }
    //<summary>このbehaviourと引数のentityが衝突するか</summary>
    public CollisionType canCollide(EntityPhysicsAttribute aEntityPhysics) {
        return mTrigger.canCollide(aEntityPhysics.mEntity);
    }
}
