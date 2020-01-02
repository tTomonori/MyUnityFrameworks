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
                    if (MapHeightUpdateSystem.updateHeight(tCharacter)) {
                        //高さ更新で座標が変更された
                        if (MapCharacterMoveSystem.isCollided(tCharacter)) {
                            //高さ更新の結果衝突した
                            MapCharacterMoveSystem.resetMoveDelta(tCharacter);//delta移動処理をリセット
                        }
                    }
                    //trigger
                    MapTriggerUpdater.trigger(tCharacter, aWorld.mEventSystem);
                    //encount
                    if (tCharacter.isPlayer() && tCharacter.getOperation() == MapCharacter.Operation.free) {
                        encountCount(tCharacter, aWorld);
                    }
                } else { tRemainedDistance = false; }

                //まだ移動できるか
                if (tRemainedDistance) {
                    tWaiting.Add(tCharacter);
                    tExistWaiting = true;
                }

                //これ以上移動しない
                //画像イベント
                applyImageEvent(tCharacter);
                //影
                MapShadowUpdater.updateShadow(tCharacter);
                //移動データリセット
                MapCharacterMoveSystem.resetFrameMovingData(tCharacter);
                //delegateに移動通知
                if (tCharacter.isPlayer() && tCharacter.getOperation() == MapCharacter.Operation.free) {
                    aWorld.mMap.mDelegate.report("move", new Arg(new Dictionary<string, object>() { { "distance", (tCharacter.mMovingData.mPrePosition.vector2 - tCharacter.mMapPosition.vector2).magnitude } }));
                }
            }
            if (!tExistWaiting)
                break;
            tProcessing = tWaiting;
            tWaiting.Clear();
            tExistWaiting = false;
        }
        //エンカウント
        if (aWorld.mMap.mEncountSystem.mIsFire) {
            if (aWorld.mEventSystem.encount(aWorld.mMap.mEncountSystem.mEncountKey))
                aWorld.mMap.mEncountSystem.resetCount();//エンカウントイベント実行成功
            else
                aWorld.mMap.mEncountSystem.lastCount();//エンカウントイベント実行失敗
        }
        //話しかけるor調べる
        foreach (MapCharacter tCharacter in aWorld.mCharacters) {
            if (!tCharacter.mMovingData.mSpeak) continue;
            MapSpeakUpdater.speak(tCharacter, aWorld.mEventSystem);
            tCharacter.mMovingData.mSpeak = false;
        }
        //待機中のイベントを実行
        aWorld.mEventSystem.runWaitingEvents();
    }
    /// <summary>画像イベントを適用する</summary>
    static public void applyImageEvent(MapEntity aBehaviour) {
        ImageEventTrigger tTrigger;
        ImageEventData tImageEventData = new ImageEventData();
        foreach (Collider tCollider in Physics.OverlapSphere(aBehaviour.worldPosition, 0)) {
            tTrigger = tCollider.GetComponent<ImageEventTrigger>();
            if (tTrigger == null) continue;
            tTrigger.plusEvent(tImageEventData, aBehaviour);
        }
        aBehaviour.mEntityRenderBehaviour.mBody.applyImageEvent(tImageEventData);
    }
    /// <summary>
    /// エンカウントのカウントを進める
    /// </summary>
    /// <returns>エンカウントした場合はtrue</returns>
    /// <param name="aCharacter">Player Character</param>
    /// <param name="aWorld">MapWorld</param>
    static private bool encountCount(MapCharacter aCharacter, MapWorld aWorld) {
        Vector3Int tPosition = aCharacter.mFootCellPosition;
        MapCell tCell = aWorld.mCells[tPosition.x, tPosition.y, tPosition.z];
        //エンカウントしないマス
        if (tCell.mEncountKey == null || tCell.mEncountKey == "") return false;

        //移動距離
        float tDeltaDistance = (aCharacter.mMovingData.mDeltaPrePosition.vector2 - aCharacter.mMapPosition.vector2).magnitude;
        if (!aWorld.mMap.mEncountSystem.count(tCell.mEncountFrequency * tDeltaDistance, tCell.mEncountKey)) return false;

        //エンカウントした
        aCharacter.mMovingData.mRemainingDistance = 0;
        aCharacter.mMovingData.mSpeak = false;
        return true;
    }
}
