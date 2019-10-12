using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    /// <summary>
    /// 影を生成してfieldに追加
    /// </summary>
    /// <param name="aData">影のデータ</param>
    private static void buildShadow(MapFileData.Shadow aData) {
        List<Vector3> tPositionList = aData.mPosition;
        MyTag tColliderTag = aData.mCollider;
        ImageShadowTrigger tShadow;
        SpriteRenderer tRenderer;
        Vector2 tOffset = aData.mOffset;
        MapCell tCell;
        foreach (Vector3 tPosition in tPositionList) {
            //追加対象のcell
            tCell = mWorld.mCells[Mathf.FloorToInt(tPosition.x), Mathf.FloorToInt(tPosition.y), Mathf.FloorToInt(tPosition.z)];

            tShadow = MyBehaviour.create<ImageShadowTrigger>();
            tShadow.name = "shadow(" + tPosition.x + "," + tPosition.y + "," + tPosition.z + ")";
            //shadePower
            tShadow.mShadePower = aData.mShadePower;
            //position
            tShadow.position = tCell.position;
            //offset
            tShadow.position2D += tOffset;
            //sprite
            tRenderer = tShadow.createChild<SpriteRenderer>();
            tRenderer.sprite = Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/sprites/" + aData.mSpritePath);
            tRenderer.color = new Color(0, 0, 0, aData.mShadePower);
            //collider
            Collider2DCreator.addCollider(tShadow.gameObject, tColliderTag);
            //追加
            tShadow.transform.SetParent(mWorld.mStratums[Mathf.FloorToInt(tPosition.z)].mShadows.transform, false);
            tShadow.gameObject.AddComponent<SortingGroup>().sortingOrder = tCell.mSortingGroup.sortingOrder;
            tShadow.positionZ -= 0.0001f;
            tShadow.changeLayer(tCell.gameObject.layer, true);
        }
    }
}

