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
        /// <summary>指定名のキャラのAIをジャック</summary>
        public MapCharacter.JackedAi jack(string aName) {
            MapCharacter.JackedAi tAi = parent.jack(aName);
            if (tAi == null) return null;
            mAiDic.Add(aName, tAi);
            return tAi;
        }
        /// <summary>指定名のキャラのAIを取得(予約名考慮)</summary>
        public MapCharacter.JackedAi getAi(string aName) {
            if (mAiDic.ContainsKey(aName)) {
                return mAiDic[aName];
            }
            //ジャックしていない場合はジャックしてから渡す
            return jack(aName);
        }
        /// <summary>指定名のキャラを取得(予約名考慮)</summary>
        public MapCharacter getCharacter(string aName) {
            if (aName == "invoker") return mInvoker;
            if (aName == "invoked") return (MapCharacter)mInvoked;
            return parent.mWorld.getCharacter(aName);
        }
    }
}
