using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    //<summary>指定階層のstratum生成してworldに追加</summary>
    static private void buildStratum(int aStratumLevel) {
        MapFileData.Stratum tStratumData = mData.mStratums[aStratumLevel];

        MapStratum tStratum = MyBehaviour.create<MapStratum>();
        mWorld.addStratum(tStratum);

        //マス生成
        List<List<int>> tFeildData = tStratumData.mFeild;
        int tXLength = tFeildData[0].Count;
        int tYLength = tFeildData.Count;
        for (int tY = 0; tY < tYLength; tY++) {
            for (int tX = 0; tX < tXLength; tX++) {
                buildCell(tX, tY, aStratumLevel);
            }
        }
    }

}

