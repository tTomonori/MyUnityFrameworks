using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMiddleStandCell : MapCell {
    /// <summary>このcellが直立している座標(cellの中心からの相対座標)</summary>
    [SerializeField] public float mOffsetY;
    public override void applyPosition() {
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfStandCell(mMapPosition.x, mMapPosition.y + mOffsetY, Mathf.FloorToInt(mHeight), mScaffoldLevel, out oPositionZ);
        positionZ = oPositionZ;
    }
}
