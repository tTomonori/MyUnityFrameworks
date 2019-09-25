using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    ///<summary>指定階層のstratum生成してworldに追加</summary>
    static private void buildStratum(int aStratumLevel) {
        if (aStratumLevel % 2 == 0)
            buildLieStratum(aStratumLevel);
        else
            buildStandStratum(aStratumLevel);
    }
    /// <summary>偶数階層を生成</summary>
    static private void buildLieStratum(int aStratumLevel) {
        MapFileData.Stratum tStratumData = mData.mStratums[aStratumLevel];

        MyBehaviour tStratum = MyBehaviour.create<MyBehaviour>();
        tStratum.name = "stratum" + aStratumLevel.ToString();
        tStratum.gameObject.AddComponent<SortingGroup>().sortingOrder = MapZOrderCalculator.calculateOrderOfStratum(aStratumLevel);
        tStratum.changeLayer(MyMap.mStratumLayerNum[aStratumLevel], false);

        //マス生成
        List<List<int>> tFeildData = tStratumData.mFeild;
        int tXLength = tFeildData[0].Count;
        int tYLength = tFeildData.Count;
        for (int tY = 0; tY < tYLength; tY++) {
            for (int tX = 0; tX < tXLength; tX++) {
                buildLieCell(tX, tY, aStratumLevel);
            }
        }
    }
    /// <summary>奇数階層を生成</summary>
    static private void buildStandStratum(int aStratumLevel) {
        MapFileData.Stratum tStratumData = mData.mStratums[aStratumLevel];

        MyBehaviour tStratum = MyBehaviour.create<MyBehaviour>();
        tStratum.name = "stratum" + aStratumLevel.ToString();
        tStratum.gameObject.AddComponent<SortingGroup>().sortingOrder = MapZOrderCalculator.calculateOrderOfStratum(aStratumLevel);
        tStratum.changeLayer(MyMap.mStratumLayerNum[aStratumLevel], false);

        //マス生成
        List<List<int>> tFeildData = tStratumData.mFeild;
        int tXLength = tFeildData[0].Count;
        int tYLength = tFeildData.Count;
        for (int tY = 0; tY < tYLength; tY++) {
            for (int tX = 0; tX < tXLength; tX++) {
                buildStandCell(tX, tY, aStratumLevel);
            }
        }
    }
}

