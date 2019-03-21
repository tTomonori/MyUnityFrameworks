using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapEvent {
    private Arg mData;
    private MapEventChild mEvent;
    private MapWorld.MapEventOperator mEventOperator;
    //現在イベント実行中か
    private bool mIsRunning = false;
    public MapEvent(Arg aData){
        mData = aData;
    }
    public MapEvent(MapEventChild aEvent){
        mEvent = aEvent;
    }
    public void run(MapWorld.MapEventOperator aOperator){
        if (mIsRunning) return;
        mIsRunning = true;
        mEventOperator = aOperator;
        mEvent.run(this,() => {
            mIsRunning = false;
        });
    }
    public abstract class MapEventChild{
        public abstract void run(MapEvent aParent, Action aCallback);
        public MapEventChild create(Arg aData){
            Type tType = Type.GetType("MapEvent."+aData.get<string>("event"));
            MapEventChild tEvent = (MapEventChild)tType.GetConstructor(new Type[]{aData.GetType()}).Invoke(new object[] { aData });
            return tEvent;
        }
    }
    public class function:MapEventChild{
        private Action<Action> mFunction;
        public function(Action<Action> aFunction){
            mFunction = aFunction;
        }
        public override void run(MapEvent aParent, Action aCallback){
            mFunction(aCallback);
        }
    }
}
