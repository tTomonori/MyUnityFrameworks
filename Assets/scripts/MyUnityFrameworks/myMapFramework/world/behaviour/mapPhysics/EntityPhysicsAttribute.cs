using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPhysicsAttribute : MapPhysicsAttribute {
    //<summary>物体属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        ghost,
        ornament,
        character,
        flying,
        pygmy
    }

    //<summary>このbehaviourが引数のbehaviourと衝突するか</summary>
    public CollisionType canCollide(MapPhysicsAttribute aBehaviour) {
        return CollisionType.pass;
    }
}
