using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterImageH : MapCharacterImage {
    //<summary>アニメーションに使うsprite</summary>
    [SerializeField] public Sprite mSprite;
    //<summary>アニメーション用に分割した範囲の配列</summary>
    private Rect[][] mFrameRects;
    //<summary>アニメーション用コンポーネント</summary>
    private GifMaterialAnimator mAnimator;

    //画像を表示するmesh
    public StandMesh mMesh { get; set; }
    //最後に移動した方向
    private DirectionImageH mLastDirection;

    //<summary>アニメーションできるようにrect生成</summary>
    public void processSprite() {
        int tX = Mathf.FloorToInt(mSprite.bounds.size.x);
        int tY = Mathf.FloorToInt(mSprite.bounds.size.y);
        float tXF = tX;
        float tYF = tY;

        mFrameRects = new Rect[tY][];
        for (int i = 0; i < tY; ++i) {
            mFrameRects[i] = new Rect[tX];
            for (int j = 0; j < tX; ++j) {
                mFrameRects[i][j] = new Rect(j / tXF, (tY - i - 1) / tYF, 1f / tXF, 1f / tYF);
            }
        }
    }
    protected void Awake() {
        mAnimator = this.createChild<GifMaterialAnimator>();
        mAnimator.mIsPlayed = true;
        mAnimator.mInterval = 0.4f;
        if (mSprite != null)
            processSprite();
        //mesh生成
        mMesh = gameObject.AddComponent<StandMesh>();
        mMesh.mRenderMode = Mesh2D.RenderMode.transparent;
        mMesh.mSprite = SpriteCutter.split(mSprite.texture, new Vector2(100, 100), new Vector2(0.5f, 0))[0][0];
        mMesh.initialize();
        mAnimator.mCoverUV = mMesh.mFilter.mesh.uv;
        mAnimator.mMesh = mMesh.mFilter.mesh;
    }

    public override void moved(Vector2 aVector) {
        //垂直移動した場合の対応策
        Vector2 tVector = aVector;
        if (tVector.x == 0 && tVector.y != 0) {
            tVector = new Vector2((mLastDirection == DirectionImageH.left || mLastDirection == DirectionImageH.stayLeft) ? -0.1f : 0.1f, tVector.y);
        }

        switch (DirectionOperator.convertToDirectionH(tVector)) {
            case DirectionH.left://左移動
                if (mLastDirection != DirectionImageH.left) {
                    mLastDirection = DirectionImageH.left;
                    mAnimator.setRects(mFrameRects[3]);
                    mAnimator.mInterval = 0.2f;
                }
                return;
            case DirectionH.right://右移動
                if (mLastDirection != DirectionImageH.right) {
                    mLastDirection = DirectionImageH.right;
                    mAnimator.setRects(mFrameRects[2]);
                    mAnimator.mInterval = 0.2f;
                }
                return;
            case DirectionH.none://静止
                if (mLastDirection == DirectionImageH.left) {
                    mLastDirection = DirectionImageH.stayLeft;
                    mAnimator.setRects(mFrameRects[1]);
                    mAnimator.mInterval = 0.4f;
                } else if (mLastDirection == DirectionImageH.right) {
                    mLastDirection = DirectionImageH.stayRight;
                    mAnimator.setRects(mFrameRects[0]);
                    mAnimator.mInterval = 0.4f;
                }
                if (mLastDirection == DirectionImageH.stayLeft) {

                } else if (mLastDirection == DirectionImageH.stayRight) {

                }
                return;
        }
    }
    public override void setDirection(Vector2 aVector) {
        switch (DirectionOperator.convertToDirectionH(aVector)) {
            case DirectionH.left:
                mLastDirection = DirectionImageH.stayLeft;
                mAnimator.setRects(mFrameRects[1]);
                return;
            case DirectionH.right:
            case DirectionH.none:
                mLastDirection = DirectionImageH.stayRight;
                mAnimator.setRects(mFrameRects[0]);
                return;
        }
        mLastDirection = DirectionImageH.none;
    }
    public override Vector2 getDirection() {
        switch (mLastDirection) {
            case DirectionImageH.left:
            case DirectionImageH.stayLeft:
                return new Vector2(-1, 0);
            case DirectionImageH.right:
            case DirectionImageH.stayRight:
                return new Vector2(1, 0);
        }
        return new Vector2(0, 0);
    }

    private enum DirectionImageH {
        left, right, stayLeft, stayRight, none
    }
    /// <summary>影を落とす</summary>
    public override void shade(ImageEventData aData) {
        mMesh.setColor(new Color(1f - aData.mShadow, 1f - aData.mShadow, 1f - aData.mShadow,1));
    }
}
