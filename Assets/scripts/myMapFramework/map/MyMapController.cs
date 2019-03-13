using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMap : MyBehaviour {
    public class MyMapController{
        private MyMap parent;
        public MyMapController(MyMap aParent){
            parent = aParent;
        }
    }
}
