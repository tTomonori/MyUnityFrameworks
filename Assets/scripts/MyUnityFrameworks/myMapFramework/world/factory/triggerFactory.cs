using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    //<summary>トリガー生成</summary>
    static public MapTrigger createTrigger(MapFileData.Trigger aData) {
        MapTrigger tTrigger = MyBehaviour.create<MapTrigger>();

        //形状
        MyTag tShapeTag = aData.mShape;
        switch (tShapeTag.mTagName) {
            case "square"://四角形
                BoxCollider2D tBox = tTrigger.gameObject.AddComponent<BoxCollider2D>();
                tBox.size = new Vector2(float.Parse(tShapeTag.mArguments[0]), float.Parse(tShapeTag.mArguments[1]));
                break;
            default:
                throw new System.Exception("MapWorldFactory-TriggerFactory : 不正な形状名「" + tShapeTag.mTagName + "」");
        }

        return tTrigger;
    }
    //<summary>トリガーを生成してworldに追加</summary>
    static private void buildTrigger(MapFileData.Trigger aData) {
        MapTrigger tTrigger = createTrigger(aData);
        mWorld.addTrigger(tTrigger, aData.mName, aData.mX, aData.mY, aData.mStratum);
    }
}
