using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MapTrigger {
    public override MapWalker.PassType confirmPassType(MapBehaviour aBehaviour){
        //behaviourなし
        if (aBehaviour == null) return MapWalker.PassType.through;
        //characterではない
        if (!(aBehaviour is MapCharacter)) return MapWalker.PassType.through;
        //characterの場合
        return MapWalker.PassType.stop;
    }
}
