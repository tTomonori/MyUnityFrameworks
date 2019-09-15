using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapZOrderCalculator {
    //<summary>階層のzPositionを計算</summary>
    public static float calculateOrderOfStratum(int aStratumLevel) {
        return -aStratumLevel / 2 * 0.9f - ((aStratumLevel % 2) * 0.001f);

    }
    //<summary>tileのzPositionを計算</summary>
    public static float calculateOrderOfTile(int aYoungerBrotherNum) {
        return 0.0001f * aYoungerBrotherNum;
    }
    //<summary>cellのzPositionを計算</summary>
    public static float calculateOrderOfCell(float aX,float aY) {
        return aY + 0.5f;
    }
    //<summary>entityのzPositionを計算(aStratumLevelは偶数)</summary>
    public static float calculateOrderOfEntity(float aX,float aY,int aStratumLevel) {
        //平地にいる
        return aY;
    }
    //<summary>entityのzPositionを計算(aStratumLevelは奇数)</summary>
    public static float calculateOrderOfEntity(float aX, float aY, int aStratumLevel, MapSlopeTile.SlopeDirection aSlopeDirection) {
        //坂にいる
        float tHigh = 0;
        switch (aSlopeDirection) {
            case MapSlopeTile.SlopeDirection.upHigh:
                tHigh = (aY + 0.5f) % 1f;
                break;
            case MapSlopeTile.SlopeDirection.downHigh:
                tHigh = -(aY + 0.5f) % 1f;
                break;
            case MapSlopeTile.SlopeDirection.leftHigh:
                tHigh = -(aX + 0.5f) % 1f;
                break;
            case MapSlopeTile.SlopeDirection.rightHigh:
                tHigh = (aX + 0.5f) % 1f;
                break;
        }
        return aY + (0.001f + 0.9f * tHigh);
    }
}
