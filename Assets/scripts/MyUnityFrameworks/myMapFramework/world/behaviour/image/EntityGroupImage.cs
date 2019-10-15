using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGroupImage : MapEntityImage {
    [SerializeField] Mesh2D[] mMeshs;
    public override void shade(ImageEventData aData) {
        foreach(Mesh2D tMesh in mMeshs) {
            tMesh.setColor(new Color(1f - aData.mShadow, 1f - aData.mShadow, 1f - aData.mShadow, 1));
        }
    }
}
