using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandMesh : Mesh2D {
    [SerializeField] public float mPivotX = 0.5f;
    public override void createMesh() {
        mFilter.mesh = new Mesh();
        Mesh tMesh = mFilter.sharedMesh;
        tMesh.name = "StandMesh";

        Vector2 tSize = mSprite.bounds.size;
        Vector3[] tVertices = new Vector3[4] {
            new Vector3(-tSize.x*mPivotX,0,0),
            new Vector3(tSize.x*(1-mPivotX),0,0),
            new Vector3(-tSize.x*mPivotX,tSize.y,0),
            new Vector3(tSize.x*(1-mPivotX),tSize.y,0)
        };
        Vector2[] tUvs = new Vector2[4] {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1)
        };
        int[] tTriangles = new int[6] {
            0,2,1,
            3,1,2
        };

        tMesh.vertices = tVertices;
        tMesh.uv = tUvs;
        tMesh.triangles = tTriangles;
    }
}
