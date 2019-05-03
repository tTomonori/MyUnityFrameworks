using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapTrigger : MapBehaviour {
    public abstract MapWalker.PassType confirmPassType(MapWalker aBehaviour,Vector2 aPosition);
    public virtual void onEnter(MapStepper aStepper){

    }
    public virtual void onExit(MapStepper aStepper){

    }
    public virtual void onMove(MapStepper aStepper){

    }
}