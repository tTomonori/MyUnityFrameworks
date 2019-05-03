using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MapBehaviour {
    [SerializeField] public MapAttribute mAttribute;
    private void Awake(){
        mOffsetZ = 0.5f;
    }
}