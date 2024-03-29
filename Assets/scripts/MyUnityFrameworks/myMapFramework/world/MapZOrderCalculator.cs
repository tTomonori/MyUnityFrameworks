﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapZOrderCalculator {
    ///<summary>平面cellのorderInLayerとzPositionを計算</summary>
    public static int calculateOrderOfLieCell(float aX, float aY, int aHeight, float aScaffoldLevel, out float oPositionZ) {
        oPositionZ = aY + aHeight + 0.5f;
        return Mathf.FloorToInt(-(aY - aHeight + 0.5f)) * 100 + Mathf.FloorToInt(aScaffoldLevel);
    }
    ///<summary>直立cellのorderInLayerとzPositionを計算</summary>
    public static int calculateOrderOfStandCell(float aX, float aY, int aHeight, float aScaffoldLevel, out float oPositionZ) {
        oPositionZ = aY - aHeight - 0.5f;
        return Mathf.FloorToInt(-(aY - aHeight + 0.5f)) * 100 + Mathf.FloorToInt(99);
    }
    ///<summary>entityのorderInLayerとpositionZを計算</summary>
    public static int calculateOrderOfEntity(float aX, float aY, float aHeight, float aScaffoldHeight, out float oPositionZ) {
        oPositionZ = aY - aHeight;
        return Mathf.FloorToInt(-(aY - aHeight + 0.5f)) * 100 + Mathf.FloorToInt(aScaffoldHeight);
    }
}
