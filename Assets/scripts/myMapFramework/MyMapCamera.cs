using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMapCamera : MyBehaviour {
    private ShootingMode mShootingMode = null;

    public void setShootingMode(ShootingMode aMode){
        mShootingMode = aMode;
        mShootingMode.parent = this;
    }
	void LateUpdate () {
        if (mShootingMode == null) return;
        mShootingMode.update();
	}

    public class ShootingMode{
        public MyMapCamera parent;
        public virtual void update(){}
    }
    //<summary>ターゲットを常に画面の真ん中に捉える</summary>
    public class ShootingTarget:ShootingMode{
        private MyBehaviour mTarget;
        public ShootingTarget(MyBehaviour aTarget){
            mTarget = aTarget;
        }
        public override void update(){
            parent.position2D = mTarget.position2D;
        }
    }
}
