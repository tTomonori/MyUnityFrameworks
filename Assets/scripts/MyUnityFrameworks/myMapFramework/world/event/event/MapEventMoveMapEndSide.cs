using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventMoveMapEndSide : MapEvent {
    /// <summary>移動先のマップでの座標</summary>
    public Vector3 mPosition;
    /// <summary>移動先となりうる範囲の左下</summary>
    public Vector3? mPositionRangeLeftBottom = null;
    /// <summary>移動先となりうる範囲の右上</summary>
    public Vector3? mPositionRangeRightUp = null;
    /// <summary>移動先となりうる範囲の左下からの距離(0~1)</summary>
    public Vector2? mPercentagePosition = null;
    /// <summary>フェードイン中の移動方向(移動速度が0以下なら向きのみを設定)</summary>
    public Vector2 mMoveInVector;
    /// <summary>フェードイン中の移動速度</summary>
    public float mMoveInSpeed;
    /// <summary>マップ移動後に実行するイベントのKey(移動先のマップのKey)</summary>
    public string mDestinationEventKey;
    /// <summary>マップ移動後に実行するイベント(移動先のマップのKeyで取得したイベント)</summary>
    public MapEvent mDestinationEvent;
    /// <summary>マップ移動後に実行するイベント(移動前のマップのKeyで取得したイベント)</summary>
    public MapEvent mHereEvent;
    /// <summary>イベント終了時コールバックの退避用</summary>
    public Action<Arg> mEventEndCallback;
    public MapEventMoveMapEndSide(Arg aData) {
        mPosition = aData.ContainsKey("position") ? aData.get<Vector3>("position") : Vector3.zero;
        if (aData.ContainsKey("positionRangeLeftBottom"))
            mPositionRangeLeftBottom = aData.get<Vector3>("positionRangeLeftBottom");
        if (aData.ContainsKey("positionRangeRightUp"))
            mPositionRangeRightUp = aData.get<Vector3>("positionRangeRightUp");
        mMoveInVector = aData.get<Vector2>("moveInVector");
        mMoveInSpeed = aData.ContainsKey("moveInSpeed") ? aData.get<float>("moveInSpeed") : MyMap.mDefaultMoveSpeed;
        mDestinationEventKey = aData.ContainsKey("destinationEventKey") ? aData.get<string>("destinationEventKey") : "";
    }
    /// <summary>マップ移動後のイベント実行</summary>
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        mEventEndCallback = aCallback;
        //フェードイン完了後に実行するイベントの準備
        if (mDestinationEventKey != "" && mDestinationEventKey != null) {
            mDestinationEvent = aOperator.parent.mWorld.mEvents[mDestinationEventKey];//イベント取得
            if (!aOperator.jackRequared(mDestinationEvent)) {
                throw new Exception("MapEventMoveMapEndSide : DestinationEventが求めるAIのジャックに失敗");
            }
        }
        if (mHereEvent != null) {
            if (!aOperator.jackRequared(mHereEvent)) {
                throw new Exception("MapEventMoveMapEndSide : HereEventが求めるAIのジャックに失敗");
            }
        }

        bool tMoveEnded = false;
        bool tFadeEnded = false;
        //フェードイン移動
        if (mMoveInSpeed > 0) {
            //移動する
            aOperator.getAi("invoker").moveBy(mMoveInVector, mMoveInSpeed, () => {
                tMoveEnded = true;
                if (tFadeEnded) fadeInEnded(aOperator);
            });
        } else if (mMoveInVector != Vector2.zero) {
            //向きのみ変更
            aOperator.getAi("invoker").parent.mCharacterImage.setDirection(mMoveInVector);
            tMoveEnded = true;
        }
        //フェードイン
        aOperator.mDelegate.onMoveMapFadeIn(() => {
            tFadeEnded = true;
            if (tMoveEnded) fadeInEnded(aOperator);
        });
    }
    /// <summary>
    /// 移動先の座標計算
    /// </summary>
    /// <param name="aCollider">移動先に配置するキャラのcollider</param>
    public void calculatePositionFromPercentagePosition(Collider2D aCollider) {
        Collider2DEditer.RectangleEndPoint tSize = aCollider.minimumCircumscribedRectangleEndPoint();
        //移動先となりうる範囲
        Vector3 tLeftBottom = (Vector3)mPositionRangeLeftBottom;
        Vector3 tRightUp = (Vector3)mPositionRangeRightUp;
        tRightUp.y += -tSize.up - MapCharacterMoveSystem.kMaxSeparation;
        tLeftBottom.y += -tSize.down + MapCharacterMoveSystem.kMaxSeparation;
        tLeftBottom.x += -tSize.left + MapCharacterMoveSystem.kMaxSeparation;
        tRightUp.x += -tSize.right - MapCharacterMoveSystem.kMaxSeparation;
        //移動先となりうる範囲のサイズが負の値になったら調整
        if (tRightUp.x < tLeftBottom.x) {
            tLeftBottom.x = ((Vector3)mPositionRangeLeftBottom).x + (((Vector3)mPositionRangeRightUp).x - ((Vector3)mPositionRangeLeftBottom).x) / 2f;
            tRightUp.x = ((Vector3)mPositionRangeLeftBottom).x + (((Vector3)mPositionRangeRightUp).x - ((Vector3)mPositionRangeLeftBottom).x) / 2f;
        }
        if (tRightUp.y < tLeftBottom.y) {
            tLeftBottom.y = ((Vector3)mPositionRangeLeftBottom).y + (((Vector3)mPositionRangeRightUp).y - ((Vector3)mPositionRangeLeftBottom).y) / 2f;
            tRightUp.y = ((Vector3)mPositionRangeLeftBottom).y + (((Vector3)mPositionRangeRightUp).y - ((Vector3)mPositionRangeLeftBottom).y) / 2f;
        }
        //移動先の座標
        Vector2 tPercentagePosition = (Vector2)mPercentagePosition;
        mPosition = new Vector3(tLeftBottom.x + (tRightUp.x - tLeftBottom.x) * tPercentagePosition.x, tLeftBottom.y + (tRightUp.y - tLeftBottom.y) * tPercentagePosition.y, 0);
        mPosition.z = (tRightUp.y - tLeftBottom.y < tRightUp.x - tLeftBottom.x) ? tLeftBottom.z + (tRightUp.z - tLeftBottom.z) * tPercentagePosition.x : tLeftBottom.z + (tRightUp.z - tLeftBottom.z) * tPercentagePosition.y;
    }
    /// <summary>マップ移動のフェードアウト演出とキャラ移動演出終了時</summary>
    private void fadeInEnded(MapEventSystem.Operator aOperator) {
        if (mDestinationEvent == null && mHereEvent == null) {
            mEventEndCallback(null);
            return;
        }
        int tNum = 0;
        Action f = () => {
            --tNum;
            if (tNum <= 0)
                mEventEndCallback(null);
        };
        if (mDestinationEvent != null) {
            ++tNum;
            mDestinationEvent.run(aOperator, (arg) => { f(); });
        }
        if (mHereEvent != null) {
            ++tNum;
            mHereEvent.run(aOperator, (arg) => { f(); });
        }
    }
}
