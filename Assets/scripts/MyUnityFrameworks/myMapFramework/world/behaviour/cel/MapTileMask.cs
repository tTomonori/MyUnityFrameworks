using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapTileMask : MyBehaviour {
    //<summary>兄ノードにかけるマスク画像</summary>
    [SerializeField] public Sprite mMaskImage;

    private void Start() {
        foreach(MapTile tTile in transform.parent.GetComponentsInChildren<MapTile>()) {
            if (tTile.positionZ <= positionZ) continue;

            tTile.gameObject.AddComponent<SpriteMask>().sprite = mMaskImage;
            tTile.gameObject.AddComponent<SortingGroup>();
            foreach(SpriteRenderer tRenderer in tTile.GetComponentsInChildren<SpriteRenderer>()) {
                tRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }
    }
}
