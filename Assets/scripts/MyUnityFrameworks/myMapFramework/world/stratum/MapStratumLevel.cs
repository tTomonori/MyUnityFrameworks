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
        int tDifference = aStratumLevel.mLevel - mLevel;
        if (mLevel % 2 == 0) {
            //平地にいる
            if (-1 <= tDifference && tDifference <= 1)
                return CollisionType.collision;
            return CollisionType.through;
        } else {
            //坂道にいる
            if (-2 <= tDifference && tDifference <= 2)
                return CollisionType.collision;
            return CollisionType.through;
        }
    }

    public enum CollisionType {
        //<summary>衝突しない</summary>
        through,
        //<summary>衝突する</summary>
        collision
    }
}
