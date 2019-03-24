using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyConversationWondow {
    private SpriteRenderer mLeftStandRendrer;
    private SpriteRenderer mRightStandRendrer;
    //<summary>会話文表示欄</summary>
    [SerializeField] public TextMesh[] mTexts;
    //<summary>一行に表示する文字の最大数</summary>
    [SerializeField] public int mMaxRowNum;
    //<summary>左の立ち絵</summary>
    [SerializeField] public MyBehaviour mLeftStand;
    //<summary>右の立ち絵</summary>
    [SerializeField] public MyBehaviour mRightStand;
    //<summary>名前表示欄</summary>
    [SerializeField] public TextMesh mNameText;
    //<summary>名前表示欄のウィンドウ</summary>
    [SerializeField] public MyBehaviour mNameWindow;
    public void on(Arg aData,Action aCallback){
        if (mLeftStand != null) mLeftStandRendrer = mLeftStand.GetComponent<SpriteRenderer>();
        if (mRightStand != null) mRightStandRendrer = mRightStand.GetComponent<SpriteRenderer>();
    }
    //<summary>表示をリセット</summary>
    public void clear(){
        if (mLeftStand != null) mLeftStand.GetComponent<SpriteRenderer>().sprite = null;
        if (mRightStand != null) mRightStand.GetComponent<SpriteRenderer>().sprite = null;
        foreach(TextMesh tText in mTexts){
            if (tText == null) continue;
            tText.text = "";
        }
        mNameText.text = "";
    }
}
