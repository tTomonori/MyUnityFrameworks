using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPosition {
    public Vector3 vector;
    /// <summary>同座標で重なった時の描画順(値が大きいほど上に描画)</summary>
    public float pileLevel;
    /// <summary>平面方向ベクトル</summary>
    public Vector2 vector2 {
        get { return new Vector2(vector.x, vector.z); }
    }
    /// <summary>描画座標</summary>
    public Vector3 renderPosition {
        get { return new Vector3(vector.x, vector.y + vector.z, vector.z - 0.001f * pileLevel); }
    }
    /// <summary>物理座標</summary>
    public Vector3 physicsPosition {
        get { return vector; }
    }
    public float x {
        get { return vector.x; }
        set { vector.x = value; }
    }
    public float y {
        get { return vector.y; }
        set { vector.y = value; }
    }
    public float z {
        get { return vector.z; }
        set { vector.z = value; }
    }
    public MapPosition(Vector3 aVector, float aPileLevel = 0) {
        vector = aVector;
        pileLevel = aPileLevel;
    }
    /// <summary>自身を複製して返す</summary>
    public MapPosition copy() {
        return new MapPosition(vector, pileLevel);
    }

    public static MapPosition operator +(MapPosition aPosition, Vector2 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x + aVector.x, aPosition.vector.y, aPosition.vector.z + aVector.y));
    }
    public static MapPosition operator -(MapPosition aPosition, Vector2 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x - aVector.x, aPosition.vector.y, aPosition.vector.z - aVector.y));
    }
    public static MapPosition operator +(MapPosition aPosition, Vector3 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x + aVector.x, aPosition.vector.y + aVector.y, aPosition.vector.z + aVector.z));
    }
    public static MapPosition operator -(MapPosition aPosition, Vector3 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x - aVector.x, aPosition.vector.y - aVector.y, aPosition.vector.z - aVector.z));
    }
}