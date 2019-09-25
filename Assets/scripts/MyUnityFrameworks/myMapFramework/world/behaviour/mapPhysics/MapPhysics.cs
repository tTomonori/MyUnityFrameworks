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
        return (aHeight1 - 0.9f < aHeight2) && (aHeight2 < aHeight1 + 0.9f);
    }
    //衝突するか判定
    public static CollisionType canCollide(MapPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        if (aAttribute1 is EntityPhysicsAttribute)
            return canCollide((EntityPhysicsAttribute)aAttribute1, aAttribute2);

        return CollisionType.pass;
    }
    public static CollisionType canCollide(EntityPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        //階層判定
        if (!collide(aAttribute1.getHeight(), aAttribute2.getHeight())) return CollisionType.pass;

        //相手の属性で分岐
        if (aAttribute2 is TileGroundPhysicsAttribute) {
            return aAttribute1.canEnter((TileGroundPhysicsAttribute)aAttribute2) ? CollisionType.pass : CollisionType.collide;
        }
        if (aAttribute2 is SlopeTilePhysicsAttribute) {
            return ((SlopeTilePhysicsAttribute)aAttribute2).canBeEntered(aAttribute1.mBehaviour.mMapPosition, aAttribute1.getHeight()) ? CollisionType.pass : CollisionType.collide;
        }
        if (aAttribute2 is EntityPhysicsAttribute) {
            return aAttribute1.canEnter((EntityPhysicsAttribute)aAttribute2) ? CollisionType.pass : CollisionType.collide;
        }
        if (aAttribute2 is TriggerPhysicsAttribute) {
            return ((TriggerPhysicsAttribute)aAttribute2).canBeEntered(aAttribute1);
        }
        throw new System.Exception("MapPhysics : entityと未定義の属性との衝突判定「" + aAttribute2.GetType().ToString() + "」");
    }
    //public static CollisionType collideEntityAndTile(EntityPhysicsAttribute aEntity, TilePhysicsAttribute aTile) {
    //    return aEntity.canCollide(aTile);
    //}
    //public static CollisionType collideEntityAndSlope(EntityPhysicsAttribute aEntity, SlopeTilePhysicsAttribute aSlope) {
    //    //坂にいるなら通過可能
    //    if (aEntity.getStratumLevel().mLevel % 2 == 1) return CollisionType.pass;
    //    //侵入できるか調べる
    //    if (!aSlope.canInvade(aEntity)) return CollisionType.collide;
    //    return aEntity.canCollide(aSlope);
    //}
    //public static CollisionType collideEntityAndESTile(EntityPhysicsAttribute aEntity, ExceptionStratumTilePhysicsAttribute aESTile) {
    //    if (!aESTile.canCollide(aEntity)) return CollisionType.pass;
    //    return aEntity.canCollide(aESTile);
    //}
    //public static CollisionType collideEntityAndEntity(EntityPhysicsAttribute aEntity, EntityPhysicsAttribute aOponent) {
    //    return aEntity.canCollide(aOponent);
    //}
    //public static CollisionType collideEntityAndTrigger(EntityPhysicsAttribute aEntity, TriggerPhysicsAttribute aTrigger) {
    //    return aTrigger.canCollide(aEntity);
    //}

    ////衝突しているか判定
    //public static bool isCollided(EntityPhysicsAttribute aAttribute1,MapPhysicsAttribute aAttribute2) {
    //    if (aAttribute1.getStratumLevel().collide(aAttribute2.getStratumLevel()) == MapStratumLevel.CollisionType.through)
    //        return false;

    //    if (aAttribute2 is ExceptionStratumTilePhysicsAttribute)
    //        return ((ExceptionStratumTilePhysicsAttribute)aAttribute2).canCollide(aAttribute1);
    //    else
    //        return true;
    //}
}
