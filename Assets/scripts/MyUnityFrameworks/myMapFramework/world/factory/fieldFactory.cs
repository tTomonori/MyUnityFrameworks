using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    /// <summary>tile内に配置されたornamentのデータのリスト</summary>
    static private List<MapFileData.Ornament> mOrnamentInTileData;
    /// <summary>フィールド(階層,マス)を生成</summary>
    static public void buildField() {
        mOrnamentInTileData = new List<MapFileData.Ornament>();
        for (int i = 0; i < mWorld.mSize.z; ++i) {
            buildStratum(i);
        }
        buildOrnamentInTile();
        mOrnamentInTileData = null;
    }
    /// <summary>Tile内に配置されたornemntを生成してworldに追加</summary>
    static private void buildOrnamentInTile() {
        foreach (MapFileData.Ornament tData in mOrnamentInTileData) {
            buildOrnament(tData);
        }
    }
}

