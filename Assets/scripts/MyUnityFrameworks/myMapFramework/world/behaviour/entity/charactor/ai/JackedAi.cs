using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public class JackedAi : Ai {
        /// <summary>updateで呼ぶ関数の型(falseならリストから削除)</summary>
        delegate bool UpdateFunc();
        /// <summary>updateで呼ぶ関数のリスト</summary>
        private List<UpdateFunc> mUpdateFuncList = new List<UpdateFunc>();
        /// <summary>このAIを持つキャラ</summary>
        public MapCharacter mCharacter {
            get { return parent; }
        }
        public override void update() {
            for(int i = 0; i < mUpdateFuncList.Count; ++i) {
                if (mUpdateFuncList[i]()) continue;
                mUpdateFuncList.RemoveAt(i);
                --i;
            }
        }
        /// <summary>AIジャックを終了する</summary>
        public void release() {
            parent.endJack();
            parent = null;
        }

        /// <summary>振り向く</summary>
        public void turn(Vector2 aDirection) {
            parent.mCharacterImage.setDirection(aDirection);
        }
        /// <summary>指定距離移動</summary>
        public void moveBy(Vector2 aVector,float aSpeed,Action aOnEnd) {
            float tRemainedDistance = aVector.magnitude;
            mUpdateFuncList.Add(() => {
                if (tRemainedDistance <= 0) {
                    //移動完了
                    aOnEnd();
                    return false;
                }
                //移動入力
                float tDistance = aSpeed * Time.deltaTime;
                if (tRemainedDistance < tDistance) tDistance = tRemainedDistance;
                parent.mState.move(aVector,tDistance);

                tRemainedDistance -= tDistance;
                return true;
            });
        }
    }

    /// <summary>AI乗っ取りされる前のAI</summary>
    private Ai mOriginalAi;
    /// <summary>AI乗っ取られ中ならtrue</summary>
    private bool mIsJacked = false;
    /// <summary>AI操作を乗っ取る</summary>
    public JackedAi jack() {
        if (mIsJacked) return null;
        mOriginalAi = mAi;
        JackedAi tAi = new JackedAi();
        tAi.parent = this;
        mAi = tAi;
        return tAi;
    }
    /// <summary>AIジャックを終了する</summary>
    private void endJack() {
        mAi = mOriginalAi;
        mOriginalAi = null;
    }
    /// <summary>AI操作を乗っ取りできるか(可能ならtrue)</summary>
    public bool canJack() {
        return !mIsJacked;
    }
}
