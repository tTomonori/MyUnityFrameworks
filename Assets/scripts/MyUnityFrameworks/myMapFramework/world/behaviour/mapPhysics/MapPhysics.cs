using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapPhysics {
    public enum CollisionType {
        pass, stop, collide
    }

    /// <summary>
    /// 第一引数の属性が第二引数の属性と衝突するか
    /// </summary>
    /// <returns>衝突結果</returns>
    /// <param name="aAttribute1">属性1</param>
    /// <param name="aAttribute2">属性2</param>
    public static CollisionType canCollide(MapPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        if (aAttribute1 is EntityPhysicsAttribute)
            return canCollide((EntityPhysicsAttribute)aAttribute1, aAttribute2);

        return CollisionType.pass;
    }
    public static CollisionType canCollide(EntityPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        //相手の属性で分岐
        if (aAttribute2 is TileGroundPhysicsAttribute) {
            return aAttribute1.canEnter((TileGroundPhysicsAttribute)aAttribute2) ? CollisionType.pass : CollisionType.collide;
        }
        if (aAttribute2 is SlopeTilePhysicsAttribute) {
            return ((SlopeTilePhysicsAttribute)aAttribute2).canBeEntered(aAttribute1.mBehaviour.mMapPosition) ? CollisionType.pass : CollisionType.collide;
        }
        if (aAttribute2 is EntityPhysicsAttribute) {
            return aAttribute1.canEnter((EntityPhysicsAttribute)aAttribute2) ? CollisionType.pass : CollisionType.collide;
        }
        if (aAttribute2 is TriggerPhysicsAttribute) {
            return ((TriggerPhysicsAttribute)aAttribute2).canBeEntered(aAttribute1);
        }
        throw new System.Exception("MapPhysics : entityと未定義の属性との衝突判定「" + aAttribute2.GetType().ToString() + "」");
    }
}
