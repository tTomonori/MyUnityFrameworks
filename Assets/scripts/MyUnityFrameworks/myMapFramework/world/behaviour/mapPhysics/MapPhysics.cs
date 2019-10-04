using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapPhysics {
    public enum CollisionType {
        pass, stop, collide
    }

    /// <summary>
    /// 指定した高さにある二つの物体が衝突するか
    /// </summary>
    /// <returns>衝突するならtrue</returns>
    /// <param name="aHeight1">衝突する側</param>
    /// <param name="aHeight2">衝突される側</param>
    public static bool collide(float aHeight1,float aHeight2) {
        return Mathf.Abs(aHeight1 - aHeight2) < 1f;
    }
    //衝突するか判定
    public static CollisionType canCollide(MapPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        if (aAttribute1 is EntityPhysicsAttribute)
            return canCollide((EntityPhysicsAttribute)aAttribute1, aAttribute2);

        return CollisionType.pass;
    }
    public static bool canCollide(EntityPhysicsAttribute aEntity,RistrictMovingTile aRistrictTile) {
        return collide(aEntity.mEntity.mHeight, aRistrictTile.mCell.mHeight);
    }
    public static CollisionType canCollide(EntityPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        //階層判定
        if (!collide(aAttribute1.getHeight(), aAttribute2.getHeight())) return CollisionType.pass;

        //相手の属性で分岐
        if (aAttribute2 is TileGroundPhysicsAttribute) {
            return aAttribute1.canEnter((TileGroundPhysicsAttribute)aAttribute2) ? CollisionType.pass : CollisionType.collide;
        }
        if (aAttribute2 is SlopeTilePhysicsAttribute) {
            return ((SlopeTilePhysicsAttribute)aAttribute2).canBeEntered(aAttribute1.mBehaviour.worldPosition2D, aAttribute1.getHeight()) ? CollisionType.pass : CollisionType.collide;
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
