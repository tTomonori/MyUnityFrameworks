using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStepper : MapBehaviour {
    private Vector2 mCurPosition;
    private Vector2 mPreStepPosition;
    private List<MapTrigger> mColliding = new List<MapTrigger>();
    public Vector2 curPosition{
        get { return mCurPosition; }
    }
    public Vector2 preStepPosition{
        get { return mPreStepPosition; }
    }
    public void Start(){
        mCurPosition = position2D;
        mPreStepPosition = mCurPosition;
    }
    public void step(){
        mCurPosition = position2D;
        List<MapTrigger> tColliding = getCollided<MapTrigger>();
        List<MapTrigger> tPreCollided = new List<MapTrigger>(mColliding);
        mColliding.Clear();
        //現在踏んでいるborder
        foreach(MapTrigger tTrigger in tColliding){
            if (tPreCollided.Remove(tTrigger))
                tTrigger.onMove(this);
            else
                tTrigger.onEnter(this);
            mColliding.Add(tTrigger);
        }
        //踏み終えたborder
        foreach(MapTrigger tTrigger in tPreCollided){
            tTrigger.onExit(this);
        }
        mPreStepPosition = mCurPosition;
    }
}
