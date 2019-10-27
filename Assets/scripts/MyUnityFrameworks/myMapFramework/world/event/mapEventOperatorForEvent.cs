using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapEventSystem {
    public partial class Operator {
        /// <summary>引数のイベントをこのoperatorで実行</summary>
        public void improvisedRun(MapEvent aEvent, Action<Arg> aOnEnd) {
            aEvent.run(this, aOnEnd);
        }
        /// <summary>指定名のキャラのAIをジャック(予約名考慮)(ジャック成功時もしくは指定名のキャラが存在しない時true)</summary>
        public bool jack(string aName) {
            //ジャックするキャラ取得
            MapCharacter tCharacter = null;
            if (aName == "invoker") {
                tCharacter = mInvoker;
            } else if (aName == "invoked" && mInvoked is MapCharacter) {
                tCharacter = (MapCharacter)mInvoked;
            } else if (aName == "player") {
                tCharacter = parent.mWorld.getPlayer();
            } else {
                tCharacter = parent.mWorld.getCharacter(aName);
            }
            if (tCharacter == null) return true;
            //ジャックスする
            MapCharacter.JackedAi tAi = tCharacter.jack();
            if (tAi != null) {
                //ジャックできた
                mAiDic.Add(aName, tAi);
                mAiDic.Add(tCharacter.mName, tAi);
                return true;
            }
            //ジャックできなかった場合
            if (mAiDic.ContainsKey(aName))
                return true;//既にジャック済み
            //ジャック失敗
            return false;
        }
        /// <summary>指定名のキャラのAIを取得(予約名考慮)</summary>
        public MapCharacter.JackedAi getAi(string aName) {
            if (!mAiDic.ContainsKey(aName)) {
                //ジャックしていない場合はジャックする
                jack(aName);
            }
            return mAiDic[aName];
        }
        /// <summary>指定名のキャラを取得(予約名考慮)</summary>
        public MapCharacter getCharacter(string aName) {
            if (aName == "invoker") return mInvoker;
            if (aName == "invoked") return (MapCharacter)mInvoked;
            return parent.mWorld.getCharacter(aName);
        }
    }
}
