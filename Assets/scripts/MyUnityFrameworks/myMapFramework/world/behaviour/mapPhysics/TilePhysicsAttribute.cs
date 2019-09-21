using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePhysicsAttribute : MapPhysicsAttribute {
    //<summary>地形属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        none,
        end,
        air,
        flat,
        water,
        magma,
        wall
    }
    //この属性が付与されているtile
    private MapTile mTile;
    //<summary>この属性が付与されているbehaviourがいる階層</summary>
    public override MapStratumLevel getStratumLevel() {
        if (mTile == null)
            mTile = GetComponent<MapTile>();
        return mTile.mCell.mStratumLevel;
    }
}
