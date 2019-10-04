using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorCalculator {
    /// <summary>
    /// ベクトルを成分分解する(成分方向ベクトルに垂直な成分を返す)
    /// </summary>
    /// <returns>成分方向ベクトルに垂直な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector2 disassembleOrthogonal(this Vector2 aVector, Vector2 aComponent) {
        //成分ベクトルに直角
        Vector2 tRightAngleVector = new Vector2(-aComponent.y, aComponent.x);
        float k = (aVector.x * aComponent.y - aVector.y * aComponent.x) / (tRightAngleVector.x * aComponent.y - tRightAngleVector.y * aComponent.x);
        //成分ベクトルに直角な方向の成分
        Vector2 tARightAngleVector = k * tRightAngleVector;
        return tARightAngleVector;
    }
    /// <summary>
    /// ベクトルを成分分解する(成分方向ベクトルに平行な成分を返す)
    /// </summary>
    /// <returns>成分方向ベクトルに平行な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector2 disassembleParallel(this Vector2 aVector, Vector2 aComponent) {
        //成分ベクトルに直角
        Vector2 tRightAngleVector = new Vector2(-aComponent.y, aComponent.x);
        float k = (aVector.x * tRightAngleVector.y - aVector.y * tRightAngleVector.x) / (aComponent.x * tRightAngleVector.y - aComponent.y * tRightAngleVector.x);
        //成分ベクトルに平行な方向の成分
        Vector2 tAParallelAngleVector = k * aComponent;
        return tAParallelAngleVector;
    }
    /// <summary>
    /// ランダムな方向の単位ベクトルを返す
    /// </summary>
    static public Vector2 randomVector() {
        Vector2 vector = new Vector2(0, 1);
        return Quaternion.Euler(0, 0, Random.Range(0, 359)) * vector;
    }


    /// <summary>
    /// 直行判定
    /// </summary>
    /// <returns>垂直ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isOrthogonal(this Vector2 v1, Vector2 v2) {
        return Equals(Vector2.Dot(v1, v2), 0.0f);
    }
    /// <summary>
    /// 直行判定(誤差を考慮する)
    /// </summary>
    /// <returns>垂直ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isOrthogonalConsiderdError(this Vector2 v1, Vector2 v2) {
        float a = Vector2.Dot(v1, v2);
        return (-0.01f < a) && (a < 0.01f);
    }
    /// <summary>
    /// 平行判定
    /// </summary>
    /// <returns>平行ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isParallel(this Vector2 v1, Vector2 v2) {
        return Equals(v1.x * v2.y - v1.y * v2.x, 0.0f);
    }
    /// <summary>
    /// 平行判定(誤差を考慮する)
    /// </summary>
    /// <returns>平行ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isParallelConsiderdError(this Vector2 v1, Vector2 v2) {
        float a = v1.x * v2.y - v1.y * v2.x;
        return (-0.01f < a) && (a < 0.01f);
    }
    /// <summary>
    /// 2つのベクトルのなす角
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns>2つのベクトルのなす角(単位は度)</returns>
    public static float corner(this Vector2 v1, Vector2 v2) {
        float tRad = Mathf.Atan2(v2.x * v1.y - v1.x * v2.y, v1.x * v2.x + v1.y * v2.y);
        tRad = Mathf.Abs(tRad);
        return 180f * tRad / Mathf.PI;
    }
    /// <summary>
    /// Matchs the length.
    /// </summary>
    /// <returns>引数長の同じ向きのベクトル</returns>
    /// <param name="aVector">向き</param>
    /// <param name="aLength">長さ</param>
    public static Vector2 matchLength(this Vector2 aVector, float aLength) {
        return aVector * (aLength / aVector.magnitude);
    }
}
