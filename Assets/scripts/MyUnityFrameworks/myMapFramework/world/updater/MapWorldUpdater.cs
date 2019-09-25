using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapWorldUpdater {
    //<summary>フレーム毎の更新</summary>
    public static void updateWorld(MapWorld aWorld) {
        return;
    //    //キャラの内部状態を更新して移動先決定
    //    foreach (MapCharacter tChara in aWorld.mCharacters) {
    //        tChara.updateInternalState();
    //        //移動用データ初期化
    //        MapCharacterMoveSystem.initFrameMovingData(tChara);
    //    }
    //    //キャラの移動
    //    //まだ移動可能なキャラ
    //    List<MapCharacter> tWaiting = new List<MapCharacter>();
    //    List<MapCharacter> tProcessing = new List<MapCharacter>(aWorld.mCharacters);
    //    for (; ; ) {
    //        foreach (MapCharacter tCharacter in tProcessing) {
    //            //移動
    //            bool tRemainedDistance = MapCharacterMoveSystem.moveCharacter(tCharacter);
    //            //階層更新
    //            MapStratumMoveSystem.updateStratumLevel(tCharacter, aWorld);
    //            //trigger

    //            if (!tRemainedDistance) {
    //                //これ以上移動しない
    //                updateMaskOrder(tCharacter);
    //                //z座標更新
    //                float tHeight = MapStratumMoveSystem.getHeight(tCharacter);
    //                tCharacter.mImage.setUpHigh(tHeight);
    //                if (tCharacter.mMovingData.mCollidedSlope.Length == 0)
    //                    tCharacter.positionZ = MapZOrderCalculator.calculateOrderOfEntity(tCharacter.mMapPosition.x, tCharacter.mMapPosition.y, SlopeTilePhysicsAttribute.SlopeDirection.none, tCharacter.mStratumLevel.mLevel / 2);
    //                else
    //                    tCharacter.positionZ = MapZOrderCalculator.calculateOrderOfEntity(tCharacter.mMapPosition.x, tCharacter.mMapPosition.y, tCharacter.mMovingData.mCollidedSlope[0].mSlopeDirection, MapStratumMoveSystem.getHeight(tCharacter));

    //                continue;
    //            }
    //            //まだ移動できる
    //            tWaiting.Add(tCharacter);
    //        }
    //        if (tWaiting.Count == 0)
    //            break;
    //        tProcessing = tWaiting;
    //        tWaiting.Clear();
    //    }
    //}

    //public static void initZOrder(MapEntity aEntity) {
    //    SlopeTilePhysicsAttribute.SlopeDirection tDirection;
    //    float tHeight = MapStratumMoveSystem.getHeight(aEntity, out tDirection);
    //    aEntity.positionZ = MapZOrderCalculator.calculateOrderOfEntity(aEntity.mMapPosition.x, aEntity.mMapPosition.y, tDirection, tHeight);
    //}
    //public static void updateMaskOrder(MapCharacter aCharacter) {
        //foreach (SpriteRenderer tRenderer in aCharacter.mImage.GetComponentsInChildren<SpriteRenderer>()) {
        //}
    }
}
