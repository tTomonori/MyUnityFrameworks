using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapStandBehaviour: MapBehaviour {
    //画像
    [SerializeField] public MapStandImage mImage;
    //sortingLayer
    public SortingGroup mSortingGroup { get; set; }
    private void Awake() {
        mSortingGroup = gameObject.AddComponent<SortingGroup>();
    }

    //座標設定
    public void setPosition(Vector2 aPosition, float aHeight) {
        mMapPosition = aPosition;
        _Height = aHeight;
        mImage.setHight(aHeight);
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfEntity(aPosition.x, aPosition.y, aHeight, out oPositionZ);
        this.positionZ = oPositionZ;
    }

    public override void setHeight(float aHeight) {
        _Height = aHeight;
        mImage.setHight(aHeight);
    }
}
