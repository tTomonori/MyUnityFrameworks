using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class CastMesh : MyBehaviour {
    public MeshRenderer mRenderer { get; set; }
    public MeshFilter mFilter { get; set; }
    [SerializeField] public Sprite mSprite;
    [SerializeField] public float mWidth = 1;
    [SerializeField] public float mHeight = 1;
    [SerializeField] public Vector2 mDirection = new Vector2(0, 1);
    [SerializeField] public Color mColor = new Color(0, 0, 0, 0.3f);
    private void Awake() {
        initialize();
    }
    private void OnValidate() {
        if (Application.isPlaying) return;
        initialize();
    }
    //初期化
    public void initialize() {
        mRenderer = GetComponent<MeshRenderer>();
        mFilter = GetComponent<MeshFilter>();
        if (mSprite != null) {
            createMesh();
            createMaterial();
        }
    }

    public void createMesh() {
        mFilter.mesh = new Mesh();
        Mesh tMesh = mFilter.sharedMesh;
        tMesh.name = "CastMesh";

        Vector2 tSize = mSprite.bounds.size;
        Vector3[] tVertices = new Vector3[6] {
            new Vector3(-mWidth/2f,0,0),
            new Vector3(mWidth/2f,0,0),
            new Vector3(-mWidth/2f,mHeight,0),
            new Vector3(mWidth/2f,mHeight,0),
            new Vector3(-mWidth/2f+mDirection.x,mDirection.y,mDirection.y),
            new Vector3(mWidth/2f+mDirection.x,mDirection.y,mDirection.y)
            };

        int[] tTriangles;
        if (mDirection.y > 0) {
            tTriangles = new int[24] {
                0,2,1,
                2,3,1,
                2,4,3,
                4,5,3,
                0,1,4,
                4,1,5,
                0,4,2,
                1,3,5
            };
        } else if (mDirection.y < 0) {
            tTriangles = new int[24] {
                0,1,2,
                2,1,3,
                4,2,5,
                2,3,5,
                4,5,0,
                0,5,1,
                4,0,2,
                5,3,1
            };
        } else {
            tTriangles = new int[12] {
                0,2,1,
                2,3,1,
                0,1,2,
                2,1,3
            };
        }

        Vector2[] tUvs = new Vector2[6] {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(1,1)
        };
        tMesh.vertices = tVertices;
        tMesh.uv = tUvs;
        tMesh.triangles = tTriangles;
    }
    public void createMaterial() {
        //mRenderer.material = new Material(Shader.Find("Unlit/Translucent"));
        mRenderer.material = new Material(Shader.Find("My/Cast"));
        mRenderer.sharedMaterial.SetColor("_Color", mColor);
        mRenderer.sharedMaterial.SetTexture("_MainTex", mSprite.texture);
        mRenderer.sharedMaterial.SetVector("_Size", new Vector4(mWidth, mHeight, Mathf.Abs(mDirection.y), 0));
        mRenderer.sharedMaterial.SetVector("_CoefficientX", new Vector4(1 / mWidth, 0, -mDirection.x / mDirection.y / mWidth, 0.5f));
        mRenderer.sharedMaterial.SetVector("_CoefficientY", new Vector4(0, 1 / mHeight, -mDirection.x / mDirection.y / mHeight, 0));
    }
    /// <summary>マテリアルにcolorを設定(マテリアルがcolorプロパティを持っていること前提)</summary>
    public void setColor(Color aColor) {
        mRenderer.sharedMaterial.SetColor("_Color", aColor);
    }
}
