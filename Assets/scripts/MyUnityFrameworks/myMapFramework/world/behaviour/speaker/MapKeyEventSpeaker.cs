using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapKeyEventSpeaker : MapSpeaker {
    public string mSpeakDefault;
    public string mSpeakFromUp;
    public string mSpeakFromDown;
    public string mSpeakFromLeft;
    public string mSpeakFromRight;
    public override bool canReply(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        if (mSpeakDefault != "") return true;
        return getAnswerKey(aCharacter) != "";
    }
    public override void speak(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        string mAnswerKey = getAnswerKey(aCharacter);
        if (mAnswerKey == "") return;
        aEventSystem.addEvent(mAnswerKey, aCharacter, mBehaviour, mCollider);
    }
    /// <summary>引数のentityに話しかけられた時に発火するイベントのkeyを取得</summary>
    public string getAnswerKey(MapEntity aEntity) {
        //最小外接矩形の距離ベクトル
        Vector2 tDistance = ColliderEditer.planeDistance(aEntity.mEntityPhysicsBehaviour.mAttriubteCollider, this.gameObject.GetComponent<Collider>());
        if (tDistance == Vector2.zero) {//最小外接矩形が重なっていた場合は座標の距離ベクトルを使う
            tDistance = aEntity.mMapPosition.vector2 - mBehaviour.mMapPosition.vector2;
        }
        //話かけてきた方向で分岐
        switch (DirectionOperator.convertToDirection(tDistance)) {
            case Direction.up:
                if (mSpeakFromUp != "") return mSpeakFromUp;
                break;
            case Direction.down:
                if (mSpeakFromDown != "") return mSpeakFromDown;
                break;
            case Direction.left:
                if (mSpeakFromLeft != "") return mSpeakFromLeft;
                break;
            case Direction.right:
                if (mSpeakFromRight != "") return mSpeakFromRight;
                break;
        }
        return mSpeakDefault;
    }
}
