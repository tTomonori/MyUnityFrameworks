using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrigger : MapTrigger {
    [SerializeField] MyBehaviour mTarget;
    public override void enter(MapEntity aEntity, MapEventSystem aEventSystem) {
        mTarget.rotateBy(360, 1.5f);
    }
}
