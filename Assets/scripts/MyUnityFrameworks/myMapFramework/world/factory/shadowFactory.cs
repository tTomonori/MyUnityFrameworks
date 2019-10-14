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
        LieMesh tMesh;
        Vector3 tOffset = aData.mOffset;
        MapCell tCell;
        MapTile tTile;
        foreach (Vector3 tPosition in tPositionList) {
            //追加対象のtile
            tCell = mWorld.mCells[Mathf.FloorToInt(tPosition.x), Mathf.FloorToInt(tPosition.y), Mathf.FloorToInt(tPosition.z)];
            tTile = (tPosition.z.decimalPart() > 0.4f) ? tCell.mHalfHeightTile : tCell.mTile;

            tShadow = MyBehaviour.create<ImageShadowTrigger>();
            tShadow.name = "shadow(" + tPosition.x + "," + tPosition.y + "," + tPosition.z + ")";
            //shadePower
            tShadow.mShadePower = aData.mShadePower;
            //position
            tShadow.mMapPosition = new MapPosition(tTile.mMapPosition.vector + tOffset);
            tShadow.mLieBehaviourPileLevel = 5;
            tShadow.applyPosition();
            //sprite
            tMesh = tShadow.createChild<LieMesh>();
            tMesh.mRenderMode = Mesh2D.RenderMode.shadow;
            tMesh.mSprite= Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/sprites/" + aData.mSpritePath);
            tMesh.initialize();
            tMesh.setColor(new Color(0, 0, 0, aData.mShadePower));
            //collider
            Collider2DCreator.addCollider(tShadow.gameObject, tColliderTag);
            //追加
            tShadow.transform.SetParent(mWorld.mStratums[Mathf.FloorToInt(tPosition.z)].mShadows.transform, false);
            tShadow.changeLayer(MyMap.mStratumLayerNum[Mathf.FloorToInt(tPosition.z)], true);
        }
    }
}

