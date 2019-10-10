using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    /// <summary>自分より1以上高い位置にいる相手とも衝突する</summary>
    public bool mCollideUpperStratum = false;
    /// <summary>自分より1以上低い位置にいる相手とも衝突する</summary>
    public bool mCollideLowerStratum = false;
    ///<summary>マップ上での座標</summary>
    public Vector2 mMapPosition {
        get { return position2D; }
        set { position2D = value; }
    }
    protected float _Height;
    /// <summary>マップ上での高さ</summary>
    public float mHeight {
        get { return _Height; }
        set { _Height = value; }
    }
    public Vector3 mMapPosition3 {
        get { return new Vector3(positionX, positionY, _Height); }
        set {
         mMapPosition = new Vector2(value.x, value.y);
            mHeight = value.z;
        }
    }
    /// <summary>足場の高さ</summary>
    public float mScaffoldLevel { get; set; }
    /// <summary>現在いる座標のcellの座標(x,y,height)</summary>
    public Vector3Int mFootCellPosition {
        get {
            return new Vector3Int(Mathf.FloorToInt(mMapPosition.x + 0.5f), Mathf.FloorToInt(mMapPosition.y + 0.5f), Mathf.FloorToInt(mHeight));
        }
    }
    /// <summary>現在MapBehaviourに設定されている座標,高さのデータを画像等に反映する</summary>
    public virtual void applyPosition() { }
    /// <summary>座標と高さを設定し画像等に反映する</summary>
    public void setPosition(Vector2 aPosition, float aHeight) {
        position2D = aPosition;
        _Height = aHeight;
        applyPosition();
    }

    ///<summary>MapWorld生成終了直後に呼ばれる(world生成後に追加した場合はこのbehaviourが配置された直後)</summary>
    public virtual void placed() { }
}
