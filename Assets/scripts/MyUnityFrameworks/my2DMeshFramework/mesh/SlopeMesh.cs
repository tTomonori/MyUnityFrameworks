using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeMesh : Mesh2D {
    [SerializeField] public Vector2 mPivot = new Vector2(0.5f, 0.5f);
    [SerializeField] public Vector2 mSize = new Vector2(1, 1);
    /// <summary>坂の方向(ベクトルの先が高い方向)</summary>
    [SerializeField] public Vector2 mDirection = new Vector2(0, 1);
    [SerializeField] public float mLowHeight = 0;
    [SerializeField] public float mHighHeight = 1;
    /// <summary>Y軸回転量(度)</summary>
    [SerializeField] public float mRotationY = 0;
    public override void createMesh() {
        mFilter.mesh = new Mesh();
        Mesh tMesh = mFilter.sharedMesh;
        tMesh.name = "SlopeMesh";

        Vector2 tSize = mSize;
        float tRad = -mRotationY / 180f * Mathf.PI;
        Vector3[] tVertices = new Vector3[4] {
            new Vector3(-tSize.x*mPivot.x*Mathf.Cos(tRad)-tSize.y*mPivot.y*Mathf.Sin(tRad),
                        tSize.x*mPivot.x*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad),
                        tSize.x*mPivot.x*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad)),
            new Vector3(tSize.x*(1-mPivot.x)*Mathf.Cos(tRad)-tSize.y*mPivot.y*Mathf.Sin(tRad),
                        -tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad),
                        -tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)-tSize.y*mPivot.y*Mathf.Cos(tRad)),
            new Vector3(-tSize.x*mPivot.x*Mathf.Cos(tRad)+tSize.y*(1-mPivot.y)*Mathf.Sin(tRad),
                        tSize.x*mPivot.x*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad),
                        tSize.x*mPivot.x*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad)),
            new Vector3(tSize.x*(1-mPivot.x)*Mathf.Cos(tRad)+tSize.y*(1-mPivot.y)*Mathf.Sin(tRad),
                        -tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad),
                        -tSize.x*(1-mPivot.x)*Mathf.Sin(tRad)+tSize.y*(1-mPivot.y)*Mathf.Cos(tRad))
        };

        //高さを考慮し頂点のY座標を調整
        if (mDirection.x > 0) {
            if (mDirection.y > 0) {
                //右上が高い
                float p = (mSize.y / (mSize.y + mSize.x / Mathf.Tan(new Vector2(1, 0).cornerRad(mDirection))));
                tVertices[0] += new Vector3(0, mLowHeight, 0);
                tVertices[1] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * (1 - p), 0);
                tVertices[2] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * p, 0);
                tVertices[3] += new Vector3(0, mHighHeight, 0);
            } else if (mDirection.y < 0) {
                //右下が高い
                float p = (mSize.x / (mSize.x + mSize.y / Mathf.Tan(new Vector2(0, -1).cornerRad(mDirection))));
                tVertices[0] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * (1 - p), 0);
                tVertices[1] += new Vector3(0, mHighHeight, 0);
                tVertices[2] += new Vector3(0, mLowHeight, 0);
                tVertices[3] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * p, 0);
            } else {
                //右が高い
                tVertices[0] += new Vector3(0, mLowHeight, 0);
                tVertices[1] += new Vector3(0, mHighHeight, 0);
                tVertices[2] += new Vector3(0, mLowHeight, 0);
                tVertices[3] += new Vector3(0, mHighHeight, 0);
            }
        } else if (mDirection.x < 0) {
            if (mDirection.y > 0) {
                //左上が高い
                float p = (mSize.y / (mSize.y + mSize.x / Mathf.Tan(mDirection.cornerRad(new Vector2(-1, 0)))));
                tVertices[0] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * (1 - p), 0);
                tVertices[1] += new Vector3(0, mLowHeight, 0);
                tVertices[2] += new Vector3(0, mHighHeight, 0);
                tVertices[3] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * p, 0);
            } else if (mDirection.y < 0) {
                //左下が高い
                float p = (mSize.x / (mSize.x + mSize.y / Mathf.Tan(mDirection.cornerRad(new Vector2(0, 1)))));
                tVertices[0] += new Vector3(0, mHighHeight, 0);
                tVertices[1] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * (1 - p), 0);
                tVertices[2] += new Vector3(0, mLowHeight + (mHighHeight - mLowHeight) * p, 0);
                tVertices[3] += new Vector3(0, mLowHeight, 0);
            } else {
                //左が高い
                tVertices[0] += new Vector3(0, mHighHeight, 0);
                tVertices[1] += new Vector3(0, mLowHeight, 0);
                tVertices[2] += new Vector3(0, mHighHeight, 0);
                tVertices[3] += new Vector3(0, mLowHeight, 0);
            }
        } else {
            if (mDirection.y > 0) {
                //上が高い
                tVertices[0] += new Vector3(0, mLowHeight, 0);
                tVertices[1] += new Vector3(0, mLowHeight, 0);
                tVertices[2] += new Vector3(0, mHighHeight, 0);
                tVertices[3] += new Vector3(0, mHighHeight, 0);
            } else if (mDirection.y < 0) {
                //下が高い
                tVertices[0] += new Vector3(0, mHighHeight, 0);
                tVertices[1] += new Vector3(0, mHighHeight, 0);
                tVertices[2] += new Vector3(0, mLowHeight, 0);
                tVertices[3] += new Vector3(0, mLowHeight, 0);
            } else {
                //傾斜無し
            }
        }

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
