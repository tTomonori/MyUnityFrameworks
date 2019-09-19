using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    //<summary>今いる階層</summary>
    public MapStratumLevel mStratumLevel;
    //<summary>マップ上での座標</summary>
    public Vector2 mMapPosition {
        get { return position2D; }
        set { position2D = value; }
    }

    //<summary>MapWorld内に配置された直後に呼ばれる</summary>
    public virtual void placed() { }
}
