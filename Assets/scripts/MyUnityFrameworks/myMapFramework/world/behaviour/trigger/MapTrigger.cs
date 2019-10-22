using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MyBehaviour {
    /// <summary>このtriggerが付与されているbehaviour</summary>
    [SerializeField] public MapBehaviour mBehaviour;
    //<summary>引数のentityが衝突するか</summary>
    public virtual MapPhysics.CollisionType canCollide(MapEntity aEntity) {
        //キャラでないなら衝突しない
        if (!(aEntity is MapCharacter))
            return MapPhysics.CollisionType.pass;
        //キャラの場合
        MapCharacter tCharacter = (MapCharacter)aEntity;
        return MapPhysics.CollisionType.pass;
    }
    /// <summary>triggerに侵入した</summary>
    public virtual void enter(MapEntity aEntity, MapEventSystem aEventSystem) { }
    /// <summary>trigger内で移動せずに居座った</summary>
    public virtual void stay(MapEntity aEntity, MapEventSystem aEventSystem) { }
    /// <summary>trigger内で移動した</summary>
    public virtual void moved(MapEntity aEntity, MapEventSystem aEventSystem) { }
    /// <summary>triggerから出て行った</summary>
    public virtual void exit(MapEntity aEntity, MapEventSystem aEventSystem) { }
    /// <summary>マップ生成時から内部にいた</summary>
    public virtual void existInner(MapEntity aEntity) { }
}
