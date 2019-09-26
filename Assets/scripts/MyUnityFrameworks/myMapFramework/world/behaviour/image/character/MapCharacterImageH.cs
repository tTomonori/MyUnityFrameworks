using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterImageH : MapCharacterImage {
    //<summary>アニメーションに使うsprite</summary>
    [SerializeField] public Sprite mSprite;
    //<summary>アニメーション用に分割したsprite</summary>
    private Sprite[][] mCutSprite;
    //<summary>アニメーション用コンポーネント</summary>
    private GifAnimator mAnimator;

    //最後に移動した方向
    private DirectionImageH mLastDirection;

    //<summary>アニメーションできるようにspriteを加工</summary>
    public void processSprite() {
        mCutSprite = SpriteCutter.split(mSprite.texture, new Vector2(100, 100), new Vector2(0.5f, 0));
    }
    private void Awake() {
        mAnimator = this.createChild<GifAnimator>();
        adaptSpriteRenderer(mAnimator.GetComponent<SpriteRenderer>());
        mAnimator.mIsPlayed = true;
        mAnimator.mInterval = 0.4f;
        if (mSprite != null)
            processSprite();

        base.Awake();
    }

    public override void moved(Vector2 aVector) {
        //垂直移動した場合の対応策
        Vector2 tVector = aVector;
        if (tVector.x == 0 && tVector.y != 0) {
            tVector = new Vector2((mLastDirection == DirectionImageH.left || mLastDirection == DirectionImageH.stayLeft) ? -0.1f : 0.1f, tVector.y);
        }

        switch (DirectionOperator.convertToDirectionH(tVector)) {
            case DirectionH.left://左移動
                if(mLastDirection != DirectionImageH.left) {
                    mLastDirection = DirectionImageH.left;
                    mAnimator.setSprites(mCutSprite[3]);
                    mAnimator.mInterval = 0.2f;
                }
                return;
            case DirectionH.right://右移動
                if (mLastDirection != DirectionImageH.right) {
                    mLastDirection = DirectionImageH.right;
                    mAnimator.setSprites(mCutSprite[2]);
                    mAnimator.mInterval = 0.2f;
                }
                return;
            case DirectionH.none://静止
                if(mLastDirection == DirectionImageH.left) {
                    mLastDirection = DirectionImageH.stayLeft;
                    mAnimator.setSprites(mCutSprite[1]);
                    mAnimator.mInterval = 0.4f;
                } else if(mLastDirection == DirectionImageH.right) {
                    mLastDirection = DirectionImageH.stayRight;
                    mAnimator.setSprites(mCutSprite[0]);
                    mAnimator.mInterval = 0.4f;
                }
                if (mLastDirection == DirectionImageH.stayLeft) {

                }else if(mLastDirection == DirectionImageH.stayRight) {

                }
                return;
        }
    }
    public override void setDirection(Vector2 aVector) {
        switch (DirectionOperator.convertToDirectionH(aVector)) {
            case DirectionH.left:
                mLastDirection = DirectionImageH.stayLeft;
                mAnimator.setSprites(mCutSprite[1]);
                return;
            case DirectionH.right:
                mLastDirection = DirectionImageH.stayRight;
                mAnimator.setSprites(mCutSprite[0]);
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
