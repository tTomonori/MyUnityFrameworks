using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapBehaviour {
    [SerializeField] private MapBehaviourImageAnimator mImageAnimator;
    [SerializeField] private MapWalker mWalker;
    [SerializeField] private BoxCollider2D mCollider;
    private Ai mAi;
    private State mState;
    private GifAnimator mCharaSprite;
    private Sprite[][] mSprites;
    private Direction mDirection = Direction.down;
    public Direction direction{
        get { return mDirection; }
        set {
            if(mState.turnAround(value))
                mDirection = value;
        }
    }

    //<summary>画像変更</summary>
    public void setSprites(Sprite[][] aSprites){
        mSprites = aSprites;
        mCharaSprite.mIsPlayed = true;
    }
    //<summary>AI変更(文字列でAIを指定)</summary>
    public void setAi(string aAiName){
        mAi = Ai.convertToInstance(aAiName, this);
    }
    private void Awake(){
        mCharaSprite = MyBehaviour.create<GifAnimator>();
        mCharaSprite.transform.SetParent(mImageAnimator.transform, false);
    }
    private void Start(){
        if (mAi == null) mAi = new EmptyAi(this);
        if (mState == null) changeState(new StandingState(this));
    }

    private void changeState(State aState){
        if(mState!=null)
            mState.exit();
        mState = aState;
        mState.enter();
    }
	
	void Update () {
        mAi.update();
        mState.update();
	}
    private int getSpritesIndex(Direction aDirection){
        switch(aDirection){
            case Direction.up:return 3;
            case Direction.down:return 0;
            case Direction.left:return 1;
            case Direction.right:return 2;
            default:return -1;
        }
    }
}
