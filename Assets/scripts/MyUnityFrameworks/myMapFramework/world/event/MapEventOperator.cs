using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapEventSystem {
    public class Operator {
        /// <summary>イベント通知先</summary>
        public MyMapEventDelegate mDelegate {
            get { return parent.mWorld.mMap.mDelegate; }
        }
        /// <summary>乗っ取りしたキャラのAI</summary>
        private Dictionary<string, MapCharacter.JackedAi> mAiDic = new Dictionary<string, MapCharacter.JackedAi>();
        /// <summary>イベントを発火させたキャラ</summary>
        public MapCharacter mInvoker;
        /// <summary>イベントを持っていたもの</summary>
        public MapBehaviour mInvoked;
        /// <summary>実行するイベント</summary>
        public MapEvent mRootEvent;
        public MapEventSystem parent;
        public Operator(MapEventSystem aParent,MapEvent aEvent) {
            parent = aParent;
            mRootEvent = aEvent;
        }
        /// <summary>イベント実行に必須のAIをジャック(ジャックに失敗した場合はfalse)</summary>
        public bool jackRequared() {
            if (!(mRootEvent is MapEventRoot)) return true;
            MapEventRoot tRoot = (MapEventRoot)mRootEvent;
            MapCharacter.JackedAi tAi;
            //プレイヤージャック
            if (tRoot.mJackPlayer) {
                tAi = parent.jackPlayer();
                if (tAi == null) {//ジャック失敗
                    releaseAi();
                    return false;
                }
                mAiDic.Add("player", tAi);
            }
            //その他のキャラジャック
            foreach(string tName in tRoot.mRequareAi) {
                tAi = parent.jack(tName);
                if (tAi == null) {//ジャック失敗
                    releaseAi();
                    return false;
                }
                mAiDic.Add(tName, tAi);
            }
            return true;
        }
        /// <summary>イベント実行</summary>
        public void run() {
            mRootEvent.run(this, (aArg) => {
                releaseAi();
            });
        }
        /// <summary>jackしたAIを全て解放</summary>
        public void releaseAi() {
            foreach(KeyValuePair<string,MapCharacter.JackedAi> tPair in mAiDic) {
                tPair.Value.release();
            }
        }
        /// <summary>指定名のキャラのAIをジャック</summary>
        public MapCharacter.JackedAi jack(string aName) {
            MapCharacter.JackedAi tAi = parent.jack(aName);
            if (tAi == null) return null;
            mAiDic.Add(aName, tAi);
            return tAi;
        }
    }
}
