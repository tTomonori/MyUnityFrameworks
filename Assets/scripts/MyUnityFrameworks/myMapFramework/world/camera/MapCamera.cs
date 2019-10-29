using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MyBehaviour {
    public Camera mCamera;
    /// <summary>映す対象</summary>
    public MapWorld mWorld;
    /// <summary>設定</summary>
    public MapCameraConfig mConfig;
    /// <summary>Worldをはみ出して写して良い最大距離</summary>
    public float mMaxMargin = 2f;
    /// <summary>カメラの描画範囲</summary>
    public float mCameraSize {
        get { return mCamera.orthographicSize; }
        set { mCamera.orthographicSize = value; }
    }
    static public MapCamera init(MapWorld aWorld) {
        MapCamera tCamera = MyBehaviour.create<MapCamera>();
        tCamera.mWorld = aWorld;
        tCamera.mCamera = tCamera.createChildCamera(aWorld.mSize.z);
        tCamera.mConfig = new MapCameraConfig();
        tCamera.positionZ = -10;
        return tCamera;
    }
    public void updateCamera() {
        mConfig.update(this);
        applyMargin();
    }
    /// <summary>子要素としてカメラ生成</summary>
    private Camera createChildCamera(int aStratumNum) {
        Camera tCamera = this.createChild<Camera>();
        tCamera.name = "mapCamera";
        tCamera.clearFlags = CameraClearFlags.Skybox;
        tCamera.orthographic = true;
        //tCamera.depth = 0;
        tCamera.orthographicSize = 5;
        tCamera.cullingMask = (1 << MyMap.mStratumLayerNum[0]);
        for (int i = 1; i < aStratumNum; ++i)
            tCamera.cullingMask |= (1 << MyMap.mStratumLayerNum[i]);

        return tCamera;
    }
    /// <summary>Marginを考慮して調整</summary>
    private void applyMargin() {
        if (mMaxMargin < 0) {//負の値を設定した場合は調整しない
            mCamera.transform.position = new Vector3(0, 0, 0);
            return;
        }

        float tX = 0;
        float tY = 0;
        float tCameraHalfHeight = mCamera.orthographicSize;
        float tCameraHalfWidth = mCamera.orthographicSize * Screen.width / Screen.height;
        //上下方向
        if (mWorld.mSize.y / 2f + mMaxMargin <= tCameraHalfHeight) {
            tY = mWorld.mSize.y / 2f - 0.5f;
        } else {
            if (mWorld.mSize.y + mMaxMargin - 0.5f - tCameraHalfHeight <= this.positionY) {
                tY = mWorld.mSize.y + mMaxMargin - 0.5f - tCameraHalfHeight;
            } else if (this.positionY <= -mMaxMargin - 0.5f + tCameraHalfHeight) {
                tY = -mMaxMargin - 0.5f + tCameraHalfHeight;
            } else {
                tY = this.positionY;
            }
        }
        //左右方向
        if (mWorld.mSize.x / 2f + mMaxMargin <= tCameraHalfWidth) {
            tX = mWorld.mSize.x / 2f - 0.5f;
        } else {
            if (mWorld.mSize.x + mMaxMargin - 0.5f - tCameraHalfWidth <= this.positionX) {
                tX = mWorld.mSize.x + mMaxMargin - 0.5f - tCameraHalfWidth;
            } else if (this.positionX <= -mMaxMargin - 0.5f + tCameraHalfWidth) {
                tX = -mMaxMargin - 0.5f + tCameraHalfWidth;
            } else {
                tX = this.positionX;
            }
        }

        mCamera.transform.localPosition = new Vector3(tX - this.positionX, tY - this.positionY, 0);
    }


    public class MapCameraConfig {
        public virtual void update(MapCamera aParent) { }
    }
    /// <summary>プレイヤーを常に中心に映す</summary>
    public class ProjectPlayerInCenter : MapCameraConfig {
        public MapCharacter mPlayer;

        public override void update(MapCamera aParent) {
            if (mPlayer == null) mPlayer = aParent.mWorld.getPlayer();
            if (mPlayer == null)
                return;
            aParent.position2D = mPlayer.mMapPosition.vector2;
        }
    }
    public void test<T>() where T : MyBehaviour {

    }
}
