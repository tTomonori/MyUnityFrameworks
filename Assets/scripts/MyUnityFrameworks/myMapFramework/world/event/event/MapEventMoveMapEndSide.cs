using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventMoveMapEndSide : MapEvent {
    /// <summary>移動先マップの入り口名</summary>
    public string mEntrance;
    /// <summary>マップ移動後に実行するイベントのKey(移動先のマップのKey)</summary>
    public string mDestinationEventKey;

    /// <summary>入り口データ</summary>
    public MapFileData.Entrance mEntranceData;
    /// <summary>マップ生成完了と同時に実行するイベント</summary>
    public MapEvent mFirstEvent;
    /// <summary>入り口に対応した移動演出完了時に実行するイベント</summary>
    public MapEvent mEntranceEvent;
    /// <summary>マップ移動後に実行するイベント(移動先のマップのKeyで取得したイベント)</summary>
    public MapEvent mDestinationEvent;
    /// <summary>移動先の座標</summary>
    public Vector3 mPosition;

    /// <summary>マップ移動後に実行するイベント(移動前のマップのKeyで取得したイベント)</summary>
    public MapEvent mHereEvent;
    /// <summary>移動前のマップでの出口に対する相対座標(0 ~ 1)</summary>
    public Vector3 mPercentagePosition;

    /// <summary>イベント終了時コールバックの退避用</summary>
    public Action<Arg> mEventEndCallback;
    /// <summary>マップ移動イベントと同時に実行中のイベント数</summary>
    public int mRunningEventNum = 0;
    /// <summary>マップ移動イベントと同時に実行するイベントの実行指示が完了</summary>
    public bool mAllCompanionEventIsStarted = false;

    public MapEventMoveMapEndSide(Arg aData) {
        mEntrance = aData.get<string>("entrance");
        mDestinationEventKey = aData.ContainsKey("destinationEventKey") ? aData.get<string>("destinationEventKey") : "";
    }
    /// <summary>マップ移動後のイベント実行</summary>
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        mEventEndCallback = aCallback;
        //マップ生成完了と同時に実行するイベントの実行
        if (mEntranceData.mFirstEventKey != "") {
            mFirstEvent = aOperator.parent.mWorld.mEvents[mEntranceData.mFirstEventKey];//イベント取得
            if (!aOperator.jackRequared(mFirstEvent)) {
                throw new Exception("MapEventMoveMapEndSide : FirstEventが求めるAIのジャックに失敗");
            }
            //実行
            ++mRunningEventNum;
            mFirstEvent.run(aOperator, (arg) => { endCompanionEvent(); });
        }
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
        if (mEntranceData.mEventKey != "") {
            mEntranceEvent = aOperator.parent.mWorld.mEvents[mEntranceData.mEventKey];//イベント取得
            if (!aOperator.jackRequared(mEntranceEvent)) {
                throw new Exception("MapEventMoveMapEndSide : EntranceEventが求めるAIのジャックに失敗");
            }
        }

        bool tMoveEnded = false;
        bool tFadeEnded = false;
        //フェードイン移動
        if (mEntranceData.mMoveSpeed > 0) {
            //移動する
            aOperator.getAi("invoker").moveBy(mEntranceData.mMoveVector, mEntranceData.mMoveSpeed, () => {
                tMoveEnded = true;
                if (tFadeEnded) fadeInEnded(aOperator);
            });
        } else if (mEntranceData.mMoveVector != Vector2.zero) {
            //向きのみ変更
            aOperator.getAi("invoker").parent.mCharacterImage.setDirection(mEntranceData.mMoveVector);
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
    public void calculatePositionFromPercentagePosition(Collider aCollider) {
        ColliderEditer.CubeEndPoint tSize = aCollider.minimumCircumscribedCubeEndPoint();
        //移動先となりうる範囲
        Vector3 tLeftFrontBottom = new Vector3(mEntranceData.mX - mEntranceData.mSize.x / 2f, mEntranceData.mY - mEntranceData.mSize.y / 2f, mEntranceData.mZ - mEntranceData.mSize.z / 2f);
        Vector3 tRightBackTop = new Vector3(mEntranceData.mX + mEntranceData.mSize.x / 2f, mEntranceData.mY + mEntranceData.mSize.y / 2f, mEntranceData.mZ + mEntranceData.mSize.z / 2f);
        tLeftFrontBottom.x -= tSize.left + MapCharacterMoveSystem.kMaxSeparation;
        tRightBackTop.x -= tSize.right - MapCharacterMoveSystem.kMaxSeparation;
        tLeftFrontBottom.y -= tSize.bottom - MapCharacterMoveSystem.kMaxSeparation;
        tRightBackTop.y -= tSize.top + MapCharacterMoveSystem.kMaxSeparation;
        tLeftFrontBottom.z -= tSize.front - MapCharacterMoveSystem.kMaxSeparation;
        tRightBackTop.z -= tSize.back + MapCharacterMoveSystem.kMaxSeparation;
        //移動先となりうる範囲のサイズが負の値になったら調整
        if (tRightBackTop.x < tLeftFrontBottom.x) {
            tLeftFrontBottom.x = mEntranceData.mX;
            tRightBackTop.x = mEntranceData.mX;
        }
        if (tRightBackTop.y < tLeftFrontBottom.y) {
            tLeftFrontBottom.y = mEntranceData.mY;
            tRightBackTop.y = mEntranceData.mY;
        }
        if (tRightBackTop.z < tLeftFrontBottom.z) {
            tLeftFrontBottom.z = mEntranceData.mZ;
            tRightBackTop.z = mEntranceData.mZ;
        }
        //移動先の座標
        mPosition = new Vector3(tLeftFrontBottom.x + (tRightBackTop.x - tLeftFrontBottom.x) * mPercentagePosition.x, tLeftFrontBottom.y + (tRightBackTop.y - tLeftFrontBottom.y) * mPercentagePosition.y, tLeftFrontBottom.z + (tRightBackTop.z - tLeftFrontBottom.z) * mPercentagePosition.z);
    }
    /// <summary>マップ移動のフェードアウト演出とキャラ移動演出終了時</summary>
    private void fadeInEnded(MapEventSystem.Operator aOperator) {
        //入り口データが指定するイベント実行
        if (mEntranceEvent != null) {
            ++mRunningEventNum;
            mEntranceEvent.run(aOperator, (arg) => { endCompanionEvent(); });
        }
        //マップ移動イベントが指定するイベント実行
        if (mDestinationEvent != null) {
            ++mRunningEventNum;
            mDestinationEvent.run(aOperator, (arg) => { endCompanionEvent(); });
        }
        //移動前のマップに定義されているイベント実行
        if (mHereEvent != null) {
            ++mRunningEventNum;
            mHereEvent.run(aOperator, (arg) => { endCompanionEvent(); });
        }
        //マップ生成と同時に実行するイベントのみ実行し、かつこの時既に実行が完了していた場合の対策
        mAllCompanionEventIsStarted = true;
        ++mRunningEventNum;
        endCompanionEvent();
    }
    /// <summary>マップ移動イベントと同時に実行するイベントの一つが実行完了</summary>
    private void endCompanionEvent() {
        --mRunningEventNum;
        if (mAllCompanionEventIsStarted && mRunningEventNum <= 0) {
            mEventEndCallback(null);
        }
    }
}
