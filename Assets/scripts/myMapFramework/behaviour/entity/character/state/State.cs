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
        public virtual void move(Vector2 aVector){}
        public virtual void examine(){}

        //正面を調べる
        protected void examineFront(){
            //調べる範囲
            Vector2 tSize = parent.mCollider.size;
            if (DirectionOperator.convertToAxis(parent.direction) == VHDirection.horizontal)
                tSize.x = 0.5f;
            else
                tSize.y = 0.5f;
            //調べる範囲の中心
            Vector2 tPoint = parent.position2D + new Vector2(0, parent.mCollider.size.y / 2);
            Vector2 tDirection = DirectionOperator.convertToVector(parent.direction);
            tPoint = tPoint + new Vector2(parent.mCollider.size.x * tDirection.x, parent.mCollider.size.y * tDirection.y) + new Vector2(tSize.x / 2 * tDirection.x, tSize.y / 2 * tDirection.y);


            Collider2D[] tColliders = Physics2D.OverlapBoxAll(tPoint, tSize, 0);
            foreach(Collider2D tCollider in tColliders){
                MapSpeaker tSpeaker = tCollider.GetComponent<MapSpeaker>();
                if (tSpeaker == null) continue;
                if (!parent.mStratum.canCollide(tSpeaker.mStratum)) continue;
                tSpeaker.speack(parent);
            }
        }
    }

    public void jump(Direction aDirection,float aDistance){
        if (mState is JumpingState) return;
        changeState(new JumpingState(this,aDirection,aDistance));
    }
}