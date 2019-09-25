using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStandImage : MyBehaviour {
    /// <summary>このbehaviourの子孫ノードで使えるSpriteRendererを生成</summary>
    static public SpriteRenderer addSpriteRenderer() {
        SpriteRenderer tRenderer = MyBehaviour.create<SpriteRenderer>();
        tRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        tRenderer.gameObject.layer = MyMap.mStandLayerNum;
        return tRenderer;
    }
    private void Awake() {
        //マスク内部のみ表示するように設定,レイヤーの設定
        foreach (SpriteRenderer tRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            tRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tRenderer.gameObject.layer = MyMap.mStandLayerNum;
        }
    }
    /// <summary>画像に高さを反映させる</summary>
    public virtual void setHight(float aHeight) {
        int tStratumLevel = Mathf.FloorToInt(aHeight);
        foreach(SpriteRenderer tRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            tRenderer.gameObject.layer = MyMap.mStratumLayerNum[tStratumLevel];
        }
    }
}
