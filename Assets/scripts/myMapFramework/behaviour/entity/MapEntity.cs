using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntity : MapBehaviour {
    [SerializeField] protected BoxCollider2D mCollider;
    [SerializeField] protected MapAttribute mAttribute;
    public BoxCollider2D boxCollider{
        get { return mCollider; }
    }
    public MapAttribute attribute{
        get { return mAttribute; }
    }
}
