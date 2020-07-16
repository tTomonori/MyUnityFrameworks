using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyConversationWondow : MyBehaviour {
    private SpriteRenderer mLeftStandRendrer;
    private SpriteRenderer mRightStandRendrer;
    private Action<string> mCallback;
    private Arg mData;
    private List<Arg> mConversationData;
    private int mNextConversationDataIndex;
    //<summary>会話ウィンドウを写しているカメラ</summary>
    [SerializeField] public Camera mCamera;
    //<summary>会話文表示欄</summary>
    [SerializeField] public TextWriter mText;
    //<summary>左の立ち絵</summary>
    [SerializeField] public MyBehaviour mLeftStand;
    //<summary>右の立ち絵</summary>
    [SerializeField] public MyBehaviour mRightStand;
    //<summary>名前表示欄</summary>
    [SerializeField] public TextMesh mNameText;
    //<summary>名前表示欄のウィンドウ</summary>
    [SerializeField] public MyBehaviour mNameWindow;
    private void Start(){
        //mCamera.enabled = false;
        mText.mCallback = onReaded;
    }
    public void on(Arg aData,Action aCallback){
        if (mLeftStand != null) mLeftStandRendrer = mLeftStand.GetComponent<SpriteRenderer>();
        if (mRightStand != null) mRightStandRendrer = mRightStand.GetComponent<SpriteRenderer>();
    }
    //<summary>表示をリセット</summary>
    public void clear(){
        //if (mLeftStand != null) mLeftStand.GetComponent<SpriteRenderer>().sprite = null;
        //if (mRightStand != null) mRightStand.GetComponent<SpriteRenderer>().sprite = null;

        //mNameText.text = "";
    }
    public void run(Arg aData,Action<string> aCallback){
        mCamera.enabled = true;
        mData = aData;
        mCallback = aCallback;
        mConversationData = aData.get<List<Arg>>("conversations");
        mNextConversationDataIndex = 0;
        runNext();
    }
    public void end(){
        mCamera.enabled = (mData.ContainsKey("endClose") && mData.get<bool>("endClose") == false);
        mCallback("");
    }
    //<summary>次の会話データを適用(全て適用済みならfalse)</summary>
    private bool runNext(){
        if (mConversationData.Count == mNextConversationDataIndex) return false;
        runData(mConversationData[mNextConversationDataIndex++]);
        return true;
    }
    private void runData(Arg aData){
        if(aData.ContainsKey("name")){
            
        }
        if(aData.ContainsKey("text")){
            mText.write(aData.get<string>("text"));
        }
    }
    //表示している文章が読み終わった
    public void onReaded(){
        if (!runNext())
            end();
    }
    private void OnMouseUpAsButton(){
        mText.go();
    }
}
