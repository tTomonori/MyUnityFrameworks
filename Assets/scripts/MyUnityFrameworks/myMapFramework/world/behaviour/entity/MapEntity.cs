using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntity : MapBehaviour {
    private EntityPhysicsAttribute _Attribute;
    public EntityPhysicsAttribute mAttribute {
        get {
            if (_Attribute == null)
                _Attribute = GetComponent<EntityPhysicsAttribute>();
            return _Attribute;
        }
    }
}
