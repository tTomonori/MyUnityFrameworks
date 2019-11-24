using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    ///<summary>指定階層のstratum生成してworldに追加</summary>
    static private void buildStratum(int aStratumLevel) {
        MapFileData.Stratum tStratumData = mData.mStratums[aStratumLevel];

        MapStratum tStratum = MyBehaviour.create<MapStratum>();
        tStratum.name = "stratum" + aStratumLevel.ToString();

        tStratum.changeLayer(MyMap.mStratumLayerNum[aStratumLevel], false);
        tStratum.transform.SetParent(mWorld.mField.transform, false);
        mWorld.mStratums[aStratumLevel] = tStratum;

        //マス生成
        List<List<int>> tFeildData = tStratumData.mFeild;
        int tXLength = tFeildData[0].Count;
        int tZLength = tFeildData.Count;
        for (int tZ = 0; tZ < tZLength; tZ++) {
            for (int tX = 0; tX < tXLength; tX++) {
                buildCell(tX, aStratumLevel, tZ);
            }
        }
    }
}

