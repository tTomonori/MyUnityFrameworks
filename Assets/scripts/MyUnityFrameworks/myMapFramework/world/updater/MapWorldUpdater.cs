using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapWorldUpdater {
    //<summary>フレーム毎の更新</summary>
    public static void updateWorld(MapWorld aWorld) {
        //キャラの内部状態を更新して移動先決定
        foreach (MapCharacter tChara in aWorld.mCharacters) {
            tChara.updateInternalState();
            //移動用データ初期化
            MapCharacterMoveSystem.initFrameMovingData(tChara);
        }
        //キャラの移動
        //まだ移動可能なキャラ
        List<MapCharacter> tWaiting = new List<MapCharacter>();
        List<MapCharacter> tProcessing = new List<MapCharacter>(aWorld.mCharacters);
        bool tExistWaiting = false;
        for (; ; ) {
            foreach (MapCharacter tCharacter in tProcessing) {
                //移動
                bool tRemainedDistance = MapCharacterMoveSystem.moveCharacter(tCharacter);
                //高さ更新
                MapHeightUpdateSystem.updateHeight(tCharacter);
                //trigger

                //まだ移動できるか
                if (tRemainedDistance) {
                    tWaiting.Add(tCharacter);
                    tExistWaiting = true;
                }

                //これ以上移動しない
                //足場の高さレベル
                tCharacter.mScaffoldLevel = getScaffoldLevel(tCharacter.mMapPosition, tCharacter.mHeight, aWorld);
                //座標適用
                tCharacter.setPosition(tCharacter.mMapPosition, tCharacter.mHeight);
                //画像イベント
                applyImageEvent(tCharacter);
                //移動データリセット
                MapCharacterMoveSystem.resetFrameMovingData(tCharacter);
            }
            if (!tExistWaiting)
                break;
            tProcessing = tWaiting;
            tWaiting.Clear();
            tExistWaiting = false;
        }
    }
    /// <summary>画像イベントを適用する</summary>
    static public void applyImageEvent(MapStandBehaviour aBehaviour) {
        ImageEventTrigger tTrigger;
        foreach(Collider2D tCollider in Physics2D.OverlapPointAll(aBehaviour.worldPosition2D)) {
            tTrigger = tCollider.GetComponent<ImageEventTrigger>();
            if (tTrigger == null) continue;

        }
    }
    /// <summary>指定座標の足場の高さを返す</summary>
    static public float getScaffoldLevel(Vector2 aPosition, float aHeight, MapWorld aWorld) {
        float tLevel;
        int tX = Mathf.FloorToInt(aPosition.x + 0.5f);
        int tY = Mathf.FloorToInt(aPosition.y + 0.5f);
        int tH = Mathf.FloorToInt(aHeight);
        //マップサイズの範囲外
        if (tX < 0 || aWorld.mSize.x - 1 < tX || tY < 0 || aWorld.mSize.y - 1 < tY) {
            return 0;
        }
        //cellから高さレベル取得
        Vector2 tLocal = new Vector2((aPosition.x + 0.5f).decimalPart(), (aPosition.y + 0.5f).decimalPart());
        for (int i = 0; ; ++i) {
            if (aWorld.mSize.z - 1 < tH - i) continue;
            if (aWorld.mSize.y - 1 < tY - i) continue;
            if (tY - i < 0) break;
            if (tH - i < 0) break;
            tLevel = getScaffoldLevelFromCell(new Vector2Int(tX, tY - i), tH - i, tLocal, aWorld, i != 0);
            if (0 <= tLevel) return tLevel;
        }
        return 0;
    }
    /// <summary>
    /// 指定座標のcell(0.5階層を含む)から足場の高さレベルを取得
    /// </summary>
    /// <returns>足場の高さレベル</returns>
    /// <param name="aPosition">cellの座標</param>
    /// <param name="aHeight">cellの高さ</param>
    /// <param name="aLocalPositionFromCell">cellからの相対座標(左下が(0,0))</param>
    /// <param name="aWorld">MapWorld</param>
    static private float getScaffoldLevelFromCell(Vector2Int aPosition, int aHeight, Vector2 aLocalPositionFromCell, MapWorld aWorld, bool aCorrectYOnSlope) {
        MapCell tCell;
        float tLevel;
        //0.5階層
        int tZ = aHeight * 2 + 1;
        if (tZ <= aWorld.mSize.z - 1) {
            tCell = aWorld.mCells[aPosition.x, aPosition.y, tZ];
            if (tCell != null) {
                int tStandY;
                float tLocalY;
                //左右方向の坂だった場合は参照するcellを変更
                if (aCorrectYOnSlope && tCell.mScaffoldType == MapCell.ScaffoldType.leftHighSlope) {
                    tLocalY = aLocalPositionFromCell.y + aLocalPositionFromCell.x;
                    tStandY = (1f <= tLocalY) ? aPosition.y + 1 : aPosition.y;
                    tLocalY = tLocalY % 1;
                } else if (aCorrectYOnSlope && tCell.mScaffoldType == MapCell.ScaffoldType.rightHighSlope) {
                    tLocalY = aLocalPositionFromCell.y + (1f - aLocalPositionFromCell.x);
                    tStandY = (1f <= tLocalY) ? aPosition.y + 1 : aPosition.y;
                    tLocalY = tLocalY % 1;
                } else {
                    tStandY = aPosition.y;
                    tLocalY = aLocalPositionFromCell.y;
                }
                tCell = aWorld.mCells[aPosition.x, tStandY, tZ];
                if (tCell != null) {
                    //高さレベル取得
                    tLevel = tCell.getScaffoldLevel(new Vector2(aLocalPositionFromCell.x - 0.5f, tLocalY - 0.5f));
                    if (0 <= tLevel) return tLevel;
                }
            }
        }
        //平面階層
        tCell = aWorld.mCells[aPosition.x, aPosition.y, aHeight * 2];
        if (tCell != null) {
            return tCell.getScaffoldLevel(aLocalPositionFromCell - new Vector2(0.5f, 0.5f));
        }
        return -1;
    }
}
