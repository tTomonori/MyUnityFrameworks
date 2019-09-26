using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMiddleStandCell : MapCell {
    /// <summary>このcellが直立している座標(cellの中心からの相対座標)</summary>
    [SerializeField] public float mOffsetY;
    public override void setPosition(Vector2 aPosition, float aHeight) {
        mMapPosition = aPosition;
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfStandCell(aPosition.x, aPosition.y + mOffsetY, Mathf.FloorToInt(aHeight), out oPositionZ);
        positionZ = oPositionZ;
    }
    public override void setHeight(float aHeight) {

    }
}
