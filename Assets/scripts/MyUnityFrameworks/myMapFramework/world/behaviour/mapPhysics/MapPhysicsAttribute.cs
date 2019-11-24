using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapPhysicsAttribute : MyBehaviour {
    ///<summary>この属性が付与されているbehaviourのcollider</summary>
    public Collider mCollider { get; set; }
    /// <summary>この属性が付与されているbehaviour</summary>
    public virtual MapBehaviour mBehaviour { get; set; }

    private void Awake() {
        mCollider = GetComponent<Collider>();
    }
}
