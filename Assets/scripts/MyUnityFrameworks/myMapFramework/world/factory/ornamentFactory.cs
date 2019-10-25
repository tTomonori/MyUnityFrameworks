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
            MapKeyEventSpeaker tSpeaker = tOrnament.mAttribute.gameObject.AddComponent<MapKeyEventSpeaker>();
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
    static private void buildOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = createOrnament(aData);
        tOrnament.mFileData = aData;
        tOrnament.transform.SetParent(mWorld.mOrnamentContainer.transform, false);
        tOrnament.setMapPosition(new Vector2(aData.mX, aData.mY), aData.mHeight);
        tOrnament.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(tOrnament.mHeight)]);
        //画像イベント適用
        MapWorldUpdater.applyImageEvent(tOrnament);
    }
    //<summary>物を生成してworldに追加</summary>
    static private void buildOrnament(MapSaveFileData.SavedOrnament aData) {
        MapOrnament tOrnament = createOrnament(aData);
        tOrnament.mFileData = aData;
        tOrnament.transform.SetParent(mWorld.mOrnamentContainer.transform, false);
        tOrnament.setMapPosition(new Vector2(aData.mX, aData.mY), aData.mHeight);
        tOrnament.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(tOrnament.mHeight)]);
        //セーブデータから復元
        tOrnament.restore(aData.mSave);
        //画像イベント適用
        MapWorldUpdater.applyImageEvent(tOrnament);
    }
}

