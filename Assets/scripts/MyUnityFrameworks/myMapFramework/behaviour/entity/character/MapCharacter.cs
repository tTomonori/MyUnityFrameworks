using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapWalker))]
public partial class MapCharacter : MapEntity {
    [SerializeField] private MapBehaviourImageAnimator mImageAnimator;
    private MapWalker mWalker;
    private Ai mAi;
    private State mState;
    private GifAnimator mCharaSprite;
    private Sprite[][] mSprites;
    private Direction mDirection = Direction.down;
    public Direction direction{
        get { return mDirection; }
        set {
            if(mState==null){
                mDirection = value;
                return;
            }
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
    public void setAi(string aAiName,Arg aArg=null){
        mAi = Ai.convertToInstance(aAiName, this, aArg);
    }
    private void Awake(){
        //画像
        mCharaSprite = MyBehaviour.create<GifAnimator>();
        mCharaSprite.transform.SetParent(mImageAnimator.transform, false);
        //移動
        mWalker = gameObject.GetComponent<MapWalker>();
    }

    private void changeState(State aState){
        if(mState!=null)
            mState.exit();
        mState = aState;
        mState.enter();
    }
    private void Start(){
        if (mAi == null) mAi = new EmptyAi(this);
        if (mState == null) changeState(new StandingState(this));
        mAi.start();
    }
	void Update () {
        mAi.updateParFrame();
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