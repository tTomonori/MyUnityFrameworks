using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateTrigger : MapTrigger {
    public override MapWalker.PassType confirmPassType(MapBehaviour aBehaviour){
        if(aBehaviour is MapCharacter){
            if (aBehaviour.GetComponent<MapAttribute>().mAttribute == MapAttribute.Attribute.ghost)
                return MapWalker.PassType.through;
        }
        return MapWalker.PassType.stop;
    }
    private void OnTriggerEnter2D(Collider2D other){
        MapCharacter tCharacter = other.gameObject.GetComponent<MapCharacter>();
        if (tCharacter == null) return;//キャラクター出ないならtriggerしない
        if (!canCollide(other)) return;//階層等を考慮して衝突するか判定
        triggered(tCharacter);
    }
    protected abstract void triggered(MapCharacter aCharacter);
}
