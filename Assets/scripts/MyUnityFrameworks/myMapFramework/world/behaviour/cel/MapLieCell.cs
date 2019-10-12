using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLieCell : MapCell {
    public override void applyPosition() {
        float oPositionZ;
        if (mDrawOffsetData == null)
            mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfLieCell(mMapPosition.x, mMapPosition.y, Mathf.FloorToInt(mHeight), mScaffoldLevel, out oPositionZ);
        else
            mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfLieCell(mMapPosition.x, mDrawOffsetData.mPositionY, Mathf.FloorToInt(mHeight), mDrawOffsetData.mScaffoldLevel, out oPositionZ);
        positionZ = oPositionZ;
    }
}
