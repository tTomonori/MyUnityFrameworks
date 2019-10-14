using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRealPosition {
    public Vector3 vector;
    public float x {
        get { return vector.x; }
        set { vector.x = value; }
    }
    public float y {
        get { return vector.y; }
        set { vector.y = value; }
    }
    public float h {
        get { return vector.z; }
        set { vector.z = value; }
    }
    public MapRealPosition(Vector3 aVector) {
        vector = aVector;
    }
    public MapPosition toMapPosition() {
        return new MapPosition(new Vector3(vector.x, vector.y + vector.z, vector.z));
    }
    public RenderPosition toRenderPosition() {
        return new RenderPosition(new Vector3(vector.x, vector.y + vector.z, Mathf.Floor(vector.y * 100) / 100f + vector.z / 10000f));
    }
}

public class MapPosition {
    public Vector3 vector;
    public Vector2 vector2 {
        get { return vector.toVector2(); }
    }
    public float x {
        get { return vector.x; }
        set { vector.x = value; }
    }
    public float y {
        get { return vector.y; }
        set { vector.y = value; }
    }
    public float h {
        get { return vector.z; }
        set { vector.z = value; }
    }
    public MapPosition(Vector3 aVector) {
        vector = aVector;
    }
    public MapRealPosition toMapRealPosition() {
        return new MapRealPosition(new Vector3(vector.x, vector.y - vector.z, vector.z));
    }
    public RenderPosition toRenderPosition() {
        return new RenderPosition(new Vector3(vector.x, vector.y, Mathf.Floor((vector.y - vector.z) * 100) / 100f + vector.z / 10000f));
    }

    public static MapPosition operator +(MapPosition aPosition, Vector2 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x + aVector.x, aPosition.vector.y + aVector.y, aPosition.vector.z));
    }
    public static MapPosition operator -(MapPosition aPosition, Vector2 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x - aVector.x, aPosition.vector.y - aVector.y, aPosition.vector.z));
    }
    public static MapPosition operator +(MapPosition aPosition, Vector3 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x + aVector.x, aPosition.vector.y + aVector.y, aPosition.vector.z + aVector.z));
    }
    public static MapPosition operator -(MapPosition aPosition, Vector3 aVector) {
        return new MapPosition(new Vector3(aPosition.vector.x - aVector.x, aPosition.vector.y - aVector.y, aPosition.vector.z - aVector.z));
    }
}

public class RenderPosition {
    public Vector3 vector;
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
    public RenderPosition(Vector3 aVector) {
        vector = aVector;
    }
    static public float localYToRenderZ(float localY) {
        return Mathf.Floor(localY * 100) / 100f;
    }
    public RenderPosition addPileLevel(int aPileLevel) {
        vector.z -= aPileLevel / 1000f;
        return this;
    }
}