using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVirtualPad : MyBehaviour {
    public ControllerType mControllerType = ControllerType.shortTail;
    private Vector2 mDragOriginPoint;
    private bool mDragFlag = false;
    //ドラッグした方向
    private Vector2 mTailVec;
    //タップしたフラグ
    private bool mTapFlag = false;
    //<summary>テールの最大長(shortTailモードの時のみ有効)</summary>
    public float mMaxTailLength = 100;
    //<summary>ドラッグした判定とする最低距離</summary>
    public float mThresholdDrag = 10;
    //<summary>ドラッグした方向</summary>
    public Vector2 mVector{
        get { return mTailVec; }
    }
    //<summary>ドラッグした方向の単位ベクトル</summary>
    public Vector2 mNormalized{
        get { return mTailVec.normalized; }
    }
    //<summary>タップした</summary>
    public bool mIsTapped{
        get { return mTapFlag; }
    }
    protected void OnMouseDrag(){
        Vector2 tMousePosition = Input.mousePosition;
        Vector2 tTailVec = tMousePosition - mDragOriginPoint;
        //ドラッグ距離が短いならドラッグしていない扱い
        if (tTailVec.magnitude < mThresholdDrag) return;
        //ドラッグした
        mDragFlag = true;
        mTailVec = tTailVec;
        //テールの長さ調整
        if(mControllerType==ControllerType.shortTail){
            if(mMaxTailLength<mTailVec.magnitude){
                //ドラッグの基点修正
                mDragOriginPoint = tMousePosition + (mDragOriginPoint - tMousePosition).normalized * mMaxTailLength;
                mTailVec = tMousePosition - mDragOriginPoint;
            }
        }
    }
    protected void OnMouseDown(){
        mDragOriginPoint = Input.mousePosition;
    }
    protected void OnMouseUp(){
        mTailVec = Vector2.zero;
        if (!mDragFlag)
            mTapFlag = true;
        mDragFlag = false;
    }
    private void FixedUpdate(){
        mTapFlag = false;
    }
    public enum ControllerType{
        shortTail,longTail
    }
}
