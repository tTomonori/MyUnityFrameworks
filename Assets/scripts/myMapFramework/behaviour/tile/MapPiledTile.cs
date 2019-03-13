using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiledTile : MapTile {
    [SerializeField] private Collider2D mEdge;
	void Start () {
        MapTile tParent = transform.parent.GetComponentInParent<MapTile>();
        if (tParent == null) return;
        //親のcolliderを付け直す
        Destroy(tParent.GetComponent<Collider2D>());
        Collider2D tParentCollider = (Collider2D)tParent.gameObject.AddComponent(mEdge.GetType());
        //edgeのcolliderを親にコピー
        switch(mEdge.GetType().ToString()){
            case "UnityEngine.BoxCollider2D":
                BoxCollider2D tMyBox = (BoxCollider2D)mEdge;
                BoxCollider2D tParentBox = (BoxCollider2D)tParentCollider;
                tParentBox.size = tMyBox.size;
                tParentBox.offset = tMyBox.offset;
                break;
        }

        Destroy(mEdge);
	}
}
