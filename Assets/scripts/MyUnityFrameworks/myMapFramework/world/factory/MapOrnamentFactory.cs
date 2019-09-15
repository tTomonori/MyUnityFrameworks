using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapOrnamentFactory {
    static public MapOrnament create(MapFileData.Ornament aData) {
        MapOrnament tOrnament = MyBehaviour.createObjectFromResources<MapOrnament>(MyMap.mMapResourcesDirectory + "/ornament/" + aData.mPath);
        tOrnament.name = aData.mName;
        //名前が未設定の場合はornamentと名付ける
        if (tOrnament.name == "")
            tOrnament.name = "ornament";

        return tOrnament;
    }
}
