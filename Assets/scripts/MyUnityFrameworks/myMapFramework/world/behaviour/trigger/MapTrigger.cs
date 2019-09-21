using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MapBehaviour {
    //<summary>引数のentityが衝突するか</summary>
    public MapPhysics.CollisionType canCollide(MapEntity aEntity) {
        //キャラでないなら衝突しない
        if (!(aEntity is MapCharacter))
            return MapPhysics.CollisionType.pass;
        //キャラの場合
        MapCharacter tCharacter = (MapCharacter)aEntity;
        return MapPhysics.CollisionType.pass;
    }
}
