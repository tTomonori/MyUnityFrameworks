using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStandCell : MapCell {
    public override void applyPosition() {
        float oPositionZ;
        if (mDrawOffsetData == null)
            mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfStandCell(mMapPosition.x, mMapPosition.y, Mathf.FloorToInt(mHeight), mScaffoldLevel, out oPositionZ);
        else
            mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfStandCell(mMapPosition.x, mDrawOffsetData.mPositionY, Mathf.FloorToInt(mHeight), mDrawOffsetData.mScaffoldLevel, out oPositionZ);
        positionZ = oPositionZ;
    }
}
