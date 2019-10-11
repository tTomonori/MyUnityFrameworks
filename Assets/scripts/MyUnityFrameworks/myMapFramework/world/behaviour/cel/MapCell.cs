using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapCell : MapBehaviour {
    /// <summary>足場の形状(描画順序決定用)</summary>
    [SerializeField] public ScaffoldType mScaffoldType = ScaffoldType.flat;
    /// <summary>足場の高さレベル</summary>
    public float mScaffoldSurfaceLevel { get; set; }
    /// <summary>足場の高さレベル(坂形状の場合のみ使用,下側の高さレベル)</summary>
    public float mScaffoldSurfaceLevel2 { get; set; }
    /// <summary>orderInLayerを計算するときこの数値分高さを補正</summary>
    [SerializeField] public int mDrawOffsetHeight = 0;

    public SortingGroup mSortingGroup { get; set; }

    private void Awake() {
        mSortingGroup = gameObject.AddComponent<SortingGroup>();
    }

    /// <summary>
    /// 指定相対座標の足場の高さレベルを取得する(負の値を返す場合は足場なし)
    /// </summary>
    /// <returns>足場の高さレベル</returns>
    /// <param name="aPosition">高さレベルを取得する座標(相対座標)</param>
    public float getScaffoldLevel(Vector2 aPosition) {
        switch (mScaffoldType) {
            case ScaffoldType.flat:
                return mScaffoldSurfaceLevel;
            case ScaffoldType.leftHighSlope:
                if (-aPosition.x <= aPosition.y) return mScaffoldSurfaceLevel;
                else return mScaffoldSurfaceLevel2;
            case ScaffoldType.rightHighSlope:
                if (aPosition.x <= aPosition.y) return mScaffoldSurfaceLevel;
                else return mScaffoldSurfaceLevel2;
            case ScaffoldType.stand:
                return -1;
        }
        return -1;
    }

    public enum ScaffoldType {
        flat,stand,leftHighSlope,rightHighSlope,none
    }
}
