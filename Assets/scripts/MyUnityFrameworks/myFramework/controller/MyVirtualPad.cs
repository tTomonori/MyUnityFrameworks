using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVirtualPad : MyPad {
    protected void OnMouseDrag(){
        mouseDrag();
    }
    protected void OnMouseDown(){
        mouseDown();
    }
    protected void OnMouseUp(){
        mouseUp();
    }
}
