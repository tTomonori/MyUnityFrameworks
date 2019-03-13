using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    /// <summary>
    /// 指定したパスのプレハブを生成
    /// </summary>
    /// <returns>生成したプレハブがもつComponent</returns>
    /// <param name="aFilePath">プレハブへのパス("mymap/" + X)</param>
    /// <typeparam name="Type">取得するComponent</typeparam>
    public static Type createFromMapResource<Type>(string aFilePath){
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("mymap/" + aFilePath);
        // プレハブからインスタンスを生成
        return Instantiate(prefab).GetComponent<Type>();
    }
    public void setPositionMaintainZ(float aX,float aY){
        position = new Vector3(aX, aY, positionZ);
    }
    public void setPosition(float aX,float aY){
        position = new Vector3(aX, aY, aY);
    }
    public void foceMoveBy(Vector2 aVector){
        position = new Vector3(positionX + aVector.x, positionY + aVector.y, positionY + aVector.y);
    }
}
