using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapEvent{
    //戦闘イベント
    public class battle : MapEventChild{
        private Arg mData;
        private MapEventChild mWin;
        private MapEventChild mLose;
        public override void init(Arg aData){
            mData = aData.get<Arg>("data");
            if (aData.ContainsKey("win"))
                mWin = MapEventChild.create(aData.get<Arg>("win"));
            if (aData.ContainsKey("lose"))
                mLose = MapEventChild.create(aData.get<Arg>("lose"));
        }
        public override void run(MapEvent aParent, Action aCallback){
            MyMap.mEventHandler.onBattleStart(mData, (aResult) =>{
                if(aResult){
                    //勝利
                    if (mWin != null)
                        mWin.run(aParent, aCallback);
                    else 
                        aCallback();
                }else{
                    //敗北
                    if (mLose != null)
                        mLose.run(aParent, aCallback);
                    else
                        aCallback();
                }
            });
        }
    }
}