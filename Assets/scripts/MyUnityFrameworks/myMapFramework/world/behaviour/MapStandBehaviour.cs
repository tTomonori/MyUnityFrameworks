using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapStandBehaviour : MapBehaviour {
    //画像
    public virtual MapStandImage mImage { get; set; }
    //sortingLayer
    public SortingGroup mSortingGroup { get; set; }
    private void Awake() {
        mSortingGroup = gameObject.AddComponent<SortingGroup>();
    }

    //座標設定
    public override void setPosition(Vector2 aPosition, float aHeight) {
        mMapPosition = aPosition;
        mImage.setHight(aHeight);
        //order更新
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfEntity(aPosition.x, aPosition.y, aHeight, out oPositionZ);
        this.positionZ = oPositionZ;
        //レイヤー更新
            mImage.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(aHeight)]);
        _Height = aHeight;
    }

    public override void setHeight(float aHeight) {
        _Height = aHeight;
        mImage.updateBelongLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(aHeight)]);
        mImage.setHight(aHeight);
    }
}
