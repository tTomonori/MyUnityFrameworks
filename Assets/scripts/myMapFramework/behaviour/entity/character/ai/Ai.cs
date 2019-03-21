using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public abstract class Ai{
        protected MapCharacter parent;
        public Ai(MapCharacter aParent){
            parent = aParent;
        }
        public virtual void update(){}
        protected void move(Vector2 aVector,float aSpeed,Vector2 aMax){
            //移動距離
            Vector2 tDistance = aVector.normalized * aSpeed * Time.deltaTime;
            if (aMax.x == 0 || aMax.y == 0) return;
            if ((tDistance.x * aMax.x > 0) && Mathf.Abs(tDistance.x) > Mathf.Abs(aMax.x)){
                tDistance.x = aMax.x;
            }
            if ((tDistance.y * aMax.y > 0) && Mathf.Abs(tDistance.y) > Mathf.Abs(aMax.y)){
                tDistance.y = aMax.y;
            }
            parent.mState.move(tDistance);
        }
        protected void move(Vector2 aVector,float aSpeed){
            parent.mState.move(aVector.normalized * aSpeed * Time.deltaTime);
        }
        static public Ai convertToInstance(string aAiName,MapCharacter aParent,Arg aArg=null){
            switch(aAiName){
                case "player":return new PlayerAi(aParent);
                case "walkAround":return new WalkAroundAi(aParent, aArg);
                default :return new EmptyAi(aParent);
            }
        }
    }
}
