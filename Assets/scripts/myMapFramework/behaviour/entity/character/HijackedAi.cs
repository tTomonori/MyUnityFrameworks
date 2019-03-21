using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public class HijackedAi : Ai{
        public HijackedAi(MapCharacter aParent):base(aParent){
            
        }
        //<summary>操作権の乗っ取りを終える</summary>
        public void endHijack(){
            parent.endHijack();
            parent = null;
        }
        //<summary>指定距離移動</summary>
        public void moveBy(Vector2 aDelta,float aSpeed,Action aCallback){
            addMoveByRoutine(aDelta, aSpeed, aCallback);
        }
    }

    //元のAI
    private Ai mOriginalAi;
    //操作権乗っ取り可能か
    private bool mIsCanHijack = true;
    //<summary>キャラの操作権乗っ取る</summary>
    public HijackedAi hijack(){
        if (!mIsCanHijack){
            Debug.Log("HijackedAi : 今はAIの乗っ取り禁止だゾ");
            return null;
        }
        mIsCanHijack = false;
        mOriginalAi = mAi;
        HijackedAi tAi = new HijackedAi(this);
        mAi = tAi;
        return tAi;
    }
    //<summary>操作権の乗っ取りを終える</summary>
    private void endHijack(){
        mAi = mOriginalAi;
        mIsCanHijack = true;
    }
    //<summary>操作権の乗っ取りができるか</summary>
    public bool isCanHijack(){
        return mIsCanHijack;
    }
}
