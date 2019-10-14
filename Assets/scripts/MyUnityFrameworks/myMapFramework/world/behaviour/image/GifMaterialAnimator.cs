using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifMaterialAnimator : MonoBehaviour {
    /// <summary>アニメーションに使うテクスチャの矩形範囲の配列</summary>
    [SerializeField] private Rect[] mRects;
    /// <summary>アニメーションさせるマテリアル</summary>
    [SerializeField] public Mesh mMesh;
    /// <summary>画像全体を表示する場合のUV座標</summary>
    [SerializeField] public Vector2[] mCoverUV;
    //アニメーションに使う画像が変更された
    private bool mChangedTextures = false;
    //<summary>画像を表示する順番</summary>
    public int[] mOrder = new int[0];
    //<summary>画像を変更する間隔</summary>
    public float mInterval = 0.2f;
    //<summary>trueならイベントループで自動更新</summary>
    public bool mIsPlayed = false;
    //<summary>表示中の画像のindex</summary>
    public int mOrderIndex = 0;
    //<summary>最後に画像を変更してから経過した時間</summary>
    private float mDeltaTime = 0;

    void Update() {
        //画像の順番が未設定なら、indexの順に設定
        if (mOrder.Length == 0) {
            mOrder = new int[mRects.Length];
            for (int i = 0; i < mRects.Length; i++)
                mOrder[i] = i;
        }

        if (mChangedTextures) {
            //画像が変更された場合
            mChangedTextures = false;
            mOrderIndex = mOrderIndex % mOrder.Length;
            //mMaterial.SetTexture(mTextureNameID, mRects[mOrder[mOrderIndex]]);
            setRect(mRects[mOrder[mOrderIndex]]);
        }

        if (!mIsPlayed) return;
        //更新
        updateImage();
    }
    //<summary>画像を更新</summary>
    public void updateImage() {
        //不正な時間間隔設定
        if (mInterval <= 0) return;
        //経過時間
        mDeltaTime += Time.deltaTime;
        //画像番号決定
        if (mOrder.Length == 0) return;
        mOrderIndex = (mOrderIndex + Mathf.FloorToInt(mDeltaTime / mInterval)) % mOrder.Length;
        mDeltaTime = mDeltaTime % mInterval;
        //画像変更
        //mMaterial.SetTexture(mTextureNameID, mRects[mOrder[mOrderIndex]]);
        setRect(mRects[mOrder[mOrderIndex]]);
    }
    //<summary>画像を変更</summary>
    public void setRects(Rect[] aRects) {
        mRects = aRects;
        mChangedTextures = true;
    }
    /// <summary>meshのuv座標変更</summary>
    public void setRect(Rect aRect) {
        Vector2[] tNewUV = new Vector2[mCoverUV.Length];
        Vector2 tLeftBottom = aRect.min;
        Vector2 tSize = aRect.size;
        for(int i = 0; i < tNewUV.Length; ++i) {
            tNewUV[i] = tLeftBottom + tSize * mCoverUV[i];
        }
        mMesh.uv = tNewUV;
    }
}
