using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapZOrderCalculator {
    /// <summary>階層のorderInLayerを計算</summary>
    public static int calculateOrderOfStratum(int aStratumLevel) {
        return aStratumLevel;
    }
    ///<summary>平面cellのzPositionを計算</summary>
    public static float calculateOrderOfLieCell(float aX, float aY, int aStratumLevel) {
        return aY + 1;
    }
    ///<summary>直立cellのorderInLayerとpositionZを計算</summary>
    public static int calculateOrderOfStandCells(float aY, int aStratumLevel, out float oPositionZ) {
        oPositionZ = aStratumLevel * 100;
        return Mathf.FloorToInt(-(aY - aStratumLevel) * 100);
    }
    ///<summary>entityのorderInLayerとpositionZを計算</summary>
    public static int calculateOrderOfEntity(float aX, float aY, float aHeight, out float oPositionZ) {
        oPositionZ = aHeight * 100;
        return Mathf.FloorToInt(-(aY - aHeight) * 100);
    }

    /// <summary>
    /// cellに含まれるentityのorderInLayerを計算
    /// </summary>
    /// <returns>orderInLayer</returns>
    /// <param name="aX">x座標</param>
    /// <param name="aY">y座標</param>
    /// <param name="aStratumLevel">階層レベル</param>
    /// <param name="aPositionZ">cellに対する相対Z座標</param>
    //public static int calculateOrderOfEntityInTile(float aX, float aY, int aStratumLevel, float aPositionZ, out float oPositionZ) {
    //    oPositionZ = aStratumLevel * 100;
    //    return Mathf.FloorToInt(-(aY + aPositionZ - aStratumLevel) * 100);
    //}
}
