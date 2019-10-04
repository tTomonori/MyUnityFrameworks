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

    ///<summary>移動開始前に移動データ初期化</summary>
    public static void initFrameMovingData(MapCharacter aCharacter) {
        //移動可能距離
        aCharacter.mMovingData.mRemainingDistance = aCharacter.mMovingData.mSpeed * Time.deltaTime;
        if (aCharacter.mMovingData.mMaxMoveDistance < aCharacter.mMovingData.mRemainingDistance)
            aCharacter.mMovingData.mRemainingDistance = aCharacter.mMovingData.mMaxMoveDistance;
        //移動前の座標
        aCharacter.mMovingData.mPrePosition = aCharacter.mMapPosition;
        //移動前の高さ
        aCharacter.mMovingData.mPreHeight = aCharacter.mHeight;
    }
    /// <summary>移動完了後に移動データリセット</summary>
    public static void resetFrameMovingData(MapCharacter aCharacter) {
        aCharacter.mMovingData.mDirection = Vector2.zero;
        aCharacter.mMovingData.mMaxMoveDistance = float.PositiveInfinity;
    }
    ///<summary>キャラを移動させる(同フレームでさらに移動可能ならtrue)</summary>
    public static bool moveCharacter(MapCharacter aCharacter) {
        if (aCharacter.mMovingData.mDirection == Vector2.zero) {
            //移動しない場合
            aCharacter.mMovingData.mRemainingDistance = 0;
            return false;
        }
        //移動処理で使うデータ収集・記録
        mCharacter = aCharacter;
        mAttribute = aCharacter.mAttribute;
        mCollider = mAttribute.mCollider;
        mDirection = aCharacter.mMovingData.mDirection.normalized;
        mDeltaDistance = aCharacter.mMovingData.mDeltaDistance;
        if (kMaxDeltaDistance < mDeltaDistance)
            mDeltaDistance = kMaxDeltaDistance;
        mRemainingDistance = mCharacter.mMovingData.mRemainingDistance;

        //移動させる
        MoveResult tResult = moveDelta();
        if (tResult.mCollisionType == MapPhysics.CollisionType.pass) {

        } else if (tResult.mCollisionType == MapPhysics.CollisionType.collide) {
            mCharacter.mMovingData.mRemainingDistance = 0;
            return false;
        } else if (tResult.mCollisionType == MapPhysics.CollisionType.stop) {
            mCharacter.mMovingData.mRemainingDistance = 0;
            return false;
        }
        //残りの移動距離を減算
        mCharacter.mMovingData.mRemainingDistance -= mDeltaDistance;
        return mCharacter.mMovingData.mRemainingDistance > 0;
    }

    ///<summary>当たり判定を貫通しない程度に移動</summary>
    private static MoveResult moveDelta() {
        //ここで移動させる距離
        float tDistance = (mRemainingDistance < mDeltaDistance) ? mRemainingDistance : mDeltaDistance;
        //移動ベクトル
        Vector2 tVector = mDirection.matchLength(tDistance);
        return moveToward(tVector);
    }
    /// <summary>ベクトル分移動</summary>
    private static MoveResult moveToward(Vector2 aVector) {
        //衝突する移動制限tileを探索
        RaycastHit2D[] tCollidedRistrictTiles = castRistrictTile(aVector, aVector.magnitude);

        RaycastHit2D[] tCollidedAttributes;
        MoveResult tResult;
        if (tCollidedRistrictTiles.Length == 0) {
            //衝突する移動制限tileなし
            tResult = linearMove(aVector, out tCollidedAttributes);
            if (tResult.mCollisionType != MapPhysics.CollisionType.collide)
                return tResult;
            //衝突した
            MoveResult tSlideResult = slide(aVector.matchLength(aVector.magnitude - tResult.mDistance), tCollidedAttributes);
            tSlideResult.mDistance += tResult.mDistance;
            return tSlideResult;
        } else {
            //衝突する移動制限tileあり
            tResult = linearMove(aVector.matchLength(tCollidedRistrictTiles[0].distance), out tCollidedAttributes);
            switch (tResult.mCollisionType) {
                case MapPhysics.CollisionType.pass:
                    MoveResult tRistrictMoveResult = ristrictMove(aVector.matchLength(aVector.magnitude - tResult.mDistance), tCollidedRistrictTiles);
                    tRistrictMoveResult.mDistance += tResult.mDistance;
                    return tRistrictMoveResult;
                case MapPhysics.CollisionType.collide:
                    MoveResult tSlideResult = slide(aVector.matchLength(aVector.magnitude - tResult.mDistance), tCollidedAttributes);
                    tSlideResult.mDistance += tResult.mDistance;
                    return tSlideResult;
                case MapPhysics.CollisionType.stop:
                    return tResult;
            }
            throw new Exception("MapCharacterMoveSystem : 未定義の衝突結果");
        }


    }
    /// <summary>スライド移動する</summary>
    /// <returns>移動結果</returns>
    /// <param name="aVector">衝突していなければ進んでいた方向ベクトル</param>
    /// <param name="aAttributes">衝突した属性を持つcolliderのHit2D</param>
    private static MoveResult slide(Vector2 aVector, RaycastHit2D[] aAttributes) {
        Vector2 tSlideVector;
        foreach (RaycastHit2D tHit in aAttributes) {
            tSlideVector = aVector.disassembleOrthogonal(tHit.normal);
            MapPhysics.CollisionType tCollisionType;
            castAttribute(tSlideVector, kMaxSeparation * 2, out tCollisionType);
            //ほとんど移動できない場合は次の属性に沿って移動
            if (tCollisionType == MapPhysics.CollisionType.collide) continue;
            //属性に沿って移動
            return moveToward(tSlideVector);
        }
        return new MoveResult(MapPhysics.CollisionType.collide, 0);
    }

    /// <summary>
    /// 移動制限tileの制限にしたがって移動
    /// </summary>
    /// <returns>移動結果</returns>
    /// <param name="aVector">移動方向</param>
    /// <param name="aRistrictTiles">接触した移動制限tileのcolliderのHit2D</param>
    private static MoveResult ristrictMove(Vector2 aVector, RaycastHit2D[] aRistrictTiles) {
        RistrictMovingTile tTile;
        foreach (RaycastHit2D tHit in aRistrictTiles) {
            tTile = tHit.collider.GetComponent<RistrictMovingTile>();
            RistrictMovingTile.RistrictMovingData tData = tTile.getMovingData(mCharacter.worldPosition2D, aVector);
            //tileに接しただけで内部を移動しない
            if (tData.mInternalVector.Length == 0 || (tData.mInternalVector.Length == 1 && tData.mInternalVector[0].magnitude < kMaxSeparation))
                continue;

            //移動制限tile内部で移動
            RaycastHit2D[] tCollidedAttributes;
            MoveResult tResult;
            float tMoveDistance = 0;
            foreach (Vector2 tVector in tData.mInternalVector) {
                tResult = linearMove(tVector, out tCollidedAttributes);
                switch (tResult.mCollisionType) {
                    case MapPhysics.CollisionType.pass:
                        tMoveDistance += tVector.magnitude;
                        continue;
                    case MapPhysics.CollisionType.collide:
                        //スライド移動する
                        Vector2 tSlideVector;
                        MoveResult tSlideResult;
                        Vector2 tRemainedVector = tVector.matchLength(tVector.magnitude - tResult.mDistance);
                        RaycastHit2D[] _;
                        foreach (RaycastHit2D tCollidedAttribute in tCollidedAttributes) {
                            tSlideVector = tRemainedVector.disassembleOrthogonal(tCollidedAttribute.normal);
                            tSlideResult = linearMove(tSlideVector, out _);
                            //移動できなかった場合は次の属性に沿って移動
                            if (tResult.mDistance <= 0) continue;
                            tSlideResult.mDistance += tMoveDistance + tResult.mDistance;
                            return tSlideResult;
                        }
                        return new MoveResult(MapPhysics.CollisionType.collide, tMoveDistance + tResult.mDistance);
                    case MapPhysics.CollisionType.stop:
                        tMoveDistance += tResult.mDistance;
                        return new MoveResult(MapPhysics.CollisionType.stop, tMoveDistance);
                }
            }
        }
        //tileに接しただけで内部を移動しなかった
        RaycastHit2D[] tOut;
        //tileから離れる為にわずかに移動
        MoveResult tLittleMoveResult = linearMove(aVector.matchLength(kMaxSeparation), out tOut);
        if (tLittleMoveResult.mDistance <= 0) {
            return tLittleMoveResult;
        }
        return moveToward(aVector);
    }
    /// <summary>
    /// 指定方向,距離に移動(移動制限tileは無視)
    /// </summary>
    /// <returns>移動結果</returns>
    /// <param name="aVector">移動方向,距離</param>
    /// <param name="oCollidedAtributes">衝突した属性を持つcolliderのHit2D</param>
    private static MoveResult linearMove(Vector2 aVector, out RaycastHit2D[] oCollidedAtributes) {
        MapPhysics.CollisionType tCollisionType;
        RaycastHit2D[] tHitList = castAttribute(aVector, aVector.magnitude, out tCollisionType);
        switch (tCollisionType) {
            case MapPhysics.CollisionType.pass:
                mCharacter.mMapPosition += aVector;
                //move(aVector);
                oCollidedAtributes = new RaycastHit2D[0];
                return new MoveResult(MapPhysics.CollisionType.pass, aVector.magnitude);
            case MapPhysics.CollisionType.collide:
                float tCollideMoveDistance = tHitList[0].distance - kMaxSeparation;
                if (tCollideMoveDistance <= 0) tCollideMoveDistance = 0;
                move(aVector.matchLength(tCollideMoveDistance));
                //mCharacter.mMapPosition += aVector.matchLength(tCollideMoveDistance);
                oCollidedAtributes = tHitList;
                return new MoveResult(MapPhysics.CollisionType.collide, tCollideMoveDistance);
            case MapPhysics.CollisionType.stop:
                float tStopMoveDistance = tHitList[0].distance + kMaxSeparation;
                //move(aVector.matchLength(tStopMoveDistance));
                mCharacter.mMapPosition += aVector.matchLength(tStopMoveDistance);
                oCollidedAtributes = tHitList;
                return new MoveResult(MapPhysics.CollisionType.stop, tStopMoveDistance);
        }
        throw new Exception("MapCharacterMoveSystem : 未定義の衝突結果");
    }

    /// <summary>
    /// 指定方向,距離で最初に衝突する属性を探索
    /// </summary>
    /// <returns>衝突した属性を持つcolliderへのHit2D(同じ距離で複数と衝突する場合は複数返す)</returns>
    /// <param name="aVector">探索方向</param>
    /// <param name="aDistance">探索距離</param>
    /// <param name="oCollisionType">衝突した属性とのcollisionType(同じ距離で複数と衝突する場合はcollideの物のみをListに入れて返す)</param>
    private static RaycastHit2D[] castAttribute(Vector2 aVector, float aDistance, out MapPhysics.CollisionType oCollisionType) {
        //衝突するcolliderを取得
        RaycastHit2D[] tHitList = new RaycastHit2D[0];
        if (mCollider is BoxCollider2D) {
            tHitList = Physics2D.BoxCastAll(mCharacter.worldPosition2D + mCollider.offset, ((BoxCollider2D)mCollider).size, mCharacter.transform.rotation.z, aVector, aDistance);
        } else if (mCollider is CircleCollider2D) {
            tHitList = Physics2D.CircleCastAll(mCharacter.worldPosition2D + mCollider.offset, ((CircleCollider2D)mCollider).radius, aVector, aDistance);
        }
        //衝突する属性を取得
        int tHitLength = tHitList.Length;
        RaycastHit2D[] tNears = new RaycastHit2D[tHitLength];
        RaycastHit2D tHit;
        MapPhysicsAttribute tAttribute;
        MapPhysics.CollisionType tCollisionType = MapPhysics.CollisionType.pass;
        int tHitIndex;
        int tNearsNum = 0;//同じ距離で衝突する属性の数
        for (tHitIndex = 0; tHitIndex < tHitLength; ++tHitIndex) {
            tHit = tHitList[tHitIndex];
            tAttribute = getCollidedAttribute(tHit.collider.gameObject, out tCollisionType);
            if (tCollisionType == MapPhysics.CollisionType.pass) continue;
            tNears[0] = tHit;
            tNearsNum = 1;
            break;
        }
        //衝突する属性がなかった
        if (tNearsNum == 0) {
            oCollisionType = MapPhysics.CollisionType.pass;
            return new RaycastHit2D[0];
        }
        //同じ距離で衝突する属性を取得
        //stopよりcollideを優先するためのフラグ
        bool tCollided = (tCollisionType == MapPhysics.CollisionType.collide);
        for (int i = tHitIndex + 1; i < tHitLength; ++i) {
            tHit = tHitList[i];
            //同じ距離で衝突する属性なし
            if (tNears[0].distance < tHit.distance)
                break;
            tAttribute = getCollidedAttribute(tHit.collider.gameObject, out tCollisionType);
            switch (tCollisionType) {
                case MapPhysics.CollisionType.pass:
                    continue;
                case MapPhysics.CollisionType.collide:
                    if (tCollided) {
                        tNears[tNearsNum] = tHit;
                        tNearsNum++;
                    } else {
                        tNears[0] = tHit;
                        tNearsNum = 1;
                        tCollided = true;
                    }
                    continue;
                case MapPhysics.CollisionType.stop:
                    if (!tCollided) {
                        tNears[tNearsNum] = tHit;
                        tNearsNum++;
                    }
                    continue;
            }
        }
        Array.Resize<RaycastHit2D>(ref tNears, tNearsNum);
        oCollisionType = (tCollided) ? MapPhysics.CollisionType.collide : MapPhysics.CollisionType.stop;
        return tNears;
    }

    /// <summary>引数のbehaviourが衝突する属性を持つか調べる</summary>
    private static MapPhysicsAttribute getCollidedAttribute(GameObject aBehaviour, out MapPhysics.CollisionType oCollisionType) {
        if (aBehaviour == mCollider.gameObject) {//自分の属性とは衝突しない
            oCollisionType = MapPhysics.CollisionType.pass;
            return null;
        }

        MapPhysicsAttribute tCollidedAttribute = null;
        oCollisionType = MapPhysics.CollisionType.pass;
        foreach (MapPhysicsAttribute tAttribute in aBehaviour.GetComponents<MapPhysicsAttribute>()) {
            MapPhysics.CollisionType tCollisionType = MapPhysics.canCollide(mCharacter.mAttribute, tAttribute);
            string t = aBehaviour.transform.parent.name;
            if (tCollisionType == MapPhysics.CollisionType.collide) {
                tCollidedAttribute = tAttribute;
                oCollisionType = MapPhysics.CollisionType.collide;
                break;
            }
            if (tCollisionType == MapPhysics.CollisionType.stop) {
                tCollidedAttribute = tAttribute;
                oCollisionType = MapPhysics.CollisionType.stop;
            }
        }
        return tCollidedAttribute;
    }
    /// <summary>
    /// 指定方向,距離で最初に衝突する移動制限tileを探索
    /// </summary>
    /// <returns>衝突した移動制限tileのcolliderのHit2D</returns>
    /// <param name="aVector">探索方向</param>
    /// <param name="aDistance">探索距離</param>
    private static RaycastHit2D[] castRistrictTile(Vector2 aVector, float aDistance) {
        //衝突するcolliderを取得
        RaycastHit2D[] tHitList = Physics2D.RaycastAll(mCharacter.worldPosition2D, aVector, aDistance);
        int tHitLength = tHitList.Length;
        RaycastHit2D[] tNears = new RaycastHit2D[tHitList.Length];
        int tHitIndex;
        RaycastHit2D tHit;
        RistrictMovingTile tTile;
        int tNearsNum = 0;
        //衝突する移動制限tileがあるか探す
        for (tHitIndex = 0; tHitIndex < tHitLength; ++tHitIndex) {
            tHit = tHitList[tHitIndex];
            tTile = tHit.collider.GetComponent<RistrictMovingTile>();
            if (tTile == null) continue;
            if (!MapPhysics.canCollide(mCharacter.mAttribute, tTile)) continue;
            tNears[0] = tHit;
            tNearsNum = 1;
            break;
        }
        if (tNearsNum == 0) {
            return new RaycastHit2D[0];
        }
        //同じ距離で衝突する移動制限tileがあるか探す
        for (int i = tHitIndex + 1; i < tHitLength; ++i) {
            tHit = tHitList[tHitIndex];
            //同じ距離で衝突するcolliderなし
            if (tNears[0].distance < tHit.distance) break;
            //同じ距離で衝突するcolliderあり
            tTile = tHit.collider.GetComponent<RistrictMovingTile>();
            if (tTile == null) continue;
            if (!MapPhysics.canCollide(mCharacter.mAttribute, tTile)) continue;
            tNears[tNearsNum] = tHit;
            tNearsNum++;
            break;
        }
        Array.Resize<RaycastHit2D>(ref tNears, tNearsNum);
        return tNears;
    }
    private static void move(Vector2 aVector) {
        //移動方向単位ベクトル
        Vector2 tNormal = aVector.normalized;
        float tLength = aVector.magnitude;
        for (; tLength > 0; tLength -= kMaxSeparation) {
            switch (tryMove(tNormal * tLength)) {
                case MapPhysics.CollisionType.pass:
                case MapPhysics.CollisionType.stop:
                    mCharacter.mMapPosition += tNormal * tLength;
                    return;
                case MapPhysics.CollisionType.collide:
                    continue;
            }
        }
    }
    /// <summary>
    /// 指定距離移動した場合の衝突状況を返す
    /// </summary>
    /// <returns>衝突状況</returns>
    /// <param name="aVector">移動方向,距離</param>
    private static MapPhysics.CollisionType tryMove(Vector2 aVector) {
        //衝突するcolliderを取得
        Collider2D[] tHitList = new Collider2D[0];
        if (mCollider is BoxCollider2D) {
            tHitList = Physics2D.OverlapBoxAll(mCharacter.worldPosition2D + aVector + mCollider.offset, ((BoxCollider2D)mCollider).size, mCharacter.transform.rotation.z);
        } else if (mCollider is CircleCollider2D) {
            tHitList = Physics2D.OverlapCircleAll(mCharacter.worldPosition2D + aVector, ((CircleCollider2D)mCollider).radius);
        }

        MapPhysics.CollisionType tCollisionType;
        bool tStopFlag = false;
        foreach (Collider2D tCollider in tHitList) {
            getCollidedAttribute(tCollider.gameObject, out tCollisionType);
            if (tCollisionType == MapPhysics.CollisionType.collide)
                return MapPhysics.CollisionType.collide;
            if (tCollisionType == MapPhysics.CollisionType.stop)
                tStopFlag = true;
        }
        return (tStopFlag) ? MapPhysics.CollisionType.stop : MapPhysics.CollisionType.pass;
    }

    public class MoveResult {
        public MapPhysics.CollisionType mCollisionType;
        public float mDistance;
        public MoveResult(MapPhysics.CollisionType aCollisionType, float aDistance) {
            mCollisionType = aCollisionType;
            mDistance = aDistance;
        }
    }

    //<summary>(誤差を考慮した上で)指定座標に居るならtrue</summary>
    public static bool arrived(MapCharacter aCharacter, Vector2 aPosition) {
        return Vector2.Distance(aCharacter.mMapPosition, aPosition) <= kMaxSeparation;
    }
}
