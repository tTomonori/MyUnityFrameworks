using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : StateTrigger {
    [SerializeField] private Direction mDirection;
    [SerializeField] private float mDistance;
    protected override void triggered(MapCharacter aCharacter){
        MapEvent.function tEvent = new MapEvent.function((aFunc) =>{
            aCharacter.jump(mDirection, mDistance);
            aFunc();
        });
        mMapWorld.mEventOperator.addEvent(new MapEvent(tEvent));
    }
}
