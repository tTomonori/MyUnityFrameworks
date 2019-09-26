using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapZOrderCalculator {
    ///<summary>平面cellのorderInLayerとzPositionを計算</summary>
    public static int calculateOrderOfLieCell(float aX, float aY, int aStratumLevel, out float oPositionZ) {
        oPositionZ = aY - aStratumLevel * 0.01f;
        return Mathf.FloorToInt(-(aY + 0.5f - aStratumLevel / 2) * 100f);
    }
    ///<summary>直立cellのorderInLayerとzPositionを計算</summary>
    public static int calculateOrderOfStandCell(float aX, float aY, int aStratumLevel, out float oPositionZ) {
        oPositionZ = aY;
        return Mathf.FloorToInt(-(aY - 0.5f - aStratumLevel / 2) * 100f);
    }
    ///<summary>entityのorderInLayerとpositionZを計算</summary>
    public static int calculateOrderOfEntity(float aX, float aY, float aHeight, out float oPositionZ) {
        oPositionZ = aY;
        return Mathf.FloorToInt(-(aY - aHeight) * 100f);
    }
}
