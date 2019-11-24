using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    //<summary>物生成</summary>
    static public MapOrnament createOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = MyBehaviour.createObjectFromResources<MapOrnament>(MyMap.mMapResourcesDirectory + "/ornament/" + aData.mPath);

        tOrnament.mName = aData.mName;
        tOrnament.name = "ornament:" + aData.mName;

        //speaker
        if (aData.mIsSpeaker) {
            MapKeyEventSpeaker tSpeaker = tOrnament.mEntityPhysicsBehaviour.mAttriubteCollider.gameObject.AddComponent<MapKeyEventSpeaker>();
            tSpeaker.mBehaviour = tOrnament;
            tSpeaker.mSpeakDefault = aData.mSpeakDefault;
            tSpeaker.mSpeakFromUp = aData.mSpeakFromUp;
            tSpeaker.mSpeakFromDown = aData.mSpeakFromDown;
            tSpeaker.mSpeakFromLeft = aData.mSpeakFromLeft;
            tSpeaker.mSpeakFromRight = aData.mSpeakFromRight;
        }

        return tOrnament;
    }
    //<summary>物を生成してworldに追加</summary>
    static private MapOrnament buildOrnament(MapFileData.Ornament aData) {
        //生成フラグ確認
        if (!flagCreate(aData)) return null;

        MapOrnament tOrnament = createOrnament(aData);
        tOrnament.mFileData = aData;
        tOrnament.transform.SetParent(mWorld.mOrnamentContainer.transform, false);
        tOrnament.mMapPosition = new MapPosition(aData.mPosition);
        tOrnament.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(aData.mY)]);
        //変数適用
        if (aData.mArg != null)
            tOrnament.setArg(aData.mArg);
        //画像イベント適用
        MapWorldUpdater.applyImageEvent(tOrnament);
        return tOrnament;
    }
}

