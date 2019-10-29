using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    /// <summary>カメラを生成してworldに追加</summary>
    static private void initCamera() {
        MapCamera tCamera = MapCamera.init(mWorld);
        tCamera.name = "worldMainCamera";
        tCamera.transform.SetParent(mWorld.mCameraContainer.transform, false);
        tCamera.mConfig = new MapCamera.ProjectPlayerInCenter();
        tCamera.mMaxMargin = mWorld.mFileData.mFieldMargin;
        if (mWorld.mFileData.mCameraSize > 0)
            tCamera.mCameraSize = mWorld.mFileData.mCameraSize;
        mWorld.mCameras.Add(tCamera);
    }
}
