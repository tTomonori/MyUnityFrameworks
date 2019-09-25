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
    private static EntityPhysicsAttribute mAttribute;
    //移動させるキャラに付いているcollider
    private static Collider2D mCollider;
    //移動方向(単位ベクトル)
    private static Vector2 mDirection;
    //当たり判定を貫通しないように移動できる距離
    private static float mDeltaDistance;

    //残りの移動距離
    private static float mRemainingDistance;

    //<summary>フレーム内での移動データ初期化</summary>
    public static void initFrameMovingData(MapCharacter aCharacter) {
        //移動可能距離
        aCharacter.mMovingData.mRemainingDistance = aCharacter.mMovingData.mSpeed * Time.deltaTime;
        if (aCharacter.mMovingData.mMaxMoveDistance < aCharacter.mMovingData.mRemainingDistance)
            aCharacter.mMovingData.mRemainingDistance = aCharacter.mMovingData.mMaxMoveDistance;
    }
    //<summary>キャラを移動させる(同フレームでさらに移動可能ならtrue)</summary>
    public static bool moveCharacter(MapCharacter aCharacter) {
        return false;
    //    if (aCharacter.mMovingData.mDirection == Vector2.zero) {
    //        aCharacter.mMovingData.mRemainingDistance = 0;
    //        return false;
    //    }
    //    //移動処理で使うデータ収集・記録
    //    mCharacter = aCharacter;
    //    mAttribute = aCharacter.mAttribute;
    //    mCollider = mAttribute.mCollider;
    //    mDirection = aCharacter.mMovingData.mDirection.normalized;
    //    mDeltaDistance = aCharacter.mMovingData.mDeltaDistance;
    //    if (kMaxDeltaDistance < mDeltaDistance)
    //        mDeltaDistance = kMaxDeltaDistance;
    //    mRemainingDistance = mCharacter.mMovingData.mRemainingDistance;
    //    //MovingDataの初期化
    //    mCharacter.mMovingData.mDirection = Vector2.zero;
    //    mCharacter.mMovingData.mMaxMoveDistance = float.PositiveInfinity;
    //    mCharacter.mMovingData.mPrePosition = mCharacter.mMapPosition;
    //    mCharacter.mMovingData.mPreStratumLevel = mCharacter.mStratumLevel.mLevel;

    //    //移動させる
    //    MoveResult tResult = moveDelta();
    //    if (tResult is Passed) {

    //    } else if (tResult is Collided) {
    //        mCharacter.mMovingData.mRemainingDistance = 0;
    //        return false;
    //    } else if (tResult is Stopped) {
    //        mCharacter.mMovingData.mRemainingDistance = 0;
    //        return false;
    //    } else if (tResult is Slided) {

    //    }
    //    //残りの移動距離を減算
    //    mCharacter.mMovingData.mRemainingDistance -= mDeltaDistance;
    //    return mCharacter.mMovingData.mRemainingDistance > 0;
    //}
    ////<summary>当たり判定を貫通しない程度に移動</summary>
    //private static MoveResult moveDelta() {
    //    //移動結果(戻り値)
    //    MoveResult tReturn;
    //    //ここで移動させる距離
    //    float tDistance = (mRemainingDistance < mDeltaDistance) ? mRemainingDistance : mDeltaDistance;
    //    //移動ベクトル
    //    Vector2 tVector = mDirection * tDistance;
    //    //直線移動
    //    MoveResult tResult = linearMove(tVector);
    //    if (tResult is Passed) {
    //        //衝突せずに移動完了
    //        tReturn = tResult;
    //    } else if (tResult is Stopped) {
    //        //止められた
    //        tReturn = tResult;
    //    } else if (tResult is Collided) {
    //        //衝突した
    //        tReturn = slideMove(((Collided)tResult).mAttribute, (tDistance - tResult.mDistance) * tVector.normalized);
    //    } else {
    //        Debug.LogWarning("MapCharacterMoveSystem : 不正な移動結果「" + tResult.GetType().ToString() + "」");
    //        tReturn = tResult;
    //    }
    //    return tReturn;
    //}
    ////<summary>スライド移動</summary>
    //private static MoveResult slideMove(MapPhysicsAttribute[] aPhysics, Vector2 aVector) {
    //    foreach (MapPhysicsAttribute tAttribute in aPhysics) {
    //        //スライド移動する方向
    //        Vector2 tSlideVector = getSlideDirection(tAttribute, aVector);
    //        //スライド移動
    //        MoveResult tSlideResult = linearMove(tSlideVector);
    //        if (tSlideResult is Passed)//衝突せずに移動完了
    //            return new Slided(tAttribute, tSlideResult.mDistance);
    //        if (tSlideResult is Stopped)//止められた
    //            return tSlideResult;
    //        //衝突した
    //        //全く移動していない場合は他のcolliderに沿って移動できるか試す
    //        if (tSlideResult.mDistance == 0)
    //            continue;
    //        //さらにスライド移動
    //        Collided tCollidedResult = (Collided)tSlideResult;
    //        MoveResult tSlideResult2 = slideMove(tCollidedResult.mAttribute, aVector * (aVector.magnitude - tCollidedResult.mDistance));
    //        //移動距離を加算して返す
    //        tSlideResult2.mDistance += tCollidedResult.mDistance;
    //        return tSlideResult2;
    //    }
    //    return new Collided(aPhysics, 0);
    //}
    ///// <summary>
    ///// 衝突時にどの方向にスライド移動するか求める
    ///// </summary>
    ///// <returns>スライドする方向</returns>
    ///// <param name="aAttribute">衝突したbehaviour</param>
    ///// <param name="aVector">どの方向から衝突したか</param>
    //private static Vector2 getSlideDirection(MapPhysicsAttribute aAttribute, Vector2 aVector) {
    //    Collider2D tCollider = aAttribute.mCollider;
    //    //衝突相手のcolliderの形状で分岐
    //    if (tCollider is BoxCollider2D) {
    //        //四角形の場合
    //        ColliderDistance2D tSquareDistance = tCollider.Distance(mCollider);
    //        //候補のスライド方向ベクトル
    //        Vector2[] tCandidateVectors = new Vector2[2] { Quaternion.Euler(0,0,tCollider.transform.rotation.z)*new Vector2(1,0)
    //        , Quaternion.Euler(0,0,tCollider.transform.rotation.z)*new Vector2(0, 1) };
    //        //距離ベクトルと垂直な候補ベクトルがあるか
    //        foreach (Vector2 tCandidateVector in tCandidateVectors) {
    //            if (!tCandidateVector.isOrthogonalConsiderdError(tSquareDistance.normal)) continue;
    //            //垂直なベクトルが見つかった
    //            return VectorCalculator.disassembleParallel(aVector, tCandidateVector);
    //        }
    //        //垂直なベクトルがなかった
    //        //候補ベクトルに平行な成分
    //        Vector2 tComponentVector1 = aVector.disassembleParallel(tCandidateVectors[0]);
    //        Vector2 tComponentVector2 = aVector.disassembleParallel(tCandidateVectors[1]);
    //        //大きい方の成分ベクトルを返す
    //        return (tComponentVector1.magnitude < tComponentVector2.magnitude) ? tComponentVector2 : tComponentVector1;
    //    } else if (tCollider is EdgeCollider2D) {
    //        //直線で囲った形の場合
    //        //衝突したcolliderとの距離ベクトル
    //        ColliderDistance2D tEdgeDistance = tCollider.Distance(mCollider);
    //        if (tEdgeDistance.normal.corner(aVector) < 90) {
    //            //距離ベクトルと移動方向ベクトルのなす角が90度未満なら、距離ベクトルに垂直な方向にスライド
    //            return aVector.disassembleOrthogonal(tEdgeDistance.normal);
    //        }

    //        EdgeCollider2D tEdgeCollider = (EdgeCollider2D)tCollider;
    //        //衝突中の距離を求める
    //        Vector2 tPrePosition = mCharacter.mMapPosition;
    //        mCharacter.mMapPosition = mCharacter.mMapPosition + kMaxSeparation * aVector.normalized;
    //        ColliderDistance2D tCollisionDistance = tCollider.Distance(mCollider);
    //        mCharacter.mMapPosition = tPrePosition;
    //        //辺の数(=頂点の数)
    //        int tEdgeNum = tEdgeCollider.pointCount;
    //        //候補のスライド方向ベクトル(=辺ベクトル)
    //        Vector2[] tEdgeVectors = new Vector2[tEdgeNum];
    //        for (int i = 0; i < tEdgeNum; ++i) {
    //            //i番目の頂点
    //            Vector2 tPointC = tEdgeCollider.points[i];
    //            //i+1番目の頂点
    //            Vector2 tPointN = tEdgeCollider.points[(i + 1) % tEdgeNum];
    //            //i番目の頂点からi+1番目の頂点へのベクトル
    //            Vector2 tPointCToPointN = tPointN - tPointC;
    //            if (tPointC == tCollisionDistance.pointA) {
    //                //i番目の頂点と衝突した
    //                //i-1番目の頂点
    //                Vector2 tPointP = tEdgeCollider.points[(i + tEdgeNum - 1) % tEdgeNum];
    //                //i番目の頂点からi-1番目の頂点へのベクトル
    //                Vector2 tPointCToPointP = tPointP - tPointC;
    //                //i番目の頂点からi+1番目の頂点方向成分
    //                Vector2 tComponentCToN = aVector.disassembleParallel(tPointCToPointN);
    //                //i番目の頂点からi-1番目の頂点方向成分
    //                Vector2 tComponentCToP = aVector.disassembleParallel(tPointCToPointP);
    //                //大きい方の成分ベクトルを返す
    //                return (tComponentCToN.magnitude < tComponentCToP.magnitude) ? tComponentCToP : tComponentCToN;
    //            }
    //            //衝突点のローカル座標
    //            Vector2 tCollisionPointLocal = tEdgeCollider.transform.position;
    //            tCollisionPointLocal = tCollisionDistance.pointA - tCollisionPointLocal;
    //            //i番目の頂点から衝突点へのベクトル
    //            Vector2 tPointCToCollision = tCollisionPointLocal - tPointC;
    //            if (!tPointCToPointN.isParallelConsiderdError(tPointCToCollision)) continue;
    //            if (!(tPointCToCollision.magnitude < tPointCToPointN.magnitude)) continue;
    //            //辺と衝突した
    //            return aVector.disassembleParallel(tPointCToPointN);
    //        }
    //        Debug.LogWarning("MapCharacterMoveSystem : EdgeCollider2Dに衝突したらしいけど、どこに衝突したのかわかりません");
    //    }
    //    //円形を含むその他の形状の場合
    //    //衝突したcolliderとの距離ベクトル
    //    ColliderDistance2D tOtherDistance = tCollider.Distance(mCollider);
    //    //スライド移動する方向
    //    Vector2 tOtherVector = aVector.disassembleOrthogonal(tOtherDistance.normal);
    //    return tOtherVector;
    //}
    ////<summary>直線移動</summary>
    //private static MoveResult linearMove(Vector2 aVector) {
    //    //移動先の衝突判定結果
    //    MapPhysics.CollisionType tCollisionType;
    //    MapPhysicsAttribute[] tCollidedAttribute;
    //    //１回目の移動衝突判定
    //    tCollisionType = canMove(mCharacter.mMapPosition + aVector, out tCollidedAttribute);
    //    if (tCollisionType == MapPhysics.CollisionType.pass) {
    //        //移動させる
    //        mCharacter.mMapPosition = mCharacter.mMapPosition + aVector;
    //        return new Passed(aVector.magnitude);
    //    }

    //    //移動可能か調べる移動距離
    //    float tDelta = aVector.magnitude;
    //    //移動可能か調べている距離と、次に調べる距離、の差
    //    float tHalfDelta = tDelta / 2;
    //    //暫定の移動先候補
    //    MapPhysics.CollisionType tProvisionalCollisionType = MapPhysics.CollisionType.collide;
    //    float tProvisionalDistance = 0;
    //    MapPhysicsAttribute[] tProvisionalContactAttribute = tCollidedAttribute;

    //    while (true) {
    //        switch (tCollisionType) {
    //            case MapPhysics.CollisionType.pass:
    //                if (tProvisionalCollisionType == MapPhysics.CollisionType.collide) {
    //                    tProvisionalDistance = tDelta;
    //                }
    //                tDelta += tHalfDelta;
    //                break;
    //            case MapPhysics.CollisionType.stop:
    //                tProvisionalCollisionType = MapPhysics.CollisionType.stop;
    //                tProvisionalDistance = tDelta;
    //                tProvisionalContactAttribute = tCollidedAttribute;
    //                tDelta -= tHalfDelta;
    //                break;
    //            case MapPhysics.CollisionType.collide:
    //                if (tDelta < tProvisionalDistance) {
    //                    tProvisionalCollisionType = MapPhysics.CollisionType.collide;
    //                    tProvisionalDistance = 0;
    //                    tProvisionalContactAttribute = tCollidedAttribute;
    //                }
    //                tDelta -= tHalfDelta;
    //                break;
    //        }
    //        if (tHalfDelta < kMaxSeparation) {
    //            //これ以上細かく衝突判定しない
    //            break;
    //        }
    //        tHalfDelta /= 2;
    //        //移動先の衝突判定
    //        tCollisionType = canMove(mCharacter.mMapPosition + aVector * tDelta, out tCollidedAttribute);
    //    }
    //    //移動させる
    //    mCharacter.mMapPosition = mCharacter.mMapPosition + aVector * tProvisionalDistance;

    //    switch (tProvisionalCollisionType) {
    //        case MapPhysics.CollisionType.stop:
    //            return new Stopped(tProvisionalContactAttribute, tProvisionalDistance);
    //        case MapPhysics.CollisionType.collide:
    //            return new Collided(tProvisionalContactAttribute, tProvisionalDistance);
    //    }
    //    throw new Exception("MapCharacterMoveSystem : 直線移動衝突判定で衝突したにも関わらず衝突してない判定になってる");
    //}
    ///// <summary>
    ///// 指定座標に移動可能か
    ///// </summary>
    ///// <returns>移動可能かどうか</returns>
    ///// <param name="aPosition">移動可能か調べる座標</param>
    ///// <param name="oCollided">移動先で衝突した属性</param>
    //private static MapPhysics.CollisionType canMove(Vector2 aPosition, out MapPhysicsAttribute[] oCollided) {
    //    Collider2D[] tCollided = getCollided(aPosition);
    //    //暫定の衝突結果
    //    MapPhysics.CollisionType tProvisionalCollisionType = MapPhysics.CollisionType.pass;
    //    //衝突した属性のリスト
    //    MapPhysicsAttribute[] tCollidedAttributeList = new MapPhysicsAttribute[10];
    //    //衝突した属性のリストの要素数
    //    int tCollidedAttributeNum = 0;
    //    foreach (Collider2D tCollider in tCollided) {
    //        MapPhysicsAttribute tAttribute = tCollider.GetComponent<MapPhysicsAttribute>();
    //        //属性なしなら衝突しない
    //        if (tAttribute == null) continue;
    //        //階層の違いで衝突しない
    //        if (mCharacter.mStratumLevel.collide(tAttribute.getStratumLevel()) == MapStratumLevel.CollisionType.through) continue;
    //        //衝突判定
    //        MapPhysics.CollisionType tCollisionType = MapPhysics.canCollide(mAttribute, tAttribute);
    //        switch (tCollisionType) {
    //            case MapPhysics.CollisionType.pass:
    //                continue;
    //            case MapPhysics.CollisionType.collide:
    //                if (tProvisionalCollisionType != MapPhysics.CollisionType.collide) {
    //                    tProvisionalCollisionType = MapPhysics.CollisionType.collide;
    //                    tCollidedAttributeNum = 0;
    //                }
    //                tCollidedAttributeList[tCollidedAttributeNum] = tAttribute;
    //                tCollidedAttributeNum++;
    //                continue;
    //            case MapPhysics.CollisionType.stop:
    //                if (tProvisionalCollisionType == MapPhysics.CollisionType.collide)
    //                    continue;
    //                if (tProvisionalCollisionType == MapPhysics.CollisionType.pass) {
    //                    tProvisionalCollisionType = MapPhysics.CollisionType.stop;
    //                }
    //                tCollidedAttributeList[tCollidedAttributeNum] = tAttribute;
    //                tCollidedAttributeNum++;
    //                continue;
    //            default:
    //                Debug.LogWarning("MapCharacterMoveSystem : 未定義の衝突判定結果「" + tCollisionType.ToString() + "」");
    //                oCollided = new MapPhysicsAttribute[1] { tAttribute };
    //                return MapPhysics.CollisionType.collide;
    //        }
    //    }
    //    Array.Resize<MapPhysicsAttribute>(ref tCollidedAttributeList, tCollidedAttributeNum);
    //    oCollided = tCollidedAttributeList;
    //    return tProvisionalCollisionType;
    //}
    ////<summary>指定座標に移動した時に衝突するcolliderを取得</summary>
    //private static Collider2D[] getCollided(Vector2 aPosition) {
        //Vector2 tPrePosition = mCharacter.mMapPosition;
        //mCharacter.mMapPosition = aPosition;
        //Collider2D[] tResult = new Collider2D[100];
        //int tColliderNum = mCollider.OverlapCollider(new ContactFilter2D(), tResult);
        //mCharacter.mMapPosition = tPrePosition;
        //Array.Resize<Collider2D>(ref tResult, tColliderNum);
        //return tResult;
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
        public MapPhysicsAttribute[] mAttribute;
        public Collided(MapPhysicsAttribute[] aAttribute, float aDistance) {
            mAttribute = aAttribute;
            mDistance = aDistance;
        }
    }
    //イベントに止められた
    public class Stopped : MoveResult {
        //止めてきたトリガー
        public MapPhysicsAttribute[] mAttribute;
        public Stopped(MapPhysicsAttribute[] aAttribute, float aDistance) {
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
