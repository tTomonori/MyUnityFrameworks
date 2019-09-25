using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collider2DEditer {
    /// <summary>最小外接矩形のサイズを返す</summary>
    public static Vector2 minimumCircumscribedRectangle(this EdgeCollider2D aCollider) {
        float up = aCollider.points[0].y;
        float down = aCollider.points[0].y;
        float left = aCollider.points[0].x;
        float right = aCollider.points[0].x;
        foreach (Vector2 tPoint in aCollider.points) {
            if (tPoint.x < left) left = tPoint.x;
            else if (right < tPoint.x) right = tPoint.x;
            if (up < tPoint.y) up = tPoint.y;
            else if (tPoint.y < down) down = tPoint.y;
        }
        return new Vector2(right - left, up - down);
    }
    /// <summary>最小外接矩形の上下左右の座標(ローカル座標)を返す</summary>
    public static RectangleEndPoint minimumCircumscribedRectangleEndPoint(this EdgeCollider2D aCollider) {
        RectangleEndPoint tPoints = new RectangleEndPoint();
        tPoints.up = aCollider.points[0].y;
        tPoints.down = aCollider.points[0].y;
        tPoints.left = aCollider.points[0].x;
        tPoints.right = aCollider.points[0].x;
        foreach (Vector2 tPoint in aCollider.points) {
            if (tPoint.x < tPoints.left) tPoints.left = tPoint.x;
            else if (tPoints.right < tPoint.x) tPoints.right = tPoint.x;
            if (tPoints.up < tPoint.y) tPoints.up = tPoint.y;
            else if (tPoint.y < tPoints.down) tPoints.down = tPoint.y;
        }
        return tPoints;
    }
    /// <summary>上下左右の四つの値</summary>
    public class RectangleEndPoint {
        public float up;
        public float down;
        public float left;
        public float right;
    }
}