using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStratumLevel {
    //<summary>階層レベル</summary>
    public int mLevel;
    public MapStratumLevel(int aLevel) {
        mLevel = aLevel;
    }

    //<summary>このオブジェクトが引数のオブジェクトに衝突するか</summary>
    public CollisionType collide(MapStratumLevel aStratumLevel) {
        if (mLevel % 2 == 0) {
            //平地にいる
            if (mLevel == aStratumLevel.mLevel || mLevel + 1 == aStratumLevel.mLevel)
                return CollisionType.collision;
            if (mLevel - 1 == aStratumLevel.mLevel)
                return CollisionType.collisionStepOnly;
            return CollisionType.through;
        } else {
            //坂道にいる
            int tDifference = aStratumLevel.mLevel - mLevel;
            if (-2 <= tDifference && tDifference <= 2)
                return CollisionType.collision;
            return CollisionType.collisionStepOnly;
        }
    }

    public enum CollisionType {
        //<summary>衝突しない</summary>
        through,
        //<summary>衝突する</summary>
        collision,
        //<summary>階層移動オブジェクトとのみ衝突する</summary>
        collisionStepOnly
    }
}
