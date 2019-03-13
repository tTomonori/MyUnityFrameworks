using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MyMap : MyBehaviour {
    public class MapEventOperator{
        private MyMap parent;
        public MapEventOperator(MyMap aParent){
            parent = aParent;
        }
        public void run(MapEvent aEvent,Action aCallback){
            
        }
    }
}
