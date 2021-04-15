using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrigger : MapTrigger {
    [SerializeField] MyBehaviour mTarget;
    public override void enter(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        mTarget.rotateZBy(360, 1.5f);
    }
}
