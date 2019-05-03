using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class MyMapInputPad : MyBehaviour {
    public MyMap.MyMapController mController;
    private Vector2 mDragStartingPoint;
    private bool mDragFlag = false;
    private void OnMouseDrag(){
        Vector2 tMousePosition = Input.mousePosition;
        if (mDragStartingPoint.x == tMousePosition.x && mDragStartingPoint.y == tMousePosition.y) return;

        mDragFlag = true;
        if (mController != null)
            mController.move(tMousePosition - mDragStartingPoint);
    }
    private void OnMouseDown(){
        mDragFlag = false;
        mDragStartingPoint = Input.mousePosition;
    }
    private void OnMouseUp(){
        if (mDragFlag) return;
        if (mController != null)
            mController.inputA();
    }
    private void OnMouseExit(){
        mDragFlag = true;
    }
}
