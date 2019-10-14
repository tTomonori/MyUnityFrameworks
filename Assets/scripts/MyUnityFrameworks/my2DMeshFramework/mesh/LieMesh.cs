using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LieMesh : Mesh2D {
    [SerializeField] Vector2 mPivot = new Vector2(0.5f, 0.5f);
    public override void createMesh() {
        mFilter.mesh = new Mesh();
        Mesh tMesh = mFilter.sharedMesh;
        tMesh.name = "LieMesh";

        Vector2 tSize = mSprite.bounds.size;
        Vector3[] tVertices = new Vector3[4] {
            new Vector3(-tSize.x*mPivot.x,-tSize.y*mPivot.y,-tSize.y*mPivot.y),
            new Vector3(tSize.x*(1-mPivot.x),-tSize.y*mPivot.y,-tSize.y*mPivot.y),
            new Vector3(-tSize.x*mPivot.x,tSize.y*(1-mPivot.y),tSize.y*(1-mPivot.y)),
            new Vector3(tSize.x*(1-mPivot.x),tSize.y*(1-mPivot.y),tSize.y*(1-mPivot.y))
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
