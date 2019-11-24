using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityShadow : MyBehaviour {
    [SerializeField] public float mDefaultDirection = 90;
    [SerializeField] public float mShadowPower = 0.3f;
    [SerializeField] public LeanMesh mMesh;
    private void OnValidate() {
        if (Application.isPlaying) return;

        mMesh = gameObject.GetComponent<LeanMesh>();
        if (mMesh == null) {
            mMesh = gameObject.AddComponent<LeanMesh>();
            mMesh.mRenderMode = Mesh2D.RenderMode.shadow;
        }
        mMesh.mDirection = mDefaultDirection;
        mMesh.mScaleDown = 0.1f;
        mMesh.initialize();
        mMesh.setColor(new Color(0, 0, 0, 0.1f));
    }
}
