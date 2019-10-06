using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLieCell : MapCell {
    public override void applyPosition() {
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfLieCell(mMapPosition.x, mMapPosition.y, Mathf.FloorToInt(mHeight), mScaffoldHeight + mDrawOffsetHeight, out oPositionZ);
        positionZ = oPositionZ;
    }
}
