using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    //<summary>物生成</summary>
    static public MapOrnament createOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = MyBehaviour.createObjectFromResources<MapOrnament>(MyMap.mMapResourcesDirectory + "/ornament/" + aData.mPath);

        return tOrnament;
    }
    //<summary>物を生成してworldに追加</summary>
    static private void buildOrnament(MapFileData.Ornament aData) {
        MapOrnament tOrnament = createOrnament(aData);
        mWorld.addOrnament(tOrnament, aData.mName, aData.mX, aData.mY, aData.mStratum);
    }
}

