using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapBehaviour : MyBehaviour {
    /// <summary>名前</summary>
    public string mName { get; set; }
    /// <summary>描画処理で動作するbehaviour</summary>
    public virtual MapRenderBehaviour mRenderBehaviour { get; set; }
    /// <summary>物理処理で動作するbehaviour</summary>
    public virtual MapPhysicsBehaviour mPhysicsBehaviour { get; set; }

    private MapPosition _MapPosition;
    /// <summary>マップ上での座標</summary>
    public MapPosition mMapPosition {
        get { return _MapPosition.copy(); }
        set {
            _MapPosition = value.copy();
            if (mRenderBehaviour != null)
                mRenderBehaviour.position = _MapPosition.renderPosition;
            if (mPhysicsBehaviour != null)
                mPhysicsBehaviour.position = _MapPosition.physicsPosition;
        }
    }

    /// <summary>現在いる座標のcellの座標</summary>
    public Vector3Int mFootCellPosition {
        get {
            Vector3 tPosition = _MapPosition.vector;
            return new Vector3Int(Mathf.FloorToInt(tPosition.x + 0.5f), Mathf.FloorToInt(tPosition.y), Mathf.FloorToInt(tPosition.z + 0.5f));
        }
    }

    ///<summary>MapWorld生成終了直後に呼ばれる(world生成後に追加した場合はこのbehaviourが配置された直後)</summary>
    public virtual void placed() { }
}

public class InspectorOfMapBehaviour : Editor {
    public MapBehaviour mBehaviour { get => target as MapBehaviour; }
    /// <summary>子要素を新規objectの子要素としてまとめる</summary>
    public void evacuate() {
        if (mBehaviour.transform.childCount == 0) return;
        GameObject tObject = new GameObject();
        tObject.name = "temp";
        tObject.transform.SetParent(mBehaviour.transform, false);
        Transform[] tChildren = mBehaviour.transform.GetComponentsInChildren<Transform>();
        foreach (Transform tChild in tChildren) {
            if (tChild == mBehaviour.transform) continue;
            if (tChild.transform.parent != mBehaviour.transform) continue;
            if (tChild == tObject.transform) continue;
            tChild.transform.SetParent(tObject.transform, false);
        }
    }
}