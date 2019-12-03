using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public abstract class Mesh2D : MyBehaviour {
    public MeshRenderer mRenderer { get; set; }
    public MeshFilter mFilter { get; set; }
    [SerializeField] public RenderMode mRenderMode;
    [SerializeField] public Sprite mSprite;
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

    abstract public void createMesh();
    public void createMaterial() {
        switch (mRenderMode) {
            case RenderMode.opaque:
                mRenderer.material = new Material(Shader.Find("My/Texture"));
                //mRenderer.material = new Material(Shader.Find("Unlit/Texture"));
                mRenderer.sharedMaterial.SetTexture("_MainTex", mSprite.texture);
                break;
            case RenderMode.transparent:
                mRenderer.material = new Material(Shader.Find("My/TransparentWriteZ"));
                mRenderer.sharedMaterial.SetTexture("_MainTex", mSprite.texture);
                break;
            case RenderMode.translucent:
                mRenderer.material = new Material(Shader.Find("My/Translucent"));
                mRenderer.sharedMaterial.SetTexture("_MainTex", mSprite.texture);
                break;
            case RenderMode.shadow:
                mRenderer.material = new Material(Shader.Find("My/Shadow"));
                mRenderer.sharedMaterial.SetTexture("_MainTex", mSprite.texture);
                break;
        }
    }

    /// <summary>マテリアルにcolorを設定(マテリアルがcolorプロパティを持っていること前提)</summary>
    public void setColor(Color aColor) {
        mRenderer.sharedMaterial.SetColor("_TintColor", aColor);
    }

    public enum RenderMode {
        opaque, transparent, translucent, shadow
    }
}
