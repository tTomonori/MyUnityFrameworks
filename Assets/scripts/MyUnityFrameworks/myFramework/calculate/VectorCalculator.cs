using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorCalculator{
    /// <summary>
    /// ベクトルを成分分解する
    /// </summary>
    /// <returns>成分方向ベクトルに垂直な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector2 disassemble(Vector2 aVector,Vector2 aComponent){
        //成分ベクトルに直角
        Vector2 tRightAngleVector = new Vector2(-aComponent.y, aComponent.x);
        float k = (aVector.x * aComponent.y - aVector.y * aComponent.x) / (tRightAngleVector.x * aComponent.y - tRightAngleVector.y * aComponent.x);
        //成分ベクトルに直角な方向の成分
        Vector2 tARightAngleVector = k * tRightAngleVector;
        return tARightAngleVector;
    }
    /// <summary>
    /// ランダムな方向の単位ベクトルを返す
    /// </summary>
    static public Vector2 randomVector(){
        Vector2 vector = new Vector2(0, 1);
        return Quaternion.Euler(0, 0, Random.Range(0,359)) * vector;
    }
}
