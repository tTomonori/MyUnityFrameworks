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
                //足場の高さ
                tCharacter.mScaffoldHeight = getScaffoldHeight(tCharacter.mFootCellPosition, aWorld);
                //座標適用
                tCharacter.setPosition(tCharacter.mMapPosition, tCharacter.mHeight);
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
    /// <summary>指定座標の足場の高さを返す</summary>
    static public float getScaffoldHeight(Vector3Int aPosition, MapWorld aWorld) {
        MapCell tLowerCell;
        if (aWorld.mSize.z - 1 >= aPosition.z * 2 + 1) {
            tLowerCell = aWorld.mCells[aPosition.x, aPosition.y, aPosition.z * 2 + 1];
            if (tLowerCell != null) return tLowerCell.mScaffoldHeight;
        }
        tLowerCell = aWorld.mCells[aPosition.x, aPosition.y, aPosition.z * 2];
        if (tLowerCell != null) return tLowerCell.mScaffoldHeight;

        for (int i = 1; i < aPosition.z; ++i) {
            tLowerCell = aWorld.mCells[aPosition.x, aPosition.y - i, (aPosition.z - i) * 2 + 1];
            if (tLowerCell != null) return tLowerCell.mScaffoldHeight;
            tLowerCell = aWorld.mCells[aPosition.x, aPosition.y - i, (aPosition.z - i) * 2];
        }
        return 0;
    }
}
