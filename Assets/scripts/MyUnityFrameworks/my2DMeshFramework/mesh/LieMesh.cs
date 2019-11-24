using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LieMesh : Mesh2D {
    [SerializeField] public Vector2 mPivot = new Vector2(0.5f, 0.5f);
    /// <summary>Y軸回転量(度)</summary>
    [SerializeField] public float mRotationY = 0;
    public override void createMesh() {
        mFilter.mesh = new Mesh();
        Mesh tMesh = mFilter.sharedMesh;
        tMesh.name = "LieMesh";

        Vector2 tSize = mSprite.bounds.size;
        float tRad = -mRotationY / 180f * Mathf.PI;
        Vector3[] tVertices = new Vector3[4] {
            new Vector3(-tSize.x*mPivot.x*Mathf.Cos(tRad)-tSize.y*mPivot.y*Mathf.Sin(tRad),tSize.x*mPivot.x*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad),tSize.x*mPivot.x*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad)),
            new Vector3(tSize.x*(1-mPivot.x)*Mathf.Cos(tRad)-tSize.y*mPivot.y*Mathf.Sin(tRad),-tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad),-tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad)),
            new Vector3(-tSize.x*mPivot.x*Mathf.Cos(tRad)+tSize.y*(1-mPivot.y)*Mathf.Sin(tRad),tSize.x*mPivot.x*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad),tSize.x*mPivot.x*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad)),
            new Vector3(tSize.x*(1-mPivot.x)*Mathf.Cos(tRad)+tSize.y*(1-mPivot.y)*Mathf.Sin(tRad),-tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad),-tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad))
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
