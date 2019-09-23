using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapZOrderCalculator {
    //<summary>階層のzPositionを計算</summary>
    public static float calculateOrderOfStratum(int aStratumLevel) {
        return -100 * (aStratumLevel / 2) + ((aStratumLevel % 2 == 0) ? 0.001f : 0);
        //return -0.001f * aStratumLevel;
        //return -aStratumLevel;
        //return -aStratumLevel / 2 * 0.9f - ((aStratumLevel % 2) * 0.001f);
    }
    //<summary>tileのzPositionを計算</summary>
    public static float calculateOrderOfTile(int aYoungerBrotherNum) {
        return 0.0001f * aYoungerBrotherNum;
    }
    //<summary>cellのzPositionを計算</summary>
    public static float calculateOrderOfCell(float aX, float aY, int aStratumLevel) {
        return aY + 1f;
        //return aY + 0.5f;
    }
    //<summary>entityのzPositionを計算</summary>
    public static float calculateOrderOfEntity(float aX, float aY, SlopeTilePhysicsAttribute.SlopeDirection aSlopeDirection, float aHeight) {
        return aY + 0.5f;
        float tY = aY + 0.5f;
        float tYIntp = Mathf.Floor(tY);
        float tYDecp = tY - tYIntp;
        float tHIntp = Mathf.Floor(aHeight);
        float tHDecp = aHeight - tHIntp;
        switch (aSlopeDirection) {
            case SlopeTilePhysicsAttribute.SlopeDirection.none:
                return tYIntp + 0.01f + tYDecp * 0.99f - tHIntp;
            case SlopeTilePhysicsAttribute.SlopeDirection.upHigh:
                return tYIntp - tHIntp + aHeight * 0.001f;
            case SlopeTilePhysicsAttribute.SlopeDirection.downHigh:
                return tYIntp + 0.01f + tYDecp * 0.09f - tHIntp + aHeight * 0.001f;
            case SlopeTilePhysicsAttribute.SlopeDirection.leftHigh:
                return tYIntp + 0.01f + tYDecp * 0.09f - tHIntp + aHeight * 0.001f;
            case SlopeTilePhysicsAttribute.SlopeDirection.rightHigh:
                return tYIntp + 0.01f + tYDecp * 0.09f - tHIntp + aHeight * 0.001f;
            default:
                throw new System.Exception("MapZOrderCalculator : 不正な坂の方向");
        }
    }
}
