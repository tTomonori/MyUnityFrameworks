using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    private abstract class State{
        protected MapCharacter parent;
        public State(MapCharacter aParent){
            parent = aParent;
        }
        public virtual void update(){}
        public virtual void enter(){}
        public virtual void exit(){}

        public virtual bool turnAround(Direction aDirection){
            return false;
        }
        public virtual void move(Vector2 aVector,float aSpeed){}
    }

    public void jump(Direction aDirection,float aDistance){
        if (mState is JumpingState) return;
        changeState(new JumpingState(this,aDirection,aDistance));
    }
}
