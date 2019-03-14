using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMap : MyBehaviour {
    public class MyMapController{
        private MyMap parent;
        public MyMapController(MyMap aParent){
            parent = aParent;
        }
        public void move(Vector2 aVector){
            parent.mPlayer.mMoveDirection = aVector;
        }
        public void inputA(){
            parent.mPlayer.mInputA = true;
        }
        public void inputB(){
            parent.mPlayer.mInputB = true;
        }
        public void play(){
            
        }
        public void pause(){
            
        }
        public bool isCanPause(){
            return true;
        }
        public void wait(){
            
        }
        public void waitEnd(){
            
        }
        public bool isCanWait(){
            return true;
        }
    }
}
