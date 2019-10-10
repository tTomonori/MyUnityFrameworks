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
    public override void applyPosition() {
        mImage.setHight(mHeight);
        //order更新
        float oPositionZ;
        mSortingGroup.sortingOrder = MapZOrderCalculator.calculateOrderOfEntity(mMapPosition.x, mMapPosition.y, mHeight, mScaffoldLevel, out oPositionZ);
        this.positionZ = oPositionZ;
        //レイヤー更新
        mImage.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(mHeight)]);
    }
}
