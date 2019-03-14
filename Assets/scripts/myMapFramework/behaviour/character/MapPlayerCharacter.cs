using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayerCharacter : MapBehaviour {
    public Vector2? mMoveDirection;
    public bool mInputA;
    public bool mInputB;
    private void LateUpdate(){
        mMoveDirection = null;
        mInputA = false;
        mInputB = false;
    }
}
