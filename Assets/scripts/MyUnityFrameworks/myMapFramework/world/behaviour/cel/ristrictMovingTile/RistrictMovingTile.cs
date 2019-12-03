using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RistrictMovingTile : MyBehaviour {
    /// <summary>制限属性を持たせたcell</summary>
    [SerializeField] public MapTile mTile;
    private Collider mCollider { get; set; }
    private void Awake() {
        mCollider = GetComponent<Collider>();
    }
    private Vector3 _ColliderSize = new Vector3(-1, -1, -1);
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
    public struct RistrictMovingData {
        /// <summary>このbehaviour内部での移動ベクトル</summary>
        public Vector3[] mInternalVector;
        /// <summary>このbehaviourを抜けてからの移動ベクトル</summary>
        public Vector3 mOutsideVector;
        /// <summary>このbehaviour内部での最後の移動方向</summary>
        public Vector3 mLastInternalDirection;
    }
    /// <summary>
    /// このbehaviourに入ってからの移動ベクトルを返す
    /// </summary>
    /// <returns>このbehaviourに入ってからの移動ベクトルデータ</returns>
    /// <param name="aStartPoint">移動開始する座標(このbehaviourからの相対座標)</param>
    /// <param name="aMoveVector">このbehaviourに侵入後の移動方向</param>
    public abstract RistrictMovingData getMovingData(Vector3 aStartPoint, Vector3 aMoveVector);

    /// <summary>
    /// このtile内部でどれだけ移動できるか(移動ベクトルに対する割合を返す)(tileが矩形であること前提)
    /// </summary>
    /// <returns>このtile内部で移動できる移動ベクトルの、引数の移動ベクトルに対する割合(0~1)</returns>
    /// <param name="aStartPoint">移動開始地点(相対座標)</param>
    /// <param name="aMovingVector">移動ベクトル</param>
    protected float calculateRateOfMovingInSelf(Vector3 aStartPoint, Vector3 aMovingVector) {
        ColliderEditer.CubeEndPoint tEnd = mColliderEndPoint;
        //tile外部までの距離
        float tHDistance;
        float tVDistance;
        float tSDistance;
        if (aMovingVector.x < 0) {
            tHDistance = aStartPoint.x - tEnd.left;
        } else if (aMovingVector.x > 0) {
            tHDistance = tEnd.right - aStartPoint.x;
        } else {
            tHDistance = float.PositiveInfinity;
        }
        if (aMovingVector.z < 0) {
            tVDistance = aStartPoint.z - tEnd.front;
        } else if (aMovingVector.z > 0) {
            tVDistance = tEnd.back - aStartPoint.z;
        } else {
            tVDistance = float.PositiveInfinity;
        }
        if (aMovingVector.y < 0) {
            tSDistance = aStartPoint.y - tEnd.bottom;
        } else if (aMovingVector.y > 0) {
            tSDistance = tEnd.top - aStartPoint.y;
        } else {
            tSDistance = float.PositiveInfinity;
        }

        //移動ベクトルに対する外部までの移動ベクトルの割合
        float tHRate = tHDistance / Mathf.Abs(aMovingVector.x);
        float tVRate = tVDistance / Mathf.Abs(aMovingVector.z);
        float tSRate = tSDistance / Mathf.Abs(aMovingVector.y);

        return Mathf.Min(Mathf.Min(tHRate, Mathf.Min(tVRate, tSRate)), 1f);
    }
}
