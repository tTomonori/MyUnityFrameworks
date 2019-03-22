using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateTrigger : MapTrigger {
    public override MapWalker.PassType confirmPassType(MapWalker aBehaviour,Vector2 aPosition){
        if(aBehaviour.cEntity is MapCharacter){
            if (aBehaviour.mAttribute.mAttribute == MapAttribute.Attribute.ghost)
                return MapWalker.PassType.through;
        }
        return MapWalker.PassType.stop;
    }
    public override void onEnter(MapStepper aStepper){
        MapCharacter tCharacter = aStepper.gameObject.GetComponent<MapCharacter>();
        if (tCharacter == null) return;//キャラクターでないならtriggerしない
        triggered(tCharacter);
    }
    protected abstract void triggered(MapCharacter aCharacter);
}
