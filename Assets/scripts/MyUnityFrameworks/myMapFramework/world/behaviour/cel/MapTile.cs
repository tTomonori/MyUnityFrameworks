using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapTile : MapBehaviour {
    /// <summary>足場の形状(描画順序決定用)</summary>
    [SerializeField] public ScaffoldType mScaffoldType = ScaffoldType.custom;
    /// <summary>orderInLayerを計算するときの補正</summary>
    public DrawOffsetData mDrawOffsetData = new DrawOffsetData();

    public enum ScaffoldType {
        flat, stand, upHighSlope, downHighSlope, leftHighSlope, rightHighSlope, custom, none
    }
    public class DrawOffsetData {
        public float mHeight;
    }

    /// <summary>MapPositionをRenderPositionに適用</summary>
    public override void applyPosition() {
        mRenderPosition = (mMapPosition + new Vector3(0, 0, mDrawOffsetData.mHeight)).toRenderPosition().addPileLevel(mLieBehaviourPileLevel);
    }

    private void OnValidate() {
        if (Application.isPlaying) return;
        if (mScaffoldType == ScaffoldType.custom || mScaffoldType == ScaffoldType.none) return;
        //image
        Mesh2D tImage = findChild<Mesh2D>("image");
        if (tImage == null) {
            tImage = (isLieCell()) ? (Mesh2D)this.createChild<LieMesh>() : (Mesh2D)this.createChild<StandMesh>();
            tImage.name = "image";
        }
        tImage.position = (isLieCell()) ? Vector3.zero : new MapPosition(new Vector3(0, -0.5f, 0)).toRenderPosition().vector;
        //collider
        TileGroundPhysicsAttribute tAttribute = findChild<TileGroundPhysicsAttribute>("attribute");
        if (tAttribute == null) {
            tAttribute = createChild<TileGroundPhysicsAttribute>();
            tAttribute.name = "attribute";
        }
    }
    public bool isLieCell() {
        switch (mScaffoldType) {
            case ScaffoldType.flat:
            case ScaffoldType.upHighSlope:
            case ScaffoldType.downHighSlope:
            case ScaffoldType.leftHighSlope:
            case ScaffoldType.rightHighSlope:
                return true;
            case ScaffoldType.stand:
            case ScaffoldType.custom:
            case ScaffoldType.none:
                return false;
        }
        return false;
    }
}
