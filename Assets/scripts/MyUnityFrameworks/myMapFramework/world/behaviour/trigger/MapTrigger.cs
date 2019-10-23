using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MyBehaviour {
    /// <summary>このtriggerが付与されているbehaviour</summary>
    [SerializeField] public MapBehaviour mBehaviour;
    //<summary>引数のentityが衝突するか</summary>
    public virtual MapPhysics.CollisionType canCollide(MapEntity aEntity) {
        return MapPhysics.CollisionType.pass;
    }
    /// <summary>triggerに侵入した</summary>
    public virtual void enter(MapCharacter aCharacter, MapEventSystem aEventSystem) { }
    /// <summary>trigger内で移動せずに居座った</summary>
    public virtual void stay(MapCharacter aCharacter, MapEventSystem aEventSystem) { }
    /// <summary>trigger内で移動した</summary>
    public virtual void moved(MapCharacter aCharacter, MapEventSystem aEventSystem) { }
    /// <summary>triggerから出て行った</summary>
    public virtual void exit(MapCharacter aCharacter, MapEventSystem aEventSystem) { }
    /// <summary>マップ生成時から内部にいた</summary>
    public virtual void existInner(MapCharacter aCharacter) { }
}
