using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapHeightUpdateSystem {
    /// <summary>キャラの高さを更新する(座標を変更した場合はtrue)</summary>
    public static bool updateHeight(MapEntity aEntity) {
        float tDistance = getDistanceToScaffold(aEntity);
        if (tDistance == 0) return false;
        //if (-MapCharacterMoveSystem.kMaxSeparation <= tDistance && tDistance <= MapCharacterMoveSystem.kMaxSeparation) return false;
        aEntity.mMapPosition += new Vector3(0, -tDistance + MapCharacterMoveSystem.kMaxSeparation, 0);
        return true;
    }
    /// <summary>
    /// 足場までの距離を返す(負の値の場合は上方向の距離を示す)
    /// </summary>
    /// <returns>足場までの距離</returns>
    /// <param name="aEntity">A entity.</param>
    public static float getDistanceToScaffold(MapEntity aEntity) {
        RaycastHit[] tHits = scaffoldRigideCast(aEntity, 0, 2f);
        //最寄りの足場を探す
        MapScaffold tScaffold = null;
        RaycastHit tHit;
        Collider tScaffoldCollider = null;
        for (int i = 0; i < tHits.Length; ++i) {
            tHit = tHits[i];
            tScaffold = tHit.collider.gameObject.GetComponent<MapScaffold>();
            if (tScaffold == null) continue;
            if (tHit.distance > 0) {
                return tHit.distance;
            } else {
                //足場と衝突している場合
                tScaffoldCollider = tHit.collider;
                break;
            }
        }
        if (tScaffold == null) {
            Debug.LogWarning("MapHeightUpdateSystem : 足場がない");
            return 0;
        }
        //足場と衝突している場合
        float tUpperDistance = 0;
        for (; ; ) {
            if (tScaffold == null) break;
            //上方向に移動して離せる距離
            tUpperDistance = getCorrectDistanceToScaffold(aEntity, tScaffoldCollider);
            //移動した結果他の足場に衝突しないか確認
            tHits = scaffoldRigideCast(aEntity, tUpperDistance + MapCharacterMoveSystem.kMaxSeparation, 0);
            tScaffold = null;
            foreach (RaycastHit tCastHit in tHits) {
                tScaffold = tCastHit.collider.GetComponent<MapScaffold>();
                if (tScaffold != null) {//他の足場に衝突した
                    tScaffoldCollider = tCastHit.collider;
                    break;
                }
            }
        }
        return -tUpperDistance;
    }
    /// <summary>
    /// 指定した足場に接する位置までの距離を返す
    /// </summary>
    /// <returns>指定した足場に接する位置までの距離(Y座標上方向の距離)</returns>
    /// <param name="aEntity">Entity</param>
    /// <param name="aScaffold">足場</param>
    private static float getCorrectDistanceToScaffold(MapEntity aEntity, Collider aScaffold) {
        Collider tEntityCollider = aEntity.mEntityPhysicsBehaviour.mScaffoldRigideCollider;
        //上方向に移動させて離せる距離
        ColliderEditer.CubeEndPoint tEntityEnd = tEntityCollider.minimumCircumscribedCubeEndPoint();
        ColliderEditer.CubeEndPoint tHitEnd = aScaffold.minimumCircumscribedCubeEndPointWorld();
        float tUpperDistance = (tHitEnd.top) - (tEntityCollider.transform.position.y + tEntityEnd.bottom);
        //cast
        RaycastHit[] tHits = scaffoldRigideCast(aEntity, tUpperDistance, tHitEnd.top - tHitEnd.bottom);
        //指定されたscaffoldのHitを探す
        for (int i = 0; i < tHits.Length; ++i) {
            RaycastHit tHit = tHits[i];
            MapScaffold tScaffold = tHit.collider.gameObject.GetComponent<MapScaffold>();
            if (tScaffold == null) continue;
            if (tHit.collider != aScaffold) continue;
            return tUpperDistance - tHit.distance;
        }
        //return tUpperDistance - (tHitEnd.top - tHitEnd.bottom);
        throw new System.Exception("MapHeightUpdateSystem : 衝突するはずの足場と衝突できなかった");
    }
    /// <summary>
    /// ScaffoldRigideのcastを飛ばす
    /// </summary>
    /// <returns>cast結果</returns>
    /// <param name="aEntity">A entity.</param>
    /// <param name="aHeightOffset">entityからcastの起点までの高さ(castの起点が上なら正)</param>
    /// <param name="aDistance">castする距離</param>
    public static RaycastHit[] scaffoldRigideCast(MapEntity aEntity, float aHeightOffset, float aDistance) {
        RaycastHit[] tCast = new RaycastHit[0];
        switch (aEntity.mEntityPhysicsBehaviour.mScaffoldRigideCollider) {
            case BoxCollider box:
                tCast = Physics.BoxCastAll(box.gameObject.transform.position + box.center + new Vector3(0, aHeightOffset, 0), box.size / 2f, new Vector3(0, -1, 0), new Quaternion(0, 0, 0, 1), aDistance);
                break;
            case SphereCollider sphere:
                tCast = Physics.SphereCastAll(sphere.gameObject.transform.position + sphere.center + new Vector3(0, aHeightOffset, 0), sphere.radius, new Vector3(0, -1, 0), aDistance);
                break;
            default:
                throw new System.Exception("MapHeightUpdateSystem : 未定義のcast「" + aEntity.mEntityPhysicsBehaviour.mScaffoldRigideCollider.GetType() + "」");
        }
        if (tCast.Length < 2) return tCast;
        //衝突距離昇順にソート
        Array.Sort(tCast, (x, y) => {
            if (x.distance >= y.distance) return 1;
            else return -1;
        });
        return tCast;
    }
}
