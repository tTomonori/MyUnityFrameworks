using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    ///<summary>マップ上での座標</summary>
    public Vector2 mMapPosition {
        get { return position2D; }
        set { position2D = value; }
    }
    protected float _Height;
    /// <summary>マップ上での高さ</summary>
    public float mHeight {
        get { return _Height; }
    }
    public virtual void setHeight(float aHeight) {
        _Height = aHeight;
    }
    public virtual void setPosition(Vector2 aPosition,float aHeight) {
        mMapPosition = aPosition;
        _Height = aHeight;
    }

    ///<summary>MapWorld生成終了直後に呼ばれる(world生成後に追加した場合はこのbehaviourが配置された直後)</summary>
    public virtual void placed() { }
}
