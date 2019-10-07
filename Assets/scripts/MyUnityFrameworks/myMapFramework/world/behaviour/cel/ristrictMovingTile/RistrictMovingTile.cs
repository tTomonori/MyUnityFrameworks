using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RistrictMovingTile : MyBehaviour {
    /// <summary>制限属性を持たせたcell</summary>
    [SerializeField] public MapCell mCell;
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
    /// <summary>このbehaviourに付いているcolliderの最小外接矩形の上下左右の座標(ローカル座標)</summary>
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
            if (mCollider is PolygonCollider2D) {
                _ColliderEndPoint = ((PolygonCollider2D)mCollider).minimumCircumscribedRectangleEndPoint();
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
        /// <summary>このbehaviour内部での最後の移動方向</summary>
        public Vector2 mLastInternalDirection;
    }
    /// <summary>
    /// このbehaviourに入ってからの移動ベクトルを返す
    /// </summary>
    /// <returns>このbehaviourに入ってからの移動ベクトルデータ</returns>
    /// <param name="aStartPoint">移動開始する座標(このbehaviourからの相対座標)</param>
    /// <param name="aMoveVector">このbehaviourに侵入後の移動方向</param>
    public abstract RistrictMovingData getMovingData(Vector2 aStartPoint, Vector2 aMoveVector);

    /// <summary>
    /// このtile内部でどれだけ移動できるか(移動ベクトルに対する割合を返す)
    /// </summary>
    /// <returns>このtile内部で移動できる移動ベクトルの、引数の移動ベクトルに対する割合(0~1)</returns>
    /// <param name="aStartPoint">移動開始地点(相対座標)</param>
    /// <param name="aMovingVector">移動ベクトル</param>
    protected float calculateRateOfMovingInSelf(Vector2 aStartPoint, Vector2 aMovingVector) {
        Collider2DEditer.RectangleEndPoint tEnd = mColliderEndPoint;
        //tile外部までの距離
        float tHDistance;
        float tVDistance;
        if (aMovingVector.x < 0) {
            tHDistance = aStartPoint.x - tEnd.left;
        } else if (aMovingVector.x > 0) {
            tHDistance = tEnd.right - aStartPoint.x;
        } else {
            tHDistance = float.PositiveInfinity;
        }
        if (aMovingVector.y < 0) {
            tVDistance = aStartPoint.y - tEnd.down;
        } else if (aMovingVector.y > 0) {
            tVDistance = tEnd.up - aStartPoint.y;
        } else {
            tVDistance = float.PositiveInfinity;
        }

        //移動ベクトルに対する外部までの移動ベクトルの割合
        float tHRate = tHDistance / Mathf.Abs(aMovingVector.x);
        float tVRate = tVDistance / Mathf.Abs(aMovingVector.y);

        return Mathf.Min(Mathf.Min(tHRate, tVRate), 1f);
    }
}
