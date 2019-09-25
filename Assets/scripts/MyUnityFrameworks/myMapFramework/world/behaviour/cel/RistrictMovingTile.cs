using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RistrictMovingTile : MyBehaviour {
    private Collider2D _Collider;
    public Collider2D mCollider {
        get {
            if (_Collider == null)
                _Collider = GetComponent<Collider2D>();
            return _Collider;
        }
    }
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
    /// <summary>このbehaviourに付いているcolliderの最小外接矩形の上下左右の座標</summary>
    public Collider2DEditer.RectangleEndPoint mColliderEndPoint {
        get {
            if (_ColliderEndPoint != null) return _ColliderEndPoint;
            if (mCollider is BoxCollider2D) {
                BoxCollider2D tBox = (BoxCollider2D)mCollider;
                _ColliderEndPoint = new Collider2DEditer.RectangleEndPoint();
                _ColliderEndPoint.up = tBox.size.y / 2 + tBox.offset.y;
                _ColliderEndPoint.down = -tBox.size.y / 2 + tBox.offset.y;
                _ColliderEndPoint.left = -tBox.size.x / 2 + tBox.offset.y;
                _ColliderEndPoint.right = tBox.size.x / 2 + tBox.offset.y;
                return _ColliderEndPoint;
            }
            if (mCollider is EdgeCollider2D) {
                _ColliderEndPoint = ((EdgeCollider2D)mCollider).minimumCircumscribedRectangleEndPoint();
                return _ColliderEndPoint;
            }
            throw new System.Exception("RistrictMovingTile : colliderの端の座標計算が未定義「" + mCollider.GetType().ToString() + "」");
        }
    }
    public struct RistrictMovingData {
        /// <summary>このbehaviour内部での移動ベクトル</summary>
        public Vector2[] mInternalVector;
        /// <summary>このbehaviourを抜けてからの移動ベクトル</summary>
        public Vector2 mOutsideVector;
    }
    /// <summary>
    /// このbehaviourに入ってからの移動ベクトルを返す
    /// </summary>
    /// <returns>このbehaviourに入ってからの移動ベクトルデータ</returns>
    /// <param name="aStartPoint">移動開始する座標(このbehaviourからの相対座標)</param>
    /// <param name="aMoveVector">このbehaviourに侵入後の移動方向</param>
    public abstract RistrictMovingData getMovingData(Vector2 aStartPoint, Vector2 aMoveVector);
}
