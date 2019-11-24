using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlopeTilePhysicsAttribute : TilePhysicsAttribute {
    //<summary>引数の座標からこの傾斜に侵入できるか(侵入できるならtrue)</summary>
    public abstract bool canBeEntered(MapPosition aPosition);

    private Vector3 _ColliderSize = new Vector2(-1, -1);
    /// <summary>このbehaviourに付いているcolliderの最小外接矩形</summary>
    public Vector3 mColliderSize {
        get {
            if (_ColliderSize.x > 0) return _ColliderSize;
            _ColliderSize = mCollider.minimumCircumscribedCube();
            return _ColliderSize;
        }
    }
    public ColliderEditer.CubeEndPoint _ColliderEndPoint;
    /// <summary>このbehaviourに付いているcolliderの最小外接矩形の上下左右の座標(ローカル座標)</summary>
    public ColliderEditer.CubeEndPoint mColliderEndPoint {
        get {
            if (_ColliderEndPoint != null) return _ColliderEndPoint;
            _ColliderEndPoint = mCollider.minimumCircumscribedCubeEndPoint();
            return _ColliderEndPoint;
        }
    }
    /// <summary>このbehaviourに付いているcolliderの最小外接矩形の上下左右の座標(マップ座標)</summary>
    public ColliderEditer.CubeEndPoint mColliderEndPointMap {
        get {
            ColliderEditer.CubeEndPoint tPoint = mColliderEndPoint.copy();
            MapPosition tPosition = mBehaviour.mMapPosition;
            tPoint.top += tPosition.y;
            tPoint.bottom += tPosition.y;
            tPoint.back += tPosition.z;
            tPoint.front += tPosition.z;
            tPoint.left += tPosition.x;
            tPoint.right += tPosition.x;
            return tPoint;
        }
    }
}
