using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntityImage : MyBehaviour{
    /// <summary>最後に適用した画像イベント</summary>
    private ImageEventData mLastImageEventData = new ImageEventData();

    /// <summary>
    /// 画像イベントを適用する
    /// </summary>
    /// <param name="aData">適用する画像イベントのデータ</param>
    public void applyImageEvent(ImageEventData aData) {
        //影
        if (mLastImageEventData.mShadow != aData.mShadow) {
            shade(aData);
        }
        //表示位置補正
        if (mLastImageEventData.mShift != aData.mShift) {
            correctPosition(aData);
        }
        mLastImageEventData = aData;
    }
    /// <summary>この画像に影を落とす</summary>
    public virtual void shade(ImageEventData aData) { }
    /// <summary>この画像の座標を弄る</summary>
    public virtual void correctPosition(ImageEventData aData) { }
}
