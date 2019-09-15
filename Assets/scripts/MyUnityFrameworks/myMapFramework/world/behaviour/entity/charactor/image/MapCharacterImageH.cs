using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterImageH : MapCharacterImage {
    //<summary>アニメーションに使うsprite</summary>
    [SerializeField] public Sprite mSprite;
    //<summary>アニメーション用に分割したsprite</summary>
    private Sprite[][] mCutSprite;
    //<summary>画像表示用コンポーネント</summary>
    private SpriteRenderer mRenderer;

    //最後に移動した方向
    private DirectionImageH mLastDirection;
    //最後に表示した画像のindex
    private int mLastImageIndex = -1;

    //<summary>アニメーションできるようにspriteを加工</summary>
    public void processSprite() {
        mCutSprite = SpriteCutter.split(mSprite.texture, new Vector2(100, 100), new Vector2(0.5f, 0));
    }
    private void Awake() {
        mRenderer = gameObject.AddComponent<SpriteRenderer>();
        if (mSprite != null)
            processSprite();
    }

    public override void moved(Vector2 aVector) {
        switch (DirectionOperator.convertToDirectionH(aVector)) {
            case DirectionH.left://左移動
                if(mLastDirection != DirectionImageH.left) {
                    mLastDirection = DirectionImageH.left;
                    mLastImageIndex = -1;
                }
                mLastImageIndex = (mLastImageIndex + 1) % mCutSprite[3].Length;
                mRenderer.sprite = mCutSprite[3][mLastImageIndex];
                return;
            case DirectionH.right://右移動
                if (mLastDirection != DirectionImageH.right) {
                    mLastDirection = DirectionImageH.right;
                    mLastImageIndex = -1;
                }
                mLastImageIndex = (mLastImageIndex + 1) % mCutSprite[2].Length;
                mRenderer.sprite = mCutSprite[2][mLastImageIndex];
                return;
            case DirectionH.none://静止
                if(mLastDirection == DirectionImageH.left) {
                    mLastDirection = DirectionImageH.stayLeft;
                    mLastImageIndex = -1;
                }else if(mLastDirection == DirectionImageH.right) {
                    mLastDirection = DirectionImageH.stayRight;
                    mLastImageIndex = -1;
                }
                if(mLastDirection == DirectionImageH.stayLeft) {
                    mLastImageIndex = (mLastImageIndex + 1) % mCutSprite[1].Length;
                    mRenderer.sprite = mCutSprite[1][mLastImageIndex];
                }else if(mLastDirection == DirectionImageH.stayRight) {
                    mLastImageIndex = (mLastImageIndex + 1) % mCutSprite[0].Length;
                    mRenderer.sprite = mCutSprite[0][mLastImageIndex];
                }
                return;
        }
    }
    public override void setDirection(Vector2 aVector) {
        mLastImageIndex = -1;
        switch (DirectionOperator.convertToDirectionH(aVector)) {
            case DirectionH.left:
                mLastDirection = DirectionImageH.stayLeft;
                mRenderer.sprite = mCutSprite[1][0];
                return;
            case DirectionH.right:
                mLastDirection = DirectionImageH.stayRight;
                mRenderer.sprite = mCutSprite[0][0];
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
        left,right,stayLeft,stayRight,none
    }
}
