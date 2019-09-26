using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapPhysicsAttribute : MyBehaviour {
    private Collider2D _Collider;
    //<summary>この属性が付与されているbehaviourのcollider</summary>
    public Collider2D mCollider {
        get {
            if (_Collider == null)
                _Collider = GetComponent<Collider2D>();
            return _Collider;
        }
    }
    /// <summary>この属性が付与されているbehaviour</summary>
    public virtual MapBehaviour mBehaviour { get; set; }

    /// <summary>この属性が存在する高さ</summary>
    public virtual float getHeight() {
        return mBehaviour.mHeight;
    }
}
