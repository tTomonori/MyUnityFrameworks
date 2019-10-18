using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapWorldUpdater {
    //<summary>フレーム毎の更新</summary>
    public static void updateWorld(MapWorld aWorld) {
        //キャラの内部状態を更新して移動先決定
        foreach (MapCharacter tChara in aWorld.mCharacters) {
            tChara.updateInternalState();
            //移動用データ初期化
            MapCharacterMoveSystem.initFrameMovingData(tChara);
        }
        //キャラの移動
        //まだ移動可能なキャラ
        List<MapCharacter> tWaiting = new List<MapCharacter>();
        List<MapCharacter> tProcessing = new List<MapCharacter>(aWorld.mCharacters);
        bool tExistWaiting = false;
        for (; ; ) {
            foreach (MapCharacter tCharacter in tProcessing) {
                //移動
                bool tRemainedDistance;
                if (tCharacter.mMovingData.mRemainingDistance > 0) {
                    tRemainedDistance = MapCharacterMoveSystem.moveCharacter(tCharacter);
                    //高さ更新
                    MapHeightUpdateSystem.updateHeight(tCharacter);
                    //trigger
                    MapTriggerUpdater.trigger(tCharacter);
                    //encount
                } else { tRemainedDistance = false; }

                //まだ移動できるか
                if (tRemainedDistance) {
                    tWaiting.Add(tCharacter);
                    tExistWaiting = true;
                }

                //これ以上移動しない
                //座標適用
                tCharacter.applyPosition();
                //画像イベント
                applyImageEvent(tCharacter);
                //移動データリセット
                MapCharacterMoveSystem.resetFrameMovingData(tCharacter);
            }
            if (!tExistWaiting)
                break;
            tProcessing = tWaiting;
            tWaiting.Clear();
            tExistWaiting = false;
        }
        //エンカウント
        //話しかけるor調べる
        foreach (MapCharacter tCharacter in aWorld.mCharacters) {
            if (!tCharacter.mMovingData.mSpeak) continue;
            MapSpeakUpdater.speak(tCharacter);
            tCharacter.mMovingData.mSpeak = false;
        }
    }
    /// <summary>画像イベントを適用する</summary>
    static public void applyImageEvent(MapEntity aBehaviour) {
        ImageEventTrigger tTrigger;
        ImageEventData tImageEventData = new ImageEventData();
        foreach (Collider2D tCollider in Physics2D.OverlapPointAll(aBehaviour.worldPosition2D)) {
            tTrigger = tCollider.GetComponent<ImageEventTrigger>();
            if (tTrigger == null) continue;
            if (!MapPhysics.isOverlapedH(aBehaviour, tTrigger)) continue;
            tTrigger.plusEvent(tImageEventData, aBehaviour);
        }
        aBehaviour.mImage.applyImageEvent(tImageEventData);
    }
}
