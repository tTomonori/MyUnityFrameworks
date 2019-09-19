using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapCharacterMoveSystem {
    //障害物と衝突した時、障害物との距離の最大許容距離
    static private float kMaxSeparation = 0.02f;
    //当たり判定を貫通しない程度に移動して良い最大距離
    static private float kMaxDeltaDistance = 1;
    //移動させるキャラ
    private static MapCharacter mCharacter;
    //移動させるキャラの属性
    private static MapPhysicsAttribute mAttribute;
    //移動させるキャラに付いているcollider
    private static Collider2D mCollider;
    //移動方向(単位ベクトル)
    private static Vector2 mDirection;
    //移動距離
    private static float mDistance;
    //当たり判定を貫通しないように移動できる距離
    private static float mDeltaDistance;

    //残りの移動距離
    private static float mRemainingDistance;

    //<summary>キャラを移動させる</summary>
    public static void moveCharacter(MapCharacter aCharacter) {
        if (aCharacter.mMovingData.mDirection == Vector2.zero) return;
        //移動処理で使うデータ収集・記録
        mCharacter = aCharacter;
        mAttribute = aCharacter.GetComponent<MapPhysicsAttribute>();
        mCollider = mAttribute.mCollider;
        mDirection = aCharacter.mMovingData.mDirection.normalized;
        mDistance = aCharacter.mMovingData.mSpeed * Time.deltaTime;
        if (aCharacter.mMovingData.mMaxMoveDistance < mDistance)
            mDistance = aCharacter.mMovingData.mMaxMoveDistance;
        mDeltaDistance = aCharacter.mMovingData.mDeltaDistance;
        if (kMaxDeltaDistance < mDeltaDistance)
            mDeltaDistance = kMaxDeltaDistance;
        mRemainingDistance = mDistance;
        //MovingDataの初期化
        mCharacter.mMovingData.mDirection = Vector2.zero;
        mCharacter.mMovingData.mMaxMoveDistance = float.PositiveInfinity;
        mCharacter.mMovingData.mPrePosition = mCharacter.mMapPosition;
        mCharacter.mMovingData.mPreStratumLevel = mCharacter.mStratumLevel.mLevel;

        //移動させる
        while (true) {
            MoveResult tResult = moveDelta();
            if (tResult is Passed) {

            } else if (tResult is Collided) {
                break;
            } else if (tResult is Stopped) {
                break;
            } else if (tResult is Slided) {

            }
            //残りの移動距離を減算
            mRemainingDistance -= mDeltaDistance;
            //最大距離移動
            if (mRemainingDistance <= 0)
                break;
        }
        //PositionZを更新
        mCharacter.positionZ = MapZOrderCalculator.calculateOrderOfEntity(mCharacter.mMapPosition.x, mCharacter.mMapPosition.y, mCharacter.mStratumLevel.mLevel);
    }
    //<summary>当たり判定を貫通しない程度に移動(これ以上移動できない場合はfalse)</summary>
    private static MoveResult moveDelta() {
        //移動結果(戻り値)
        MoveResult tReturn;
        //ここで移動させる距離
        float tDistance = (mRemainingDistance < mDeltaDistance) ? mRemainingDistance : mDeltaDistance;
        //移動ベクトル
        Vector2 tVector = mDirection * tDistance;
        //直線移動
        MoveResult tResult = linearMove(tVector);
        if (tResult is Passed) {
            //衝突せずに移動完了
            tReturn = tResult;
        } else if (tResult is Stopped) {
            //止められた
            tReturn = tResult;
        } else if (tResult is Collided) {
            //衝突した
            tReturn = slideMove(((Collided)tResult).mAttribute, (tDistance - tResult.mDistance)*tVector.normalized);
        } else {
            Debug.LogWarning("MapCharacterMoveSystem : 不正な移動結果「" + tResult.GetType().ToString() + "」");
            tReturn = tResult;
        }
        //階層更新

        return tReturn;
    }
    //<summary>スライド移動</summary>
    private static MoveResult slideMove(MapPhysicsAttribute aPhysics, Vector2 aVector) {
        //衝突したcolliderとの距離ベクトル
        ColliderDistance2D tColliderDistance = aPhysics.mCollider.Distance(mCollider);
        //スライド移動する方向
        Vector2 tSlideVector = VectorCalculator.disassemble(aVector, tColliderDistance.normal);
        //スライド移動
        MoveResult tSlideResult = linearMove(tSlideVector);
        if (tSlideResult is Passed)//衝突せずに移動完了
            return tSlideResult;
        if (tSlideResult is Stopped)//止められた
            return tSlideResult;

        //衝突した
        //ほとんど移動できていない場合はこれ以上スライド移動しない
        if (tSlideResult.mDistance < kMaxSeparation)
            return tSlideResult;
        //さらにスライド移動
        Collided tCollidedResult = (Collided)tSlideResult;
        MoveResult tSlideResult2 = slideMove(tCollidedResult.mAttribute, aVector * (aVector.magnitude - tCollidedResult.mDistance));
        if (tSlideResult2 is Passed) {//衝突せずに移動完了
            return new Slided(tCollidedResult.mAttribute, tCollidedResult.mDistance + tSlideResult2.mDistance);
        }
        //衝突せずに移動完了以外は移動距離を加算して返す
        tSlideResult2.mDistance += tCollidedResult.mDistance;
        return tSlideResult2;
    }
    //<summary>直線移動</summary>
    private static MoveResult linearMove(Vector2 aVector) {
        //移動先の衝突判定結果
        MapPhysicsAttribute.CollisionType tCollisionType;
        MapPhysicsAttribute tCollidedAttribute;
        //１回目の移動衝突判定
        tCollisionType = canMove(mCharacter.mMapPosition + aVector, out tCollidedAttribute);
        if (tCollisionType == MapPhysicsAttribute.CollisionType.pass) {
            //移動させる
            mCharacter.mMapPosition = mCharacter.mMapPosition + aVector;
            return new Passed(aVector.magnitude);
        }

        //移動可能か調べる移動距離
        float tDelta = aVector.magnitude;
        //移動可能か調べている距離と、次に調べる距離、の差
        float tHalfDelta = tDelta / 2;
        //暫定の移動先候補
        MapPhysicsAttribute.CollisionType tProvisionalCollisionType = MapPhysicsAttribute.CollisionType.collide;
        float tProvisionalDistance = 0;
        MapPhysicsAttribute tProvisionalContactAttribute = tCollidedAttribute;

        while (true) {
            switch (tCollisionType) {
                case MapPhysicsAttribute.CollisionType.pass:
                    if (tProvisionalCollisionType == MapPhysicsAttribute.CollisionType.collide) {
                        tProvisionalDistance = tDelta;
                    }
                    tDelta += tHalfDelta;
                    break;
                case MapPhysicsAttribute.CollisionType.stop:
                    tProvisionalCollisionType = MapPhysicsAttribute.CollisionType.stop;
                    tProvisionalDistance = tDelta;
                    tProvisionalContactAttribute = tCollidedAttribute;
                    tDelta -= tHalfDelta;
                    break;
                case MapPhysicsAttribute.CollisionType.collide:
                    if (tDelta < tProvisionalDistance) {
                        tProvisionalCollisionType = MapPhysicsAttribute.CollisionType.collide;
                        tProvisionalDistance = 0;
                        tProvisionalContactAttribute = tCollidedAttribute;
                    }
                    tDelta -= tHalfDelta;
                    break;
            }
            if (tHalfDelta < kMaxSeparation) {
                //これ以上細かく衝突判定しない
                break;
            }
            tHalfDelta /= 2;
            //移動先の衝突判定
            tCollisionType = canMove(mCharacter.mMapPosition + aVector * tDelta, out tCollidedAttribute);
        }
        //移動させる
        mCharacter.mMapPosition = mCharacter.mMapPosition + aVector * tProvisionalDistance;

        switch (tProvisionalCollisionType) {
            case MapPhysicsAttribute.CollisionType.stop:
                return new Stopped(tProvisionalContactAttribute, tProvisionalDistance);
            case MapPhysicsAttribute.CollisionType.collide:
                return new Collided(tProvisionalContactAttribute, tProvisionalDistance);
        }
        throw new Exception("MapCharacterMoveSystem : 直線移動衝突判定で衝突したにも関わらず衝突してない判定になってる");
    }
    /// <summary>
    /// 指定座標に移動可能か
    /// </summary>
    /// <returns>移動可能かどうか</returns>
    /// <param name="aPosition">移動可能か調べる座標</param>
    /// <param name="oCollided">移動先で衝突した属性</param>
    private static MapPhysicsAttribute.CollisionType canMove(Vector2 aPosition, out MapPhysicsAttribute oCollided) {
        Collider2D[] tCollided = getCollided(aPosition);
        foreach (Collider2D tCollider in tCollided) {
            MapPhysicsAttribute tAttribute = tCollider.GetComponent<MapPhysicsAttribute>();
            //属性なしなら衝突しない
            if (tAttribute == null) continue;
            //階層の違いで衝突しない
            if (mCharacter.mStratumLevel.collide(tAttribute.getStratumLevel()) == MapStratumLevel.CollisionType.through) continue;
            //衝突判定
            MapPhysicsAttribute.CollisionType tCollisionType = mAttribute.canCollide(tAttribute);
            switch (tCollisionType) {
                case MapPhysicsAttribute.CollisionType.pass:
                    continue;
                case MapPhysicsAttribute.CollisionType.collide:
                    oCollided = tAttribute;
                    return MapPhysicsAttribute.CollisionType.collide;
                case MapPhysicsAttribute.CollisionType.stop:
                    oCollided = tAttribute;
                    return MapPhysicsAttribute.CollisionType.stop;
                default:
                    Debug.LogWarning("MapCharacterMoveSystem : 未定義の衝突判定結果「" + tCollisionType.ToString() + "」");
                    oCollided = tAttribute;
                    return MapPhysicsAttribute.CollisionType.collide;
            }
        }
        oCollided = null;
        return MapPhysicsAttribute.CollisionType.pass;
    }
    //<summary>指定座標に移動した時に衝突するcolliderを取得</summary>
    private static Collider2D[] getCollided(Vector2 aPosition) {
        Vector2 tPrePosition = mCharacter.mMapPosition;
        mCharacter.mMapPosition = aPosition;
        Collider2D[] tResult = new Collider2D[100];
        int tColliderNum = mCollider.OverlapCollider(new ContactFilter2D(), tResult);
        mCharacter.mMapPosition = tPrePosition;
        Array.Resize<Collider2D>(ref tResult, tColliderNum);
        return tResult;
    }

    //移動結果
    public class MoveResult {
        //<summary>移動した距離</summary>
        public float mDistance;
    }
    //衝突することなく移動完了
    public class Passed : MoveResult {
        public Passed(float aDistance) {
            mDistance = aDistance;
        }
    }
    //衝突して止まった
    public class Collided : MoveResult {
        //衝突した物
        public MapPhysicsAttribute mAttribute;
        public Collided(MapPhysicsAttribute aAttribute, float aDistance) {
            mAttribute = aAttribute;
            mDistance = aDistance;
        }
    }
    //イベントに止められた
    public class Stopped : MoveResult {
        //止めてきたトリガー
        public MapPhysicsAttribute mAttribute;
        public Stopped(MapPhysicsAttribute aAttribute, float aDistance) {
            mAttribute = aAttribute;
            mDistance = aDistance;
        }
    }
    //衝突した物に沿うように移動した
    public class Slided : MoveResult {
        //衝突した物
        public MapPhysicsAttribute mAttribute;
        public Slided(MapPhysicsAttribute aAttribute, float aDistance) {
            mAttribute = aAttribute;
            mDistance = aDistance;
        }
    }

    //<summary>(誤差を考慮した上で)指定座標に居るならtrue</summary>
    public static bool arrived(MapCharacter aCharacter, Vector2 aPosition) {
        return Vector2.Distance(aCharacter.mMapPosition, aPosition) <= kMaxSeparation;
    }
}
