using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapCharacterImage : MapStandImage {
    //<summary>移動方向によって画像を変更</summary>
    abstract public void moved(Vector2 aVector);
    //<summary>向きを設定</summary>
    abstract public void setDirection(Vector2 aVector);
    //<summary>現在の向きを取得</summary>
    abstract public Vector2 getDirection();
}
