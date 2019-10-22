using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public class JackedAi : Ai {
        public MapCharacter mCharacter {
            get { return parent; }
        }
        public override void update() {

        }
        /// <summary>AIジャックを終了する</summary>
        public void release() {
            parent.endJack();
            parent = null;
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
