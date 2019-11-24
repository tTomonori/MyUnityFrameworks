using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityUnitImage : MapEntityImage{
    [SerializeField] public Mesh2D mMesh;
    /// <summary>影を落とす</summary>
    public override void shade(ImageEventData aData) {
        mMesh.setColor(new Color(1f - aData.mShadow, 1f - aData.mShadow, 1f - aData.mShadow, 1));
    }
}
