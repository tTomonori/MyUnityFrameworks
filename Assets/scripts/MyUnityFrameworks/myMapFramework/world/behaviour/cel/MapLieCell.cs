using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLieCell : MapCell {
    public override void setPosition(Vector2 aPosition, float aHeight) {
        mMapPosition = aPosition;
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfLieCell(aPosition.x, aPosition.y, Mathf.FloorToInt(aHeight), out oPositionZ);
        positionZ = oPositionZ;
    }
    public override void setHeight(float aHeight) {

    }
}
