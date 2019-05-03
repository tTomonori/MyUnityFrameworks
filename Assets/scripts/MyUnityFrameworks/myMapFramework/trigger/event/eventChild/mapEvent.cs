using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapEvent{
    //マップを移動
    public class moveMap : MapEventChild{
        private Arg mData;
        private string mMapName;
        private Vector2 mPosition;
        private int mStratumNum;
        public override void init(Arg aData){
            mData = aData;
            mMapName = aData.get<string>("mapName");
            mPosition = new Vector2(aData.get<float>("positionX"), aData.get<float>("positionY"));
            mStratumNum = aData.get<int>("stratum");
        }
        public override void run(MapEvent aParent, Action aCallback){
            MyMap.MoveMapOptions tOption = new MyMap.MoveMapOptions();
            tOption.mMapName = mMapName;
            tOption.mPlayerOption = new MyMap.MoveMapOptions.PlayerOption();
            tOption.mPlayerOption.mPosition = mPosition;
            tOption.mPlayerOption.mStratumNum = mStratumNum;
            //マップ移動イベント発火通知
            MyMap.mEventHandler.onMoveMap(mData, () =>{
                //フェードアウト完了
                MyMap.moveMap(tOption, ()=>{
                    //新しいマップ生成完了
                    MyMap.mEventHandler.onCreatedMap(aCallback);
                });
            });
        }
    }
}