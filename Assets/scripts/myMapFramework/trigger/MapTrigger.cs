using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapTrigger : MapBehaviour {
    public abstract MapWalker.PassType confirmPassType(MapBehaviour aBehaviour);
}
