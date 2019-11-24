using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEntityImage : MyBehaviour {
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

[CustomEditor(typeof(MapEntityImage))]
public class InspectorOfMapEntityImage : Editor {
    public MapEntityImage mEntityImage { get => target as MapEntityImage; }
    public override void OnInspectorGUI() {
        //元のInspector部分を表示
        base.OnInspectorGUI();
        //ボタンを表示
        if (GUILayout.Button("To UnitImage(stand)")) {
            EntityUnitImage tImage = mEntityImage.gameObject.AddComponent<EntityUnitImage>();
            mEntityImage.GetComponentInParent<MapEntityRenderBehaviour>().mBody = tImage;

            StandMesh tMesh = tImage.gameObject.AddComponent<StandMesh>();
            tImage.mMesh = tMesh;

            DestroyImmediate(mEntityImage);
        }
        if (GUILayout.Button("To UnitImage(lie)")) {
            EntityUnitImage tImage = mEntityImage.gameObject.AddComponent<EntityUnitImage>();
            mEntityImage.GetComponentInParent<MapEntityRenderBehaviour>().mBody = tImage;

            LieMesh tMesh = tImage.gameObject.AddComponent<LieMesh>();
            tImage.mMesh = tMesh;

            DestroyImmediate(mEntityImage);
        }
        if (GUILayout.Button("To GroupImage")) {
            EntityGroupImage tImage = mEntityImage.gameObject.AddComponent<EntityGroupImage>();
            mEntityImage.GetComponentInParent<MapEntityRenderBehaviour>().mBody = tImage;
            DestroyImmediate(mEntityImage);
        }
    }
}