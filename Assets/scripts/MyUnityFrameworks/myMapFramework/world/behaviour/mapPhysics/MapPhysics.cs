using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapPhysics {
    public enum CollisionType {
        pass, stop, collide
    }
    //衝突するか判定
    public static CollisionType canCollide(MapPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        if (aAttribute1 is EntityPhysicsAttribute)
            return canCollide((EntityPhysicsAttribute)aAttribute1, aAttribute2);

        return CollisionType.pass;
    }
    public static CollisionType canCollide(EntityPhysicsAttribute aAttribute1, MapPhysicsAttribute aAttribute2) {
        //階層判定
        if (aAttribute1.getStratumLevel().collide(aAttribute2.getStratumLevel()) == MapStratumLevel.CollisionType.through)
            return CollisionType.pass;

        //相手の属性で分岐
        if (aAttribute2 is SlopeTilePhysicsAttribute) {
            return collideEntityAndSlope(aAttribute1, (SlopeTilePhysicsAttribute)aAttribute2);
        }
        if (aAttribute2 is ExceptionStratumTilePhysicsAttribute) {
            return collideEntityAndESTile(aAttribute1, (ExceptionStratumTilePhysicsAttribute)aAttribute2);
        }
        if (aAttribute2 is TilePhysicsAttribute) {
            return collideEntityAndTile(aAttribute1, (TilePhysicsAttribute)aAttribute2);
        }
        if (aAttribute2 is EntityPhysicsAttribute) {
            return collideEntityAndEntity(aAttribute1, (EntityPhysicsAttribute)aAttribute2);
        }
        if (aAttribute2 is TriggerPhysicsAttribute) {
            return collideEntityAndTrigger(aAttribute1, (TriggerPhysicsAttribute)aAttribute2);
        }
        throw new System.Exception("MapPhysics : 未定義の属性との衝突判定「" + aAttribute2.GetType().ToString() + "」");
    }
    public static CollisionType collideEntityAndTile(EntityPhysicsAttribute aEntity, TilePhysicsAttribute aTile) {
        return aEntity.canCollide(aTile);
    }
    public static CollisionType collideEntityAndSlope(EntityPhysicsAttribute aEntity, SlopeTilePhysicsAttribute aSlope) {
        //坂にいるなら通過可能
        if (aEntity.getStratumLevel().mLevel % 2 == 1) return CollisionType.pass;
        //侵入できるか調べる
        if (!aSlope.canInvade(aEntity)) return CollisionType.collide;
        return aEntity.canCollide(aSlope);
    }
    public static CollisionType collideEntityAndESTile(EntityPhysicsAttribute aEntity, ExceptionStratumTilePhysicsAttribute aESTile) {
        if (!aESTile.canCollide(aEntity)) return CollisionType.pass;
        return aEntity.canCollide(aESTile);
    }
    public static CollisionType collideEntityAndEntity(EntityPhysicsAttribute aEntity, EntityPhysicsAttribute aOponent) {
        return aEntity.canCollide(aOponent);
    }
    public static CollisionType collideEntityAndTrigger(EntityPhysicsAttribute aEntity, TriggerPhysicsAttribute aTrigger) {
        return aTrigger.canCollide(aEntity);
    }

    //衝突しているか判定
    public static bool isCollided(EntityPhysicsAttribute aAttribute1,MapPhysicsAttribute aAttribute2) {
        if (aAttribute1.getStratumLevel().collide(aAttribute2.getStratumLevel()) == MapStratumLevel.CollisionType.through)
            return false;

        if (aAttribute2 is ExceptionStratumTilePhysicsAttribute)
            return ((ExceptionStratumTilePhysicsAttribute)aAttribute2).canCollide(aAttribute1);
        else
            return true;
    }
}
