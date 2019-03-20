using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMap {
    public class MyMapController{
        public void move(Vector2 aVector){
            MyMap.mPlayer.mMoveDirection = aVector;
        }
        public void inputA(){
            MyMap.mPlayer.mInputA = true;
        }
        public void inputB(){
            MyMap.mPlayer.mInputB = true;
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
