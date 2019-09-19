using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MapBehaviour {
    //<summary>引数のentityが衝突するか</summary>
    public MapPhysicsAttribute.CollisionType canCollide(MapEntity aEntity) {
        //キャラでないなら衝突しない
        if (!(aEntity is MapCharacter))
            return MapPhysicsAttribute.CollisionType.pass;
        //キャラの場合
        MapCharacter tCharacter = (MapCharacter)aEntity;
        return MapPhysicsAttribute.CollisionType.pass;
    }
}
