using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
    up,down,left,right,none
}
public enum DirectionVH{
    horizontal,vertical,none
}
public enum DirectionX{
    leftUp,leftDown,rightUp,rightDown,none
}
public enum DirectionH{
    left,right,none
}

public static class DirectionOperator{
    ///上下左右
    //<summary>ベクトルを上下左右の4方向に変換</summary>
    static public Direction convertToDirection(Vector2 aVector){
        if(Mathf.Abs(aVector.x)>Mathf.Abs(aVector.y)){
            if (0 < aVector.x) return Direction.right;
            else return Direction.left;
        }
        else{
            if (0 < aVector.y) return Direction.up;
            else return Direction.down;
        }
    }
    //<summary>十字4方向をそれぞれ単位ベクトルに変換</summary>
    static public Vector2 convertToVector(Direction aDirection){
        switch(aDirection){
            case Direction.up:return new Vector2(0, 1);
            case Direction.down:return new Vector2(0, -1);
            case Direction.left:return new Vector2(-1, 0);
            case Direction.right:return new Vector2(1, 0);
        }
        return new Vector2(0, 0);
    }
    ///水平垂直
    //<summary>ベクトルを水平垂直の2方向に変換</summary>
    static public DirectionVH convertToDirectionVH(Vector2 aVector){
        if (aVector.x == 0 && aVector.y == 0)
            return DirectionVH.none;
        if (Mathf.Abs(aVector.x) > Mathf.Abs(aVector.y))
            return DirectionVH.horizontal;
        else
            return DirectionVH.vertical;
    }
    //<summary>水平垂直の2方向をそれぞれ単位ベクトルに変換</summary>
    static public Vector2 convertToVector(DirectionVH aDirection){
        switch(aDirection){
            case DirectionVH.horizontal:return new Vector2(1, 0);
            case DirectionVH.vertical:return new Vector2(0, 1);
        }
        return new Vector2(0, 0);
    }
    ///X字
    //<summary>ベクトルをX字の4方向に変換</summary>
    static public DirectionX convertToDirectionX(Vector2 aVector){
        if (aVector.x == 0 && aVector.y == 0)
            return DirectionX.none;
        if (aVector.x < 0){
            if (aVector.y < 0) return DirectionX.leftDown;
            else return DirectionX.leftUp;
        }
        else{
            if (aVector.y < 0) return DirectionX.rightDown;
            else return DirectionX.rightUp;
        }
    }
    //<summary>X字4方向をそれぞれ単位ベクトルに変換</summary>
    static public Vector2 convertToVector(DirectionX aDirection){
        switch(aDirection){
            case DirectionX.leftUp:return (new Vector2(-1, 1)).normalized;
            case DirectionX.leftDown:return (new Vector2(-1, -1)).normalized;
            case DirectionX.rightUp: return (new Vector2(1, 1)).normalized;
            case DirectionX.rightDown: return (new Vector2(1, -1)).normalized;
        }
        return new Vector2(0, 0);
    }
    ///左右
    //<summary>ベクトルを左右の2方向に変換</summary>
    static public DirectionH convertToDirectionH(Vector2 aVector){
        if (aVector.x == 0)
            return DirectionH.none;
        if (aVector.x < 0)
            return DirectionH.left;
        else
            return DirectionH.right;
    }
    //<summary>左右の2方向をそれぞれ単位ベクトルに変換</summary>
    static public Vector2 convertToVector(DirectionH aDirection){
        switch(aDirection){
            case DirectionH.left:return new Vector2(-1, 0);
            case DirectionH.right:return new Vector2(1, 0);
        }
        return new Vector2(0, 0);
    }
    //<summary>十字4方向を水平垂直2方向に変換する</summary>
    static public DirectionVH convertToAxis(Direction aDirection){
        switch (aDirection){
            case Direction.up: return DirectionVH.vertical;
            case Direction.down: return DirectionVH.vertical;
            case Direction.left: return DirectionVH.horizontal;
            case Direction.right: return DirectionVH.horizontal;
        }
        return DirectionVH.none;
    }
}