using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapEventSystem {
    public partial class Operator {
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
        /// <summary>イベントを持っていたcollider</summary>
        public Collider2D mInvokedCollider;
        /// <summary>実行するイベント</summary>
        public MapEvent mRootEvent;
        public MapEventSystem parent;
        public Operator(MapEventSystem aParent, MapEvent aEvent) {
            parent = aParent;
            mRootEvent = aEvent;
        }
        /// <summary>イベント実行に必須のAIをジャック(ジャックに失敗した場合はfalse)</summary>
        public bool jackRequared() {
            return jackRequared(mRootEvent);
        }
        /// <summary>引数のEventが必要とするAIをジャック(ジャックに失敗した場合はfalse)</summary>
        public bool jackRequared(MapEvent aEvent) {
            //rootEventでない場合はジャックする必要なし
            if (!(mRootEvent is MapEventRoot)) return true;

            return jackRequared((MapEventRoot)mRootEvent);
        }
        /// <summary>引数のRootEventが必要とするAIをジャック(ジャックに失敗した場合はfalse)</summary>
        public bool jackRequared(MapEventRoot aRoot) {
            //Invokerジャック
            if (aRoot.mJackInvoker) {
                if (!jack("invoker")) {
                    return false;
                }
            }
            //Invokedジャック
            if (aRoot.mJackInvoked) {
                if (!jack("invoked")) {
                    return false;
                }
            }
            //その他のキャラジャック
            foreach (string tName in aRoot.mRequareAi) {
                if (!jack(tName)) {
                    return false;
                }
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
            //予約名は先にreleaseする
            foreach (string tName in new string[] { "invoker", "invoked", "player" }) {
                if (!mAiDic.ContainsKey(tName)) continue;
                MapCharacter.JackedAi tAi = mAiDic[tName];
                //予約名とキャラ名が異なる場合
                if (tAi.parent.mName != tName) {
                    mAiDic.Remove(tAi.parent.mName);
                }
                //release
                tAi.release();
                mAiDic.Remove(tName);
            }
            //予約名以外
            foreach (KeyValuePair<string, MapCharacter.JackedAi> tPair in mAiDic) {
                tPair.Value.release();
            }
        }
    }
}
