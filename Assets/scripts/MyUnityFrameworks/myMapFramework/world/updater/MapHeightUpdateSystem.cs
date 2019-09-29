using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapHeightUpdateSystem {
    /// <summary>キャラの高さを更新する</summary>
    public static void updateHeight(MapCharacter aCharacter) {
        SlopeTilePhysicsAttribute tSlope = getCollidedSlopTile(aCharacter);
        if (tSlope != null) {
            bool oIsIn;
            float tHeight = tSlope.getPointHeight(aCharacter.worldPosition2D, out oIsIn);
            aCharacter.mMovingData.mCollidedSlope = tSlope;
            aCharacter.mHeight = tHeight;
            return;
        }
        //衝突している坂がない場合
        if (aCharacter.mMovingData.mCollidedSlope == null) {
            return;
        }
        bool oIsIn2;
        aCharacter.mHeight = aCharacter.mMovingData.mCollidedSlope.getPointHeight(aCharacter.worldPosition2D, out oIsIn2);
        aCharacter.mMovingData.mCollidedSlope = null;
    }
    /// <summary>キャラが配置された直後に高さ情報を初期化</summary>
    public static void initHeight(MapCharacter aCharacter) {
        aCharacter.mMovingData.mCollidedSlope = getCollidedSlopTile(aCharacter);
    }
    /// <summary>behaviourが衝突している坂tileを取得(衝突しているtileは1つのみであること前提)</summary>
    public static SlopeTilePhysicsAttribute getCollidedSlopTile(MapBehaviour aBehaviour) {
        foreach (Collider2D tCollider in Physics2D.OverlapPointAll(aBehaviour.worldPosition2D)) {
            SlopeTilePhysicsAttribute tSlope = tCollider.GetComponent<SlopeTilePhysicsAttribute>();
            if (tSlope == null) continue;
            if (MapPhysics.collide(aBehaviour.mHeight, tSlope.getHeight())) {
                return tSlope;
            }
        }
        return null;
    }

}
