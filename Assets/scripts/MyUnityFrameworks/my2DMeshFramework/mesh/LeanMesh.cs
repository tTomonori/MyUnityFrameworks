using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanMesh : Mesh2D {
    [SerializeField] public Vector2 mPivot = new Vector2(0.5f, 0f);
    /// <summary>影の方向(度)</summary>
    [SerializeField] public float mDirection = 90;
    [SerializeField] public Vector2 mScale = new Vector2(1, 1);
    [SerializeField] public float mScaleUp = 1;
    [SerializeField] public float mScaleDown = 1;
    [SerializeField] public Color mColor = new Color(1, 1, 1, 1);
    public Vector2 mDirectionVector {
        set {
            if (value.x == 0 && value.y == 0) mDirection = 90;
            else {
                mDirection = 180f * Mathf.Atan2(value.y, value.x) / Mathf.PI;
            }
        }
    }
    public override void createMesh() {
        mFilter.mesh = new Mesh();
        Mesh tMesh = mFilter.sharedMesh;
        tMesh.name = "LeanMesh";

        Vector2 tSize = mSprite.bounds.size;
        Vector2 tDirection = Quaternion.Euler(0, 0, mDirection) * new Vector2(tSize.y, 0);
        Vector2 tDirectionH = Mathf.Sin(mDirection / 180f * Mathf.PI)>0 ? new Vector2(tSize.x, 0) : new Vector2(-tSize.x, 0);
        Vector3[] tVertices = new Vector3[6] {
            -tDirection*mPivot.y*mScaleDown-tDirectionH*mPivot.x,
            -tDirection*mPivot.y*mScaleDown+tDirectionH*(1-mPivot.x),
            tDirection*(1-mPivot.y)*mScaleUp-tDirectionH*mPivot.x,
            tDirection*(1-mPivot.y)*mScaleUp+tDirectionH*(1-mPivot.x),
            new Vector3(-tDirectionH.x*mPivot.x,0,0),
            new Vector3(tDirectionH.x*(1-mPivot.x),0,0)
        };
        //scale
        tVertices[0] *= mScale;
        tVertices[1] *= mScale;
        tVertices[2] *= mScale;
        tVertices[3] *= mScale;
        tVertices[4] *= mScale;
        tVertices[5] *= mScale;
        //z
        tVertices[0].z = (tVertices[0].y < 0) ? tVertices[0].y : 0;
        tVertices[1].z = (tVertices[1].y < 0) ? tVertices[1].y : 0;
        tVertices[2].z = (tVertices[2].y < 0) ? tVertices[2].y : 0;
        tVertices[3].z = (tVertices[3].y < 0) ? tVertices[3].y : 0;
        Vector2[] tUvs = new Vector2[6] {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(0,mPivot.y),
            new Vector2(1,mPivot.y)
        };
        int[] tTriangles = new int[12] {
            4,1,0,
            4,5,1,
            2,5,4,
            2,3,5
        };

        tMesh.vertices = tVertices;
        tMesh.uv = tUvs;
        tMesh.triangles = tTriangles;
    }
    public override void createMaterial() {
        base.createMaterial();
        setColor(mColor);
    }
}
