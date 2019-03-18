using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBorderStepper : MapBehaviour {
    private Vector2 mPrePosition;
    private Vector2 mCurPosition;
    private List<MapBorder> mColliding = new List<MapBorder>();
    private void Start(){
        mPrePosition = position2D;
        mCurPosition = mPrePosition;
    }
    public void step(){
        mCurPosition = position2D;
        List<MapBorder> tBorders = getCollided<MapBorder>();
        List<MapBorder> tPreCollided = new List<MapBorder>(tBorders);
        mColliding.Clear();
        //現在踏んでいるborder
        foreach(MapBorder tBorder in tBorders){
            if (tPreCollided.Remove(tBorder))
                tBorder.onMove(this, mPrePosition, mCurPosition);
            else
                tBorder.onEnter(this, mPrePosition, mCurPosition);
            mColliding.Add(tBorder);
        }
        //踏み終えたborder
        foreach(MapBorder tBorder in tPreCollided){
            tBorder.onExit(this, mPrePosition, mCurPosition);
        }
        mPrePosition = mCurPosition;
    }
}
