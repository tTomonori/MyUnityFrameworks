using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntity : MapBehaviour {
    //画像
    public virtual MapEntityImage mImage { get; set; }
    /// <summary>このentityに付いている属性</summary>
    [SerializeField]public EntityPhysicsAttribute mAttribute;
}
