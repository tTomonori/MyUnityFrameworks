using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventMoveMap : MapEvent {
    /// <summary>移動先のマップファイルのパス</summary>
    public string mMapPath;
    /// <summary>フェードアウト中の移動方向(移動速度が0以下なら向きのみを変える)</summary>
    public Vector2 mMoveOutVector;
    /// <summary>フェードアウト中の移動速度</summary>
    public float mMoveOutSpeed;
    /// <summary>マップ移動後に実行するイベントのKey(移動前のマップのKey)</summary>
    public string mHereEventKey;
    /// <summary>マップ移動完了後に実行するイベント</summary>
    public MapEventMoveMapEndSide mEndSide;
    /// <summary>フェードアウト完了時のプレイヤーの向き</summary>
    public Vector2 mPlayerDirection;
    public MapEventMoveMap(Arg aData) {
        mEndSide = new MapEventMoveMapEndSide(aData);
        mMapPath = aData.get<string>("mapPath");
        mMoveOutVector = aData.get<Vector2>("moveOutVector");
        mMoveOutSpeed = aData.ContainsKey("moveOutSpeed") ? aData.get<float>("moveOutSpeed") : MyMap.mDefaultMoveSpeed;
        mHereEventKey = aData.ContainsKey("hereEventKey") ? aData.get<string>("hereEventKey") : "";
    }
    /// <summary>マップ移動前のイベント実行</summary>
    public override void run(MapEventSystem.Operator aOperator, Action<Arg> aCallback) {
        //移動先の座標計算
        calculatePercentagePosition(aOperator);
        //マップ移動通知
        aOperator.mDelegate.onMoveMap(this);
        //現在のマップのイベントKeyに対応したイベント取得
        if (mHereEventKey != null && mHereEventKey != "")
            mEndSide.mHereEvent = aOperator.parent.mWorld.mEvents[mHereEventKey];
        //フェードアウト移動
        if (mMoveOutSpeed > 0) {
            //移動する
            aOperator.getAi("invoker").moveBy(mMoveOutVector, mMoveOutSpeed, () => { });
        } else if (mMoveOutVector != Vector2.zero) {
            //向きのみ変更
            aOperator.getAi("invoker").parent.mCharacterImage.setDirection(mMoveOutVector);
        }
        //フェードアウト
        aOperator.mDelegate.onMoveMapFadeOut(() => { fadeOutEnded(aOperator); });
    }
    /// <summary>移動先の座標計算</summary>
    private void calculatePercentagePosition(MapEventSystem.Operator aOperator) {
        //triggerの矩形範囲
        Collider2DEditer.RectangleEndPoint tRange = aOperator.mInvokedCollider.minimumCircumscribedRectangleEndPoint();
        //invokerのcolliderを考慮して矩形範囲を調整
        Collider2DEditer.RectangleEndPoint tSize = aOperator.mInvoker.mAttribute.mCollider.minimumCircumscribedRectangleEndPoint();
        tRange.up -= tSize.up;
        tRange.down -= tSize.down;
        tRange.left -= tSize.left;
        tRange.right -= tSize.right;


        Vector2 tRelative = aOperator.mInvoker.worldPosition2D - aOperator.mInvokedCollider.transform.position.toVector2();
        Vector2 tRangePosition = new Vector2((tRelative.x - tRange.left) / (tRange.right - tRange.left), (tRelative.y - tRange.down) / (tRange.up - tRange.left));
        //座標が(0~1)の範囲になるように調整
        if (tRangePosition.x < 0) tRangePosition.x = 0;
        else if (1 < tRangePosition.x) tRangePosition.x = 1;
        if (tRangePosition.y < 0) tRangePosition.y = 0;
        else if (1 < tRangePosition.y) tRangePosition.y = 1;

        mEndSide.mPercentagePosition = tRangePosition;
    }
    /// <summary>マップ移動のフェードアウト演出終了時</summary>
    private void fadeOutEnded(MapEventSystem.Operator aOperator) {
        mPlayerDirection = aOperator.getCharacter("player").mCharacterImage.getDirection();
        aOperator.parent.mWorld.mMap.moveMap(this);
    }
}
