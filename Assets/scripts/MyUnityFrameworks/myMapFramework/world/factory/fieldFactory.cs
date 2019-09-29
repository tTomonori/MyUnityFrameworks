using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MapWorldFactory {
    /// <summary>フィールド(階層,マス)を生成</summary>
    static public void buildField() {
        for (int i = 0; i < mWorld.mSize.z; ++i) {
            buildStratum(i);
        }
    }
}

