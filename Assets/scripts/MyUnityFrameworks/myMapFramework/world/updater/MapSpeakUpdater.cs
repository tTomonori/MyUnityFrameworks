using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapSpeakUpdater {
    /// <summary>話しかけるor調べる処理</summary>
    static public void speak(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        foreach (MapSpeaker tSpeaker in getTargetSpeakers(aCharacter)) {
            //応答不可
            if (!tSpeaker.canReply(aCharacter, aEventSystem)) continue;
            tSpeaker.speak(aCharacter, aEventSystem);
            return;
        }
    }
    /// <summary>話しかけるor調べる対象となるspeakerを優先順位順で取得</summary>
    static public List<MapSpeaker> getTargetSpeakers(MapCharacter aCharacter) {
        //配置後全く移動していない場合は話しかけれない
        if (aCharacter.mMovingData.mLastDirection == Vector3.zero) return new List<MapSpeaker>();

        List<MapSpeaker> tSpeakers = getFrontSpeakers(aCharacter);
        List<MapSpeaker> tTargets = new List<MapSpeaker>();
        float tCorner;
        List<float> tCornerList = new List<float>();
        Vector3 tSeacherPoint = aCharacter.mEntityPhysicsBehaviour.mAttriubteCollider.getCenter();//話しかけるキャラのcolliderの中心点
        Vector3 tClosestPoint;
        Vector2 tCharacterDirection = new Vector2(aCharacter.mMovingData.mLastDirection.x, aCharacter.mMovingData.mLastDirection.z);//キャラが向いている方向
        foreach (MapSpeaker tSpeaker in tSpeakers) {
            //tSeacherPointからspeakerのcolliderへの最寄り点
            tClosestPoint = tSpeaker.mCollider.ClosestPoint(tSeacherPoint);
            //キャラの向きとspeakerへの方向のなす角
            tCorner = tCharacterDirection.cornerAbs(new Vector2(tClosestPoint.x - tSeacherPoint.x, tClosestPoint.z - tSeacherPoint.z));

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
        return tTargets;
    }
    /// <summary>正面の話しかけるor調べる範囲内のspeakerを取得</summary>
    static public List<MapSpeaker> getFrontSpeakers(MapCharacter aCharacter) {
        Collider tCharacterCollider = aCharacter.mEntityPhysicsBehaviour.mAttriubteCollider;
        Vector3 tColliderHalfSize = tCharacterCollider.minimumCircumscribedCube() / 2f;
        //キャラが向いている方向
        float tAngle = new Vector2(-1, 0).corner(new Vector2(aCharacter.mMovingData.mLastDirection.x, aCharacter.mMovingData.mLastDirection.z));
        //調べる範囲
        float tYRate = Mathf.Abs(Mathf.Abs(tAngle) - 90) / 90f;
        Vector3 tSearchSize = new Vector3(tColliderHalfSize.x * (1 - tYRate) + tColliderHalfSize.z * tYRate + aCharacter.mMovingData.mSpeakDistance, tColliderHalfSize.y * 2, tColliderHalfSize.x * tYRate + tColliderHalfSize.z * (1 - tYRate));
        //範囲内のcolliderを取得
        Collider[] tColliders = Physics.OverlapBox(tCharacterCollider.transform.position + tCharacterCollider.getCenter() + aCharacter.mMovingData.mLastDirection.normalized * tSearchSize.x / 2f, tSearchSize / 2f, Quaternion.Euler(0, tAngle, 0));
        //speakerのみ抽出
        List<MapSpeaker> tSpeakers = new List<MapSpeaker>();
        MapSpeaker tSpeaker;
        foreach (Collider tCollider in tColliders) {
            tSpeaker = tCollider.GetComponent<MapSpeaker>();
            if (tSpeaker == null) continue;
            tSpeakers.Add(tSpeaker);
        }
        return tSpeakers;
    }
}
