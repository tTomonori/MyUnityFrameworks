using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStandImage : MyBehaviour {
    /// <summary>最後に適用した画像イベント</summary>
    private ImageEventData mLastImageEventData = new ImageEventData();

    [System.NonSerialized] public int mLayerNum = -1;
    /// <summary>このcomponentの子孫ノードで使えるSpriteRendererを生成</summary>
    public SpriteRenderer addSpriteRenderer() {
        SpriteRenderer tRenderer = MyBehaviour.create<SpriteRenderer>();
        tRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        tRenderer.gameObject.layer = (mLayerNum==-1)?0:mLayerNum;
        return tRenderer;
    }
    /// <summary>SpriteRendererをこのcomponentの子孫ノードで使える様に設定する</summary>

    public void adaptSpriteRenderer(SpriteRenderer aRenderer) {
        aRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        aRenderer.gameObject.layer = (mLayerNum == -1) ? 0 : mLayerNum;
    }
    protected void Awake() {
        //マスク外部のみ表示するように設定,レイヤーの設定
        foreach (SpriteRenderer tRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            tRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }
    /// <summary>画像に高さを反映させる</summary>
    public virtual void setHight(float aHeight) {
    }
    public virtual void updateBelongLayer(int aLayerNum) {
        if (mLayerNum == aLayerNum) return;
        mLayerNum = aLayerNum;
        this.changeLayer(aLayerNum);
    }

    /// <summary>
    /// 画像イベントを適用する
    /// </summary>
    /// <param name="aData">適用する画像イベントのデータ</param>
    public void applyImageEvent(ImageEventData aData) {
        //影
        if (mLastImageEventData.mShadow != aData.mShadow) {
            float tShadow = 1f - aData.mShadow;
            foreach(SpriteRenderer tRenderer in GetComponentsInChildren<SpriteRenderer>()) {
                tRenderer.color = new Color(tShadow, tShadow, tShadow, 1);
            }
        }
        //表示位置補正
        if (mLastImageEventData.mShift != aData.mShift) {

        }
        mLastImageEventData = aData;
    }
}
