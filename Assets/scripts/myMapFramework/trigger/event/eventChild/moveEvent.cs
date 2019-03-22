using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapEvent{
    //指定距離移動
    public class moveBy : MapEventChild{
        private List<string> mTargetCharaName;
        private Vector2 mDelta;
        private float mSpeed;
        public override void init(Arg aData){
            mTargetCharaName = aData.get<List<string>>("targetName");
            mDelta = new Vector2(aData.get<float>("deltaX"), aData.get<float>("deltaY"));
            mSpeed = aData.get<float>("speed");
        }
        public override void run(MapEvent aParent, Action aCallback){
            CallbackSystem tCS = new CallbackSystem();
            foreach(string tName in mTargetCharaName){
                MapCharacter.HijackedAi tAi = aParent.getAi(tName);
                tAi.moveBy(mDelta, mSpeed, tCS.getCounter());
            }
            tCS.then(aCallback);
        }
    }
}