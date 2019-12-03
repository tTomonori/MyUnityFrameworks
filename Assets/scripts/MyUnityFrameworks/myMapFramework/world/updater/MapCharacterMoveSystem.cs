using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapCharacterMoveSystem {
    ///<summary>障害物と衝突した時、障害物との距離の最大許容距離</summary>
    static public float kMaxSeparation { get; private set; } = 0.02f;
    ///<summary>当たり判定を貫通しない程度に移動して良い最大距離</summary>
    static public float kMaxDeltaDistance { get; private set; } = 0.4f;
    //移動させるキャラ
    private static MapCharacter mCharacter;
    //移動させるキャラの属性
    private static EntityPhysicsAttribute mAttribute;
    //移動させるキャラに付いているcollider
    private static Collider mCollider;
    //移動方向(単位ベクトル)
    private static Vector3 mDirection;
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
    }
    /// <summary>移動完了後に移動データリセット</summary>
    public static void resetFrameMovingData(MapCharacter aCharacter) {
        aCharacter.mMovingData.mDirection = Vector2.zero;
        aCharacter.mMovingData.mMaxMoveDistance = float.PositiveInfinity;
    }
    /// <summary>最後のdelta移動処理をリセットする</summary>
    public static void resetMoveDelta(MapCharacter aCharacter) {
        aCharacter.mMapPosition = aCharacter.mMovingData.mDeltaPrePosition;
    }
    ///<summary>キャラを移動させる(同フレームでさらに移動可能ならtrue)</summary>
    public static bool moveCharacter(MapCharacter aCharacter) {
        if (aCharacter.mMovingData.mDirection == Vector3.zero) {
            //移動しない場合
            aCharacter.mMovingData.mRemainingDistance = 0;
            return false;
        }
        //移動処理前の座標を記録
        aCharacter.mMovingData.mDeltaPrePosition = aCharacter.mMapPosition;
        //最後の移動方向記録
        aCharacter.mMovingData.mLastDirection = aCharacter.mMovingData.mDirection;
        //移動処理で使うデータ収集・記録
        mCharacter = aCharacter;
        mAttribute = aCharacter.mEntityPhysicsBehaviour.mAttribute;
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
        Vector3 tVector = mDirection.matchLength(tDistance);
        return moveToward(tVector);
    }
    /// <summary>ベクトル分移動</summary>
    private static MoveResult moveToward(Vector3 aVector) {
        //衝突する移動制限tileを探索
        RaycastHit[] tCollidedRistrictTiles = castRistrictTile(aVector, aVector.magnitude);

        RaycastHit[] tCollidedAttributes;
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
    /// <param name="aAttributes">衝突した属性を持つcolliderのHit</param>
    private static MoveResult slide(Vector3 aVector, RaycastHit[] aAttributes) {
        Vector3 tSlideVector;
        Vector2 tArg2DVector = new Vector2(aVector.x, aVector.z);
        foreach (RaycastHit tHit in aAttributes) {
            if (aVector.y == 0) {
                Vector2 tV = tArg2DVector.disassembleOrthogonal(new Vector2(tHit.normal.x, tHit.normal.z));
                tSlideVector = new Vector3(tV.x, 0, tV.y);
            } else {
                tSlideVector = aVector.disassembleOrthogonal(tHit.normal);
            }
            //スライド移動しようとしても移動方向がかわらない場合は衝突として扱う
            if (aVector == tSlideVector) return new MoveResult(MapPhysics.CollisionType.collide, 0);
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
    /// <param name="aRistrictTiles">接触した移動制限tileのcolliderのHit</param>
    private static MoveResult ristrictMove(Vector3 aVector, RaycastHit[] aRistrictTiles) {
        RistrictMovingTile.RistrictMovingData tMovingData;
        MoveResult tResult;
        MoveResult tMinResult;
        RaycastHit[] tHitList;

        //移動制限tileにしたがって移動
        tResult = ristrictMoveInner(aVector, aRistrictTiles, out tMovingData);
        //衝突,止められた,これ以上移動しない場合は移動完了
        if (tResult.mCollisionType != MapPhysics.CollisionType.pass || tMovingData.mOutsideVector == Vector3.zero) {
            return tResult;
        }
        //移動制限tile外部で移動制限tileを探して移動
        for (; ; ) {
            //移動制限tileを探す
            tHitList = castRistrictTile(tMovingData.mLastInternalDirection, kMaxSeparation);
            //移動制限tileが見つからなかった
            if (tHitList.Length == 0) break;
            //移動制限tileにしたがって移動
            tMinResult = ristrictMoveInner(tMovingData.mOutsideVector, tHitList, out tMovingData);
            //衝突,止められた場合は移動完了
            if (tMinResult.mCollisionType != MapPhysics.CollisionType.pass || tMovingData.mOutsideVector == Vector3.zero) {
                tMinResult.mDistance = tResult.mDistance;
                return tMinResult;
            }
            tResult.mDistance = tMinResult.mDistance;
            //tileに接しただけで内部を移動しなかった
            if (tMinResult.mDistance <= 0) {
                break;
            }
        }
        //移動制限tile外部で直線移動する
        RaycastHit[] t;
        MoveResult tOutResult = linearMove(tMovingData.mOutsideVector, out t);
        tOutResult.mDistance += tResult.mDistance;
        return tOutResult;
    }
    /// <summary>
    /// 移動制限tile内部で移動
    /// </summary>
    /// <returns>移動結果</returns>
    /// <param name="aVector">移動制限tile外部で移動するはずの移動ベクトル</param>
    /// <param name="aRistrictTiles">移動制限tileのcolliderのHit</param>
    /// <param name="oMovingData">制限移動データ</param>
    private static MoveResult ristrictMoveInner(Vector3 aVector, RaycastHit[] aRistrictTiles, out RistrictMovingTile.RistrictMovingData oMovingData) {
        RistrictMovingTile tTile;
        oMovingData = new RistrictMovingTile.RistrictMovingData();
        foreach (RaycastHit tHit in aRistrictTiles) {
            tTile = tHit.collider.GetComponent<RistrictMovingTile>();
            RistrictMovingTile.RistrictMovingData tData = tTile.getMovingData(mCharacter.mMapPosition.vector - tTile.mTile.mMapPosition.vector, aVector);
            oMovingData = tData;
            //tileに接しただけで内部を移動しない
            if (tData.mInternalVector.Length == 0)
                continue;

            //移動制限tile内部で移動
            RaycastHit[] tCollidedAttributes;
            MoveResult tResult;
            float tMoveDistance = 0;
            foreach (Vector3 tVector in tData.mInternalVector) {
                tResult = linearMove(tVector, out tCollidedAttributes);
                switch (tResult.mCollisionType) {
                    case MapPhysics.CollisionType.pass:
                        tMoveDistance += tVector.magnitude;
                        continue;
                    case MapPhysics.CollisionType.collide:
                        //スライド移動する
                        Vector3 tSlideVector;
                        MoveResult tSlideResult;
                        Vector3 tRemainedVector = tVector.matchLength(tVector.magnitude - tResult.mDistance);
                        RaycastHit[] _;
                        foreach (RaycastHit tCollidedAttribute in tCollidedAttributes) {
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
            //移動制限tile内部での移動終了後
            return new MoveResult(MapPhysics.CollisionType.pass, tMoveDistance);
        }
        return new MoveResult(MapPhysics.CollisionType.pass, 0);
    }
    /// <summary>
    /// 指定方向,距離に移動(移動制限tileは無視)
    /// </summary>
    /// <returns>移動結果</returns>
    /// <param name="aVector">移動方向,距離</param>
    /// <param name="oCollidedAtributes">衝突した属性を持つcolliderのHit</param>
    private static MoveResult linearMove(Vector3 aVector, out RaycastHit[] oCollidedAtributes) {
        MapPhysics.CollisionType tCollisionType;
        RaycastHit[] tHitList = castAttribute(aVector, aVector.magnitude, out tCollisionType);
        switch (tCollisionType) {
            case MapPhysics.CollisionType.pass:
                move(aVector);
                oCollidedAtributes = new RaycastHit[0];
                return new MoveResult(MapPhysics.CollisionType.pass, aVector.magnitude);
            case MapPhysics.CollisionType.collide:
                float tCollideMoveDistance = tHitList[0].distance - kMaxSeparation;
                if (tCollideMoveDistance <= 0) tCollideMoveDistance = 0;
                //移動先で属性と衝突しないか確認してから移動する
                move(aVector.matchLength(tCollideMoveDistance));
                oCollidedAtributes = tHitList;
                return new MoveResult(MapPhysics.CollisionType.collide, tCollideMoveDistance);
            case MapPhysics.CollisionType.stop:
                float tStopMoveDistance = tHitList[0].distance + kMaxSeparation;
                move(aVector.matchLength(tStopMoveDistance));
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
    private static RaycastHit[] castAttribute(Vector3 aVector, float aDistance, out MapPhysics.CollisionType oCollisionType) {
        //衝突するcolliderを取得
        RaycastHit[] tHitList = cast(aVector, aDistance);
        //衝突する属性を取得
        int tHitLength = tHitList.Length;
        RaycastHit[] tNears = new RaycastHit[tHitLength];
        RaycastHit tHit;
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
            return new RaycastHit[0];
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
        Array.Resize<RaycastHit>(ref tNears, tNearsNum);
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
            MapPhysics.CollisionType tCollisionType = MapPhysics.canCollide(mAttribute, tAttribute);
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
    private static RaycastHit[] castRistrictTile(Vector3 aVector, float aDistance) {
        //衝突するcolliderを取得
        RaycastHit[] tHitList = castHeight(aVector, aDistance);
        int tHitLength = tHitList.Length;
        RaycastHit[] tNears = new RaycastHit[tHitList.Length];
        int tHitIndex;
        RaycastHit tHit;
        RistrictMovingTile tTile;
        int tNearsNum = 0;
        //衝突する移動制限tileがあるか探す
        for (tHitIndex = 0; tHitIndex < tHitLength; ++tHitIndex) {
            tHit = tHitList[tHitIndex];
            tTile = tHit.collider.GetComponent<RistrictMovingTile>();
            if (tTile == null) continue;
            tNears[0] = tHit;
            tNearsNum = 1;
            break;
        }
        if (tNearsNum == 0) {
            return new RaycastHit[0];
        }
        //同じ距離で衝突する移動制限tileがあるか探す
        for (int i = tHitIndex + 1; i < tHitLength; ++i) {
            tHit = tHitList[i];
            //同じ距離で衝突するcolliderなし
            if (tNears[0].distance < tHit.distance) break;
            //同じ距離で衝突するcolliderあり
            tTile = tHit.collider.GetComponent<RistrictMovingTile>();
            if (tTile == null) continue;
            tNears[tNearsNum] = tHit;
            tNearsNum++;
        }
        Array.Resize<RaycastHit>(ref tNears, tNearsNum);
        return tNears;
    }
    private static void move(Vector3 aVector) {
        //移動方向単位ベクトル
        Vector3 tNormal = aVector.normalized;
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
    private static MapPhysics.CollisionType tryMove(Vector3 aVector) {
        //衝突するcolliderを取得
        RaycastHit[] tHitList = cast(aVector, aVector.magnitude);

        MapPhysics.CollisionType tCollisionType;
        bool tStopFlag = false;
        foreach (RaycastHit tHit in tHitList) {
            getCollidedAttribute(tHit.collider.gameObject, out tCollisionType);
            if (tCollisionType == MapPhysics.CollisionType.collide)
                return MapPhysics.CollisionType.collide;
            if (tCollisionType == MapPhysics.CollisionType.stop)
                tStopFlag = true;
        }
        return (tStopFlag) ? MapPhysics.CollisionType.stop : MapPhysics.CollisionType.pass;
    }

    /// <summary>
    /// 移動するキャラのcolliderのrayを飛ばす
    /// </summary>
    /// <returns>衝突したcollider</returns>
    /// <param name="aVector">cast方向</param>
    /// <param name="aDistance">cast距離</param>
    private static RaycastHit[] cast(Vector3 aVector, float aDistance) {
        RaycastHit[] tHitList = new RaycastHit[0];
        if (mCollider is BoxCollider) {
            tHitList = Physics.BoxCastAll(mCharacter.mPhysicsBehaviour.worldPosition + ((BoxCollider)mCollider).center, ((BoxCollider)mCollider).size / 2f, aVector, mCollider.transform.rotation, aDistance);
        } else if (mCollider is SphereCollider) {
            tHitList = Physics.SphereCastAll(mCharacter.mPhysicsBehaviour.worldPosition + ((SphereCollider)mCollider).center, ((SphereCollider)mCollider).radius, aVector, aDistance);
        } else {
            Debug.LogWarning("MapCharacterMoveSystem : 未定義のcolliderのcast「" + mCollider.GetType().ToString() + "」");
        }
        return tHitList;
    }
    /// <summary>
    /// 移動するキャラの座標点から高さ分のrayを飛ばす
    /// </summary>
    /// <returns>衝突したcollider</returns>
    /// <param name="aVector">cast方向</param>
    /// <param name="aDistance">cast距離</param>
    private static RaycastHit[] castHeight(Vector3 aVector, float aDistance) {
        RaycastHit[] tHitList = new RaycastHit[0];
        switch (mCollider) {
            case BoxCollider box:
                tHitList = Physics.BoxCastAll(mCharacter.mPhysicsBehaviour.worldPosition + new Vector3(0,box.center.y,0), new Vector3(0, box.size.y / 2f, 0), aVector, Quaternion.Euler(0, 0, 0), aDistance);
                break;
            case SphereCollider sphere:
                tHitList = Physics.BoxCastAll(mCharacter.mPhysicsBehaviour.worldPosition + new Vector3(0,sphere.center.y,0), new Vector3(0, sphere.radius, 0), aVector, Quaternion.Euler(0, 0, 0), aDistance);
                break;
            default:
                Debug.LogWarning("MapCharacterMoveSystem : 未定義のcolliderのcastHeight「" + mCollider.GetType().ToString() + "」");
                break;
        }
        //if (mCollider is BoxCollider) {
        //    tHitList = Physics.BoxCastAll(mCharacter.mPhysicsBehaviour.worldPosition + ((BoxCollider)mCollider).center, new Vector3(0, ((BoxCollider)mCollider).size.y / 2f, 0), aVector, Quaternion.Euler(0, 0, 0), aDistance);
        //} else if (mCollider is SphereCollider) {
        //    tHitList = Physics.BoxCastAll(mCharacter.mPhysicsBehaviour.worldPosition + ((SphereCollider)mCollider).center, new Vector3(0, ((SphereCollider)mCollider).radius, 0), aVector, Quaternion.Euler(0, 0, 0), aDistance);
        //} else {
        //    Debug.LogWarning("MapCharacterMoveSystem : 未定義のcolliderのcastHeight「" + mCollider.GetType().ToString() + "」");
        //}
        return tHitList;
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
    public static bool arrived(MapCharacter aCharacter, Vector3 aPosition) {
        return Vector3.Distance(aCharacter.mMapPosition.vector, aPosition) <= kMaxSeparation;
    }
    //<summary>(誤差を考慮した上で)指定座標(x,z)に居るならtrue</summary>
    public static bool arrived(MapCharacter aCharacter, Vector2 aPosition) {
        return Vector2.Distance(aCharacter.mMapPosition.vector2, aPosition) <= kMaxSeparation;
    }
    /// <summary>何かに衝突しているならtrue</summary>
    public static bool isCollided(MapCharacter aCharacter) {
        mCharacter = aCharacter;
        mCollider = aCharacter.mEntityPhysicsBehaviour.mAttriubteCollider;
        mAttribute = aCharacter.mEntityPhysicsBehaviour.mAttribute;
        MapPhysics.CollisionType tType;
        RaycastHit[] tHits = castAttribute(new Vector3(0, 1, 0), 0, out tType);
        mCharacter = null;
        mCollider = null;
        mAttribute = null;
        return tType == MapPhysics.CollisionType.collide;
    }
}
