using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapKeyEventTrigger : MapTrigger {
    /// <summary>このtriggerを発火させるentityの名前(空なら全てのentityが発火させる)</summary>
    public List<string> mTriggerKey;
    /// <summary>このtriggerを発火させるentityとの衝突</summary>
    public MapPhysics.CollisionType mCollisionType = MapPhysics.CollisionType.pass;

    //発火するイベントのKey
    public string mEnterKey;
    public string mStayKey;
    public string mMovedKey;
    public string mExitKey;

    //<summary>引数のentityが衝突するか</summary>
    public override MapPhysics.CollisionType canCollide(MapEntity aEntity) {
        if (mCollisionType == MapPhysics.CollisionType.pass) return MapPhysics.CollisionType.pass;
        //キャラでないなら衝突しない
        if (!(aEntity is MapCharacter))
            return MapPhysics.CollisionType.pass;

        //キャラの場合
        return isTriggerCharacter((MapCharacter)aEntity) ? mCollisionType : MapPhysics.CollisionType.pass;
    }
    /// <summary>指定キャラがこのtriggerを発火させるか(trueなら発火)</summary>
    public bool isTriggerCharacter(MapCharacter aCharacter) {
        //AI操作でないなら発火しない
        if (aCharacter.getOperation() != MapCharacter.Operation.free) return false;

        foreach (string tKeyName in mTriggerKey) {
            //プレイヤーか
            if (tKeyName == "player") {
                if (aCharacter.isPlayer())
                    return true;
                continue;
            }
            //名前が一致するか
            if (tKeyName == aCharacter.mName) {
                return true;
            }
        }
        return false;
    }
    /// <summary>triggerに侵入した</summary>
    public override void enter(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        if (!isTriggerCharacter(aCharacter)) return;
        if (mEnterKey == "" || mEnterKey == null) return;
        aEventSystem.addEvent(mEnterKey, aCharacter, mBehaviour, mCollider);
    }
    /// <summary>trigger内で移動せずに居座った</summary>
    public override void stay(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        if (!isTriggerCharacter(aCharacter)) return;
        if (mStayKey == "" || mStayKey == null) return;
        aEventSystem.addEvent(mStayKey, aCharacter, mBehaviour, mCollider);
    }
    /// <summary>trigger内で移動した</summary>
    public override void moved(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        if (!isTriggerCharacter(aCharacter)) return;
        if (mMovedKey == "" || mMovedKey == null) return;
        aEventSystem.addEvent(mMovedKey, aCharacter, mBehaviour, mCollider);
    }
    /// <summary>triggerから出て行った</summary>
    public override void exit(MapCharacter aCharacter, MapEventSystem aEventSystem) {
        if (!isTriggerCharacter(aCharacter)) return;
        if (mExitKey == "" || mExitKey == null) return;
        aEventSystem.addEvent(mExitKey, aCharacter, mBehaviour, mCollider);
    }
}
