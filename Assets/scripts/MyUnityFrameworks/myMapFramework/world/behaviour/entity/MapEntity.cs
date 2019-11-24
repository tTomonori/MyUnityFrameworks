using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntity : MapBehaviour {
    public virtual MapEntityRenderBehaviour mEntityRenderBehaviour { get; set; }

    [SerializeField] public MapEntityPhysicsBehaviour mEntityPhysicsBehaviour;
    public override MapPhysicsBehaviour mPhysicsBehaviour {
        get { return mEntityPhysicsBehaviour; }
        set { mEntityPhysicsBehaviour = (MapEntityPhysicsBehaviour)value; }
    }
}
