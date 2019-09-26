using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePhysicsAttribute : MapPhysicsAttribute {
    /// <summary>この属性をもつbehaviourが属しているcell</summary>
    [SerializeField] public MapCell mCell;

    public override MapBehaviour mBehaviour {
        get { return mCell; }
        set { mCell = (MapCell)value; }
    }
}
