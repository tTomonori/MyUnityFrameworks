using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : StateTrigger {
    [SerializeField] private Direction mDirection;
    [SerializeField] private float mDistance;
    protected override void triggered(MapCharacter aCharacter){
        aCharacter.jump(mDirection, mDistance);
    }
}
