using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
    up,down,left,right,none
}
public enum VHDirection{
    horizontal,vertical,none
}

public static class DirectionOperator{
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
    static public Vector2 convertToVector(Direction aDirection){
        switch(aDirection){
            case Direction.up:return new Vector2(0, 1);
            case Direction.down:return new Vector2(0, -1);
            case Direction.left:return new Vector2(-1, 0);
            case Direction.right:return new Vector2(1, 0);
        }
        return new Vector2();
    }
    static public VHDirection convertToAxis(Direction aDirection){
        switch (aDirection){
            case Direction.up: return VHDirection.vertical;
            case Direction.down: return VHDirection.vertical;
            case Direction.left: return VHDirection.horizontal;
            case Direction.right: return VHDirection.horizontal;
        }
        return VHDirection.none;
    }
    /// <summary>
    /// ベクトルを成分分解する
    /// </summary>
    /// <returns>成分方向ベクトルに垂直な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector2 disassemble(Vector2 aVector,Vector2 aComponent){
        //成分ベクトルに直角
        Vector2 tRightAngleVector = new Vector2(-aComponent.y, aComponent.x);
        float k = (aVector.x * aComponent.y - aVector.y * aComponent.x) / (tRightAngleVector.x * aComponent.y - tRightAngleVector.y * aComponent.x);
        //成分ベクトルに直角な方向の成分
        Vector2 tARightAngleVector = k * tRightAngleVector;
        return tARightAngleVector;
    }
    static public Vector2 randomVector(){
        Vector2 vector = new Vector2(0, 1);
        return Quaternion.Euler(0, 0, Random.Range(0,359)) * vector;
    }
}