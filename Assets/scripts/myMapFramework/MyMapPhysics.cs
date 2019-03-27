using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMapPhysics {
    static public List<Collider2D> overlapAll(MapBehaviour aBehaviour,MapStratum.ContactFilter aFilter,Vector2 aPosition){
        Vector2 tTempPosition = aBehaviour.position2D;
        //一時的に移動
        aBehaviour.position2D = aPosition;
        //衝突するコライダー取得
        List<Collider2D> tRes = overlapAll(aBehaviour, aFilter);
        //元の位置に戻す
        aBehaviour.position2D = tTempPosition;
        return tRes;
    }
    static public List<Collider2D> overlapAll(MapBehaviour aBehaviour,MapStratum.ContactFilter aFilter){
        //衝突するコライダーを取得
        Collider2D[] tCollisers = new Collider2D[20];
        Physics2D.OverlapCollider(aBehaviour.mCollider, new ContactFilter2D(), tCollisers);
        //衝突したもののみを返す
        MapStratum tMyStratum = aBehaviour.mStratum;
        List<Collider2D> tRes = new List<Collider2D>();
        foreach(Collider2D tCollider in tCollisers){
            if (tCollider == null) break;
            if (tCollider.GetComponent<MapAttribute>() == null) continue;
            MapStratum tStratum = tCollider.GetComponentInParent<MapStratum>();
            if (tStratum == null) continue;
            if (!tMyStratum.canCollide(tStratum, aFilter)) continue;
            tRes.Add(tCollider);
        }
        return tRes;
    }
}