using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapWorld : MyBehaviour {
    public class MapEventOperator{
        private MapWorld parent;
        private List<MapEvent> mEvents = new List<MapEvent>();
        public MapEventOperator(MapWorld aParent){
            parent = aParent;
        }
        public void addEvent(MapEvent aEvent){
            mEvents.Add(aEvent);
        }
        public void runEvents(){
            foreach(MapEvent aEvent in mEvents){
                aEvent.run(this);
            }
            mEvents.Clear();
        }
    }
}
