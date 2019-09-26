using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapCell : MapBehaviour {
    public SortingGroup mSortingGroup { get; set; }

    private void Awake() {
        mSortingGroup = gameObject.AddComponent<SortingGroup>();
    }
}
