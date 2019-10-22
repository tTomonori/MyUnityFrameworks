using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    /// <summary>名前</summary>
    public string mName { get; set; }
    /// <summary>衝突判定が適用される高さ(このBehaviourの高さ +0 ~ +mColliderHeight未満 の範囲で衝突)</summary>
    [SerializeField] public float mCollideHeight = 1f;
    /// <summary>平面behaviourを重ねた場合の描画順序を決定するための値</summary>
    public int mLieBehaviourPileLevel = 0;
    /// <summary>3次元変換した場合の座標</summary>
    public MapRealPosition mMapRealPosition {
        get { return mMapPosition.toMapRealPosition(); }
        set { mMapPosition = value.toMapPosition(); }
    }
    /// <summary>マップ上での座標</summary>
    public MapPosition mMapPosition {
        get { return new MapPosition(new Vector3(positionX, positionY, _Height)); }
        set {
            position2D = value.vector;
            _Height = value.h;
        }
    }
    /// <summary>描画する際の座標</summary>
    public RenderPosition mRenderPosition {
        get { return new RenderPosition(position); }
        set { position = value.vector; }
    }
    protected float _Height;
    /// <summary>マップ上での高さ</summary>
    public float mHeight {
        get { return _Height; }
        set { _Height = value; }
    }
    /// <summary>現在いる座標のcellの座標(x,y,height)</summary>
    public Vector3Int mFootCellPosition {
        get {
            Vector3 tPosition = mMapPosition.vector;
            return new Vector3Int(Mathf.FloorToInt(tPosition.x + 0.5f), Mathf.FloorToInt(tPosition.y + 0.5f), Mathf.FloorToInt(_Height));
        }
    }
    /// <summary>MapPositionをRenderPositionに適用</summary>
    public virtual void applyPosition() {
        mRenderPosition = mMapPosition.toRenderPosition().addPileLevel(mLieBehaviourPileLevel);
    }
    /// <summary>座標と高さを設定し画像等に反映する</summary>
    public void setMapPosition(Vector2 aPosition, float aHeight) {
        position2D = aPosition;
        _Height = aHeight;
        applyPosition();
    }

    ///<summary>MapWorld生成終了直後に呼ばれる(world生成後に追加した場合はこのbehaviourが配置された直後)</summary>
    public virtual void placed() { }
}
