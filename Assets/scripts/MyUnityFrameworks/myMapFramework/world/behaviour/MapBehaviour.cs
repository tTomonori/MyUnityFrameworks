﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    //<summary>マップ上での座標</summary>
    public Vector2 mMapPosition {
        get { return position2D; }
        set { position2D = value; }
    }
}
