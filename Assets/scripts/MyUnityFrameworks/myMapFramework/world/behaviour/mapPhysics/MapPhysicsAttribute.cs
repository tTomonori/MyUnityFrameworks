using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapPhysicsAttribute : MonoBehaviour {
    private Collider2D _Collider;
    //<summary>この属性が付与されているbehaviourのcollider</summary>
    public Collider2D mCollider {
        get {
            if (_Collider == null)
                _Collider = GetComponent<Collider2D>();
            return _Collider;
        }
    }
    //<summary>この属性が付与されているbehaviourがいる階層</summary>
    public abstract MapStratumLevel getStratumLevel();
    //<summary>このbehaviourが引数のbehaviourと衝突するか</summary>
    public abstract CollisionType canCollide(MapPhysicsAttribute aBehaviour);

    public enum CollisionType {
        pass,stop,collide
    }
}
