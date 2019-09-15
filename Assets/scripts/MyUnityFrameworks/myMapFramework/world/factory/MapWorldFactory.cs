using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    //生成に使っているマップデータ
    static private MapFileData mData;
    //生成中のMapWorld
    static private MapWorld mWorld;
    //<summary>ワールドを作成</summary>
    static public MapWorld create(MapFileData aData) {
        //マップデータを記憶
        mData = aData;
        mWorld = MyBehaviour.create<MapWorld>();
        //stratum生成
        for (int i = 0; i < aData.mStratums.Count; i++) {
            buildStratum(i);
        }
        //ornament生成
        foreach (MapFileData.Ornament tOrnamentData in mData.mOrnaments) {
            buildOrnament(tOrnamentData);
        }
        //charactor(npc)生成
        foreach(MapFileData.Npc tNpcData in mData.mNpc) {
            buildCharacter(tNpcData);
        }

        MapWorld tCreatedWorld = mWorld;
        mWorld = null;
        mData = null;
        return tCreatedWorld;
    }
}
