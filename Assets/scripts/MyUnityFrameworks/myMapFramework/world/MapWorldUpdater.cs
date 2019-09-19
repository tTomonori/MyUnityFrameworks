using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapWorldUpdater {
    //<summary>フレーム毎の更新</summary>
    public static void updateWorld(MapWorld aWorld) {
        //キャラの内部状態を更新して移動先決定
        foreach(MapCharacter tChara in aWorld.mCharacters) {
            tChara.updateInternalState();
        }
        //キャラの移動
        foreach(MapCharacter tChara in aWorld.mCharacters) {
            MapCharacterMoveSystem.moveCharacter(tChara);
        }
    }
}
