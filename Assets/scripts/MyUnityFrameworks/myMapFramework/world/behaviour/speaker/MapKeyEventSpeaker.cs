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
        aEventSystem.addEvent(mAnswerKey, aCharacter, mBehaviour);
    }
    /// <summary>引数のentityに話しかけられた時に発火するイベントのkeyを取得</summary>
    public string getAnswerKey(MapEntity aEntity) {
        ColliderDistance2D tDistance = aEntity.mAttribute.mCollider.Distance(this.gameObject.GetComponent<Collider2D>());
        switch (DirectionOperator.convertToDirection(tDistance.normal)) {
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
