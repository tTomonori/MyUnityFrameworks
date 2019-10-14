using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePhysicsAttribute : MapPhysicsAttribute {
    /// <summary>この属性をもつbehaviourが属しているcell</summary>
    [SerializeField] public MapTile mTile;

    public override MapBehaviour mBehaviour {
        get { return mTile; }
        set { mTile = (MapTile)value; }
    }

    private void OnValidate() {
        mTile = GetComponentInParent<MapTile>();
    }
}
