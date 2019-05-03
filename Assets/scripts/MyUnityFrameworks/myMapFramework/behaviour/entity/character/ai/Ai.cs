using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public abstract partial class Ai{
        protected MapCharacter parent;
        private List<Action> mMiniRoutine;
        public Ai(MapCharacter aParent){
            parent = aParent;
            mMiniRoutine = new List<Action>();
        }
        public virtual void start(){}
        protected virtual void update(){}
        //フレーム毎の更新
        public void updateParFrame(){
            runMiniRoutine();
            update();
        }
        /// <summary>
        /// ミニルーチン追加
        /// </summary>
        /// <param name="aRoutine">フレーム毎に実行するクロージャ</param>
        /// <param name="aRunThisFrame">trueなら追加したフレーム中にもクロージャを実行する</param>
        protected void addMiniRoutine(Action aRoutine,bool aRunThisFrame = true){
            mMiniRoutine.Add(aRoutine);
            if (aRunThisFrame)
                aRoutine();
        }
        //ミニルーチン削除
        protected bool removeMiniRoutine(Action aRoutine){
            return mMiniRoutine.Remove(aRoutine);
        }
        //ミニルーチン実行
        private void runMiniRoutine(){
            List<Action> tRoutineList = new List<Action>(mMiniRoutine);
            foreach(Action tRoutine in tRoutineList){
                tRoutine();
            }
        }
        //<summary>指定方向に指定速度で、指定距離を超えないように移動する時の移動距離</summary>
        protected Vector2 calculateDistance(Vector2 aVector,float aSpeed,Vector2 aMax){
            //制限なしの場合の移動距離
            Vector2 tDistance = aVector.normalized * aSpeed * Time.deltaTime;
            //移動距離補正
            if ((tDistance.x * aMax.x > 0) && Mathf.Abs(tDistance.x) > Mathf.Abs(aMax.x)){
                tDistance.x = aMax.x;
            }
            if ((tDistance.y * aMax.y > 0) && Mathf.Abs(tDistance.y) > Mathf.Abs(aMax.y)){
                tDistance.y = aMax.y;
            }
            return tDistance;
        }
        //<summary>指定方向に指定速度で移動する時の移動距離</summary>
        protected Vector2 calculateDistance(Vector2 aVector,float aSpeed){
            return aVector.normalized * aSpeed * Time.deltaTime;
        }
        //<summary>指定方向に指定距離移動</summary>
        protected void move(Vector2 aVector){
            parent.mState.move(aVector);
        }
        //<summary>正面を調べる</summary>
        protected void examine(){
            parent.mState.examine();
        }
        //AIのインスタンス生成
        static public Ai convertToInstance(string aAiName,MapCharacter aParent,Arg aArg=null){
            switch(aAiName){
                case "player":return new PlayerAi(aParent);
                case "walkAround":return new WalkAroundAi(aParent, aArg);
                default :return new EmptyAi(aParent);
            }
        }
    }
}
