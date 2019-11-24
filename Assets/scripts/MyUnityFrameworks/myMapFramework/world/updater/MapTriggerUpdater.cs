using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapTriggerUpdater {
    /// <summary>衝突したtriggerを発火させる</summary>
    static public void trigger(MapCharacter aEntity, MapEventSystem aEventSystem) {
        bool tMoved = aEntity.mMovingData.mPrePosition != aEntity.mMapPosition;
        List<MapTrigger> tCollidedTriggers = getCollidedTriggers(aEntity);

        foreach (MapTrigger tTrigger in tCollidedTriggers) {
            int tCount = aEntity.mMovingData.mCollidedTriggers.Count;
            bool tFind = false;
            for (int i = 0; i < tCount; ++i) {
                if (aEntity.mMovingData.mCollidedTriggers[i] != tTrigger) continue;

                if (tMoved) tTrigger.moved(aEntity, aEventSystem);//内部移動
                else tTrigger.stay(aEntity, aEventSystem);//内部停止

                aEntity.mMovingData.mCollidedTriggers.RemoveAt(i);
                tFind = true;
                break;
            }
            if (tFind) continue;
            tTrigger.enter(aEntity, aEventSystem);//侵入
        }
        foreach (MapTrigger tTrigger in aEntity.mMovingData.mCollidedTriggers) {
            tTrigger.exit(aEntity, aEventSystem);//撤退
        }
        aEntity.mMovingData.mCollidedTriggers = tCollidedTriggers;
    }
    /// <summary>衝突しているtriggerを取得</summary>
    static private List<MapTrigger> getCollidedTriggers(MapEntity aEntity) {
        RaycastHit[] tHits = new RaycastHit[0];
        //衝突しているcollider取得
        switch (aEntity.mEntityPhysicsBehaviour.mAttriubteCollider) {
            case BoxCollider box:
                tHits = Physics.BoxCastAll(box.gameObject.transform.position + box.center, box.size / 2f, new Vector3(0,1,0),box.transform.rotation, 0);
                break;
            case SphereCollider sphere:
                tHits = Physics.SphereCastAll(sphere.transform.position + sphere.center, sphere.radius, Vector3.zero);
                break;
        }
        //triggerを取得
        List<MapTrigger> tTriggers = new List<MapTrigger>();
        MapTrigger tTrigger;
        foreach (RaycastHit tHit in tHits) {
            tTrigger = tHit.collider.gameObject.GetComponent<MapTrigger>();
            if (tTrigger == null) continue;
            tTriggers.Add(tTrigger);
        }
        return tTriggers;
    }
    /// <summary>trigger衝突状況データを初期化</summary>
    static public void initTriggerDataOfMovingData(MapCharacter aCharacter) {
        List<MapTrigger> tTriggers = getCollidedTriggers(aCharacter);
        foreach (MapTrigger tTrigger in tTriggers) {
            tTrigger.existInner(aCharacter);
        }
        aCharacter.mMovingData.mCollidedTriggers = tTriggers;
    }
}
