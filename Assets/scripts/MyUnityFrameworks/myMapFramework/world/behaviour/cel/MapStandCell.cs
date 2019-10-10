using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStandCell : MapCell {
    public override void applyPosition() {
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfStandCell(mMapPosition.x, mMapPosition.y, Mathf.FloorToInt(mHeight), mScaffoldLevel + mDrawOffsetHeight, out oPositionZ);
        positionZ = oPositionZ;
    }
}
