using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GifAnimator : MonoBehaviour {
    private SpriteRenderer mRenderer;
    [SerializeField] private Sprite[] mSprites;
    private bool mChangedSprites = false;
    public int[] mOrder=new int[0];
    public float mInterval = 0.2f;
    public bool mIsPlayed = false;

    public int mOrderIndex = 0;
    private float mDeltaTime = 0;

    private void Awake(){
        mRenderer = gameObject.GetComponent<SpriteRenderer>(); 
    }

    void Update () {
        if (mOrder.Length == 0){
            mOrder = new int[mSprites.Length];
            for (int i = 0; i < mSprites.Length; i++)
                mOrder[i] = i;
        }
        if(mChangedSprites){
            //画像が変更された場合
            mChangedSprites = false;
            mOrderIndex = mOrderIndex % mOrder.Length;
            mRenderer.sprite = mSprites[mOrder[mOrderIndex]];
        }
        if (!mIsPlayed) return;
        if (mInterval <= 0) return;
        //経過時間
        mDeltaTime += Time.deltaTime;
        //画像番号決定
        mOrderIndex = (mOrderIndex + Mathf.FloorToInt(mDeltaTime / mInterval)) % mOrder.Length;
        mDeltaTime = mDeltaTime % mInterval;
        //画像変更
        mRenderer.sprite = mSprites[mOrder[mOrderIndex]];
	}
    public void setSprites(Sprite[] aSprites){
        mSprites = aSprites;
        mChangedSprites = true;
    }
}
