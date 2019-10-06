using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapCell : MapBehaviour {
    /// <summary>下方にある物を隠すか</summary>
    [SerializeField] public bool mHideLower = true;
    /// <summary>orderInLayerを計算するときこの数値分高さを補正</summary>
    [SerializeField] public int mDrawOffsetHeight = 0;
    public SortingGroup mSortingGroup { get; set; }

    private void Awake() {
        mSortingGroup = gameObject.AddComponent<SortingGroup>();
    }
}
