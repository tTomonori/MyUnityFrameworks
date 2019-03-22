using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpeaker : MapBehaviour {
    public MapEvent mEvent;
    public void speack(){
        mMapWorld.mEventOperator.addEvent(mEvent);
    }
}
