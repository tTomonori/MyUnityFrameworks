using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    //<summary>物生成</summary>
    static public MapOrnament createOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = MyBehaviour.createObjectFromResources<MapOrnament>(MyMap.mMapResourcesDirectory + "/ornament/" + aData.mPath);

        tOrnament.mName = aData.mName;
        tOrnament.name = "ornament:" + aData.mName;

        return tOrnament;
    }
    //<summary>物を生成してworldに追加</summary>
    static private void buildOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = createOrnament(aData);
        tOrnament.transform.SetParent(mWorld.mOrnamentContainer.transform, false);
        tOrnament.setMapPosition(new Vector2(aData.mX, aData.mY), aData.mHeight);
        tOrnament.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(tOrnament.mHeight)]);
        //画像イベント適用
        MapWorldUpdater.applyImageEvent(tOrnament);
    }
}

