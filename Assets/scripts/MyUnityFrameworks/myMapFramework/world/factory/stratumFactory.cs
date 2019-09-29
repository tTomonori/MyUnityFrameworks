using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    ///<summary>指定階層のstratum生成してworldに追加</summary>
    static private void buildStratum(int aStratumLevel) {
        MapFileData.Stratum tStratumData = mData.mStratums[aStratumLevel];

        MyBehaviour tStratum = MyBehaviour.create<MyBehaviour>();
        if (aStratumLevel % 2 == 0)
            tStratum.name = "stratum" + (aStratumLevel / 2).ToString();
        else
            tStratum.name = "stratum" + (aStratumLevel / 2).ToString() + ".5";

        tStratum.changeLayer(MyMap.mStratumLayerNum[aStratumLevel / 2], false);
        tStratum.transform.SetParent(mWorld.mField.transform, false);
        mWorld.mStratums[aStratumLevel] = tStratum;

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

