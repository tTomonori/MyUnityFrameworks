using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpeaker : MapBehaviour {
    public MapEvent mEvent;
    public MapEvent mEventFromUp;
    public MapEvent mEventFromDown;
    public MapEvent mEventFromLeft;
    public MapEvent mEventFromRight;
    public void speack(MapCharacter aCharacter){
        Direction tDirection = DirectionOperator.convertToDirection(aCharacter.position2D - position2D);
        MapEvent tEvent = mEvent;
        switch(tDirection){
            case Direction.up:
                if (mEventFromUp != null) tEvent = mEventFromUp;
                break;
            case Direction.down:
                if (mEventFromDown != null) tEvent = mEventFromDown;
                break;
            case Direction.left:
                if (mEventFromLeft != null) tEvent = mEventFromLeft;
                break;
            case Direction.right:
                if (mEventFromRight != null) tEvent = mEventFromRight;
                break;
        }
        if (tEvent == null) return;
        mMapWorld.mEventOperator.addEvent(tEvent);
    }
}
