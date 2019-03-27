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
            //調べる位置
            Vector2 tTarget=new Vector2();
            switch(parent.direction){
                case Direction.up:
                    tTarget = parent.position2D + new Vector2(0, parent.mCollider.size.y);
                    break;
                case Direction.down:
                    tTarget = parent.position2D + new Vector2(0, -parent.mCollider.size.y);
                    break;
                case Direction.left:
                    tTarget = parent.position2D + new Vector2(-parent.mCollider.size.x, 0);
                    break;
                case Direction.right:
                    tTarget = parent.position2D + new Vector2(parent.mCollider.size.x, 0);
                    break;
            }
            List<Collider2D> tColliders = MyMapPhysics.overlapAll(parent, new MapStratum.ContactFilter(),tTarget);
            foreach(Collider2D tCollider in tColliders){
                MapSpeaker tSpeaker = tCollider.GetComponent<MapSpeaker>();
                if (tSpeaker == null) continue;
                tSpeaker.speack(parent);
                return;
            }
        }
    }

    public void jump(Direction aDirection,float aDistance){
        if (mState is JumpingState) return;
        changeState(new JumpingState(this,aDirection,aDistance));
    }
}