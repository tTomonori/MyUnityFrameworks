using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlopeTilePhysicsAttribute : TilePhysicsAttribute {
    //<summary>引数の座標からこの傾斜に侵入できるか</summary>
    public abstract bool canBeEntered(Vector2 aPosition, float aHeight);
    /// <summary>
    /// 指定座標の高さ(world内部での絶対値)を返す(傾斜外にいるならこの傾斜を通過して行った場合の高さを返す)
    /// </summary>
    /// <returns>高さ</returns>
    /// <param name="aPosition">高さを調べる座標</param>
    /// <param name="oIsIn">この傾斜の上にいるならtrue</param>
    public abstract float getPointHeight(Vector2 aPosition, out bool oIsIn);

    private Vector2 _ColliderSize = new Vector2(-1, -1);
    /// <summary>このbehaviourに付いているcolliderの最小外接矩形</summary>
    public Vector2 mColliderSize {
        get {
            if (_ColliderSize.x > 0) return _ColliderSize;
            if (mCollider is BoxCollider2D) {
                _ColliderSize = ((BoxCollider2D)mCollider).size;
                return _ColliderSize;
            }
            if (mCollider is EdgeCollider2D) {
                _ColliderSize = ((EdgeCollider2D)mCollider).minimumCircumscribedRectangle();
                return _ColliderSize;
            }
            throw new System.Exception("RistrictMovingTile : colliderのサイズ計算が未定義「" + mCollider.GetType().ToString() + "」");
        }
    }
    public Collider2DEditer.RectangleEndPoint _ColliderEndPoint;
    /// <summary>このbehaviourに付いているcolliderの最小外接矩形の上下左右の座標(絶対座標)</summary>
    public Collider2DEditer.RectangleEndPoint mColliderEndPoint {
        get {
            if (_ColliderEndPoint != null) return _ColliderEndPoint;
            Vector2 tPosition = this.position2D;
            if (mCollider is BoxCollider2D) {
                BoxCollider2D tBox = (BoxCollider2D)mCollider;
                _ColliderEndPoint = new Collider2DEditer.RectangleEndPoint();
                _ColliderEndPoint.up = tBox.size.y / 2 + tBox.offset.y + tPosition.y;
                _ColliderEndPoint.down = -tBox.size.y / 2 + tBox.offset.y + tPosition.y;
                _ColliderEndPoint.left = -tBox.size.x / 2 + tBox.offset.x + tPosition.x;
                _ColliderEndPoint.right = tBox.size.x / 2 + tBox.offset.x + tPosition.x;
                return _ColliderEndPoint;
            }
            if (mCollider is EdgeCollider2D) {
                _ColliderEndPoint = ((EdgeCollider2D)mCollider).minimumCircumscribedRectangleEndPoint();
                _ColliderEndPoint.up += tPosition.y;
                _ColliderEndPoint.down += tPosition.y;
                _ColliderEndPoint.left += tPosition.x;
                _ColliderEndPoint.right += tPosition.x;
                return _ColliderEndPoint;
            }
            throw new System.Exception("RistrictMovingTile : colliderの端の座標計算が未定義「" + mCollider.GetType().ToString() + "」");
        }
    }
}
