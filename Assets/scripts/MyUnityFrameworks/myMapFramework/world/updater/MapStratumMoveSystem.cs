using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapStratumMoveSystem {
    //<summary>階層更新</summary>
    public static void updateStratumLevel(MapCharacter aCharacter, MapWorld aWorld) {
        EntityPhysicsAttribute tAttribute = aCharacter.mAttribute;
        //衝突している階層移動属性を探す
        SlopeTilePhysicsAttribute[] tCollidesSlopes = getCollided(aCharacter);
        if (tCollidesSlopes.Length == 0) {
            //一つも衝突していない
            if (aCharacter.mMovingData.mCollidedSlope.Length == 0) return;//元から衝突していなかったならそのまま
            //slopeの外へ移動していた
            for (int i = 0; ; i++) {
                //ここで配列の長さを超えたら → throw new Exception("MapStratumMoveSysptem : 不正なSlope上での移動");
                SlopeTilePhysicsAttribute tSlope = aCharacter.mMovingData.mCollidedSlope[i];
                ColliderDistance2D d = tSlope.mCollider.Distance(tAttribute.mCollider);
                SlopeTilePhysicsAttribute.Side tSide = tSlope.getRelativeSide(tAttribute);
                if (tSide == SlopeTilePhysicsAttribute.Side.highSide) {
                    applayStratumLevel(aCharacter, aWorld, tSlope.getStratumLevel().mLevel + 1);
                    break;
                } else if (tSide == SlopeTilePhysicsAttribute.Side.lowSide) {
                    applayStratumLevel(aCharacter, aWorld, tSlope.getStratumLevel().mLevel - 1);
                    break;
                }
            }
            aCharacter.mMovingData.mCollidedSlope = new SlopeTilePhysicsAttribute[0];
            return;
        } else {
            //衝突している階層移動属性がある
            aCharacter.mMovingData.mCollidedSlope = tCollidesSlopes;
            int tHeightestStratumLevel = 0;
            foreach (SlopeTilePhysicsAttribute tSlope in tCollidesSlopes) {
                int tLevel = tSlope.getStratumLevel().mLevel;
                if (tHeightestStratumLevel < tLevel)
                    tHeightestStratumLevel = tLevel;
            }
            applayStratumLevel(aCharacter, aWorld, tHeightestStratumLevel);
        }
    }
    //階層の適用
    private static void applayStratumLevel(MapCharacter aCharacter, MapWorld aWorld, int aStratumLevel) {
        aWorld.moveStratumLevel(aCharacter, aStratumLevel);
        aCharacter.mStratumLevel.mLevel = aStratumLevel;
    }
    //移動用階層データ初期化
    public static void initSlopData(MapCharacter aCharacter) {
        aCharacter.mMovingData.mCollidedSlope = getCollided(aCharacter);
    }
    //<summary>衝突している階層移動属性を取得</summary>
    private static SlopeTilePhysicsAttribute[] getCollided(MapEntity aEntity) {
        EntityPhysicsAttribute tAttribute = aEntity.mAttribute;
        //衝突しているcolliderを取得
        Collider2D[] tColliders = new Collider2D[100];
        int tColliderNum = tAttribute.mCollider.OverlapCollider(new ContactFilter2D(), tColliders);
        //slopeのみを抽出
        SlopeTilePhysicsAttribute[] tSlopes = new SlopeTilePhysicsAttribute[100];
        int tSlopeNum = 0;
        foreach (Collider2D tCollider in tColliders) {
            if (tCollider == null) break;
            SlopeTilePhysicsAttribute tSlope = tCollider.GetComponent<SlopeTilePhysicsAttribute>();
            if (tSlope == null) continue;
            //衝突しているか判定
            if (!MapPhysics.isCollided(tAttribute, tSlope)) continue;
            tSlopes[tSlopeNum] = tSlope;
            ++tSlopeNum;
        }
        Array.Resize<SlopeTilePhysicsAttribute>(ref tSlopes, tSlopeNum);
        return tSlopes;
    }
}
