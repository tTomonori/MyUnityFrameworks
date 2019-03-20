using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapWorld : MyBehaviour {
    public class MapEventOperator{
        private MapWorld parent;
        public MapEventOperator(MapWorld aParent){
            parent = aParent;
        }
        public void run(MapEvent aEvent,Action aCallback){
            
        }
    }
}
