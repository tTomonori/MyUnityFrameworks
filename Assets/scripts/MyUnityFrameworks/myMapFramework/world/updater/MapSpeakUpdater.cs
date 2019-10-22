using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapSpeakUpdater {
    /// <summary>話しかけるor調べる処理</summary>
    static public void speak(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        foreach (MapSpeaker tSpeaker in getTargetSpeakers(aCharacter)) {
            //応答不可
            if (!tSpeaker.canReply(aCharacter)) continue;
            tSpeaker.speak(aCharacter, aEventSystem);
            return;
        }
    }
    /// <summary>話しかけるor調べる対象となるspeakerを優先順位順で取得</summary>
    static public List<MapSpeaker> getTargetSpeakers(MapCharacter aCharacter) {
        //移動していない場合は話しかけれない
        if (aCharacter.mMovingData.mLastDirection == Vector2.zero) return new List<MapSpeaker>();

        List<MapSpeaker> tSpeakers = getSurroundSpeakers(aCharacter);
        List<MapSpeaker> tTargets = new List<MapSpeaker>();
        ColliderDistance2D tDistance;
        CircleCollider2D tCharaCenter = makeCenterCollider(aCharacter);
        float tCorner;
        List<float> tCornerList = new List<float>();
        foreach (MapSpeaker tSpeaker in tSpeakers) {
            //話かける対象となるか
            tDistance = tSpeaker.GetComponent<Collider2D>().Distance(tCharaCenter);
            //移動方向と話しかける対象がいる方向のなす角
            if (tDistance.distance >= 0) {
                tCorner = tDistance.normal.corner(aCharacter.mMovingData.mLastDirection);
            } else {
                tCorner = (tSpeaker.mBehaviour.transform.position - tCharaCenter.transform.position).toVector2().corner(aCharacter.mMovingData.mLastDirection);
            }
            if (tDistance.distance >= 0 && tCorner > 70)
                continue;
            //対象のリストに追加
            bool tAdd = false;
            for (int i = 0; i < tCornerList.Count; ++i) {
                if (tCornerList[i] < tCorner) continue;
                tCornerList.Insert(i, tCorner);
                tTargets.Add(tSpeaker);
                tAdd = true;
                break;
            }
            if (!tAdd) {
                tCornerList.Add(tCorner);
                tTargets.Add(tSpeaker);
            }
        }
        GameObject.Destroy(tCharaCenter.gameObject);
        return tTargets;
    }
    /// <summary>周囲のspeaker取得</summary>
    static public List<MapSpeaker> getSurroundSpeakers(MapCharacter aCharacter) {
        Collider2D[] tColliders = getSurroundColliders(aCharacter);
        List<MapSpeaker> tSpeakers = new List<MapSpeaker>();
        MapSpeaker tSpeaker;
        foreach (Collider2D tCollider in tColliders) {
            tSpeaker = tCollider.GetComponent<MapSpeaker>();
            if (tSpeaker == null) continue;
            if (!MapPhysics.isOverlapedH(aCharacter, tSpeaker.mBehaviour)) continue;
            tSpeakers.Add(tSpeaker);
        }
        return tSpeakers;
    }
    /// <summary>周囲のcolliderを取得</summary>
    static public Collider2D[] getSurroundColliders(MapCharacter aCharacter) {
        if (aCharacter.mAttribute.mCollider is BoxCollider2D) {
            BoxCollider2D tBox = (BoxCollider2D)aCharacter.mAttribute.mCollider;
            return Physics2D.OverlapCapsuleAll(tBox.transform.position.toVector2() + tBox.offset, tBox.size + new Vector2(1f, 1f), (tBox.size.x < tBox.size.y) ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal, tBox.transform.rotation.z);
        }
        if (aCharacter.mAttribute.mCollider is CircleCollider2D) {
            return Physics2D.OverlapCircleAll(aCharacter.mAttribute.mCollider.transform.position, ((CircleCollider2D)(aCharacter.mAttribute.mCollider)).radius);
        }
        return Physics2D.OverlapCircleAll(aCharacter.transform.position, 1f);
    }
    /// <summary>キャラのcolliderの中心点にdistanceをとるためのcolliderを配置</summary>
    static public CircleCollider2D makeCenterCollider(MapCharacter aCharacter) {
        CircleCollider2D tCenter = MyBehaviour.create<CircleCollider2D>();
        if (aCharacter.mAttribute.mCollider is BoxCollider2D) {
            tCenter.transform.position = aCharacter.mAttribute.mCollider.transform.position.toVector2() + ((BoxCollider2D)aCharacter.mAttribute.mCollider).offset;
            tCenter.radius = 0.01f;
            return tCenter;
        }
        tCenter.transform.position = aCharacter.mAttribute.mCollider.transform.position;
        tCenter.radius = 0.01f;
        return tCenter;
    }
}
