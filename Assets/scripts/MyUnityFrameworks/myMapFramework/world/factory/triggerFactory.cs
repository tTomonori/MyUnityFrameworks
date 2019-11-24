using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MapWorldFactory {
    //<summary>トリガー生成</summary>
    static public MapTrigger createTrigger(MapFileData.Trigger aData) {
        MapKeyEventTrigger tTrigger = MyBehaviour.create<MapKeyEventTrigger>();

        //形状
        MyTag tShapeTag = aData.mShape;
        switch (tShapeTag.mTagName) {
            case "cube"://立方体
                BoxCollider tBox = tTrigger.gameObject.AddComponent<BoxCollider>();
                tBox.size = new Vector3(float.Parse(tShapeTag.mArguments[0]), float.Parse(tShapeTag.mArguments[1]), float.Parse(tShapeTag.mArguments[1]));
                tBox.center = new Vector3(0, tBox.size.y / 2f, 0);
                break;
            default:
                throw new System.Exception("MapWorldFactory-TriggerFactory : 不正な形状名「" + tShapeTag.mTagName + "」");
        }

        //triggerKey
        tTrigger.mTriggerKey = aData.mTriggerKey;
        //eventKey
        tTrigger.mEnterKey = aData.mEnterKey;
        tTrigger.mStayKey = aData.mStayKey;
        tTrigger.mMovedKey = aData.mMovedKey;
        tTrigger.mExitKey = aData.mExitKey;
        //collisionType
        switch (aData.mCollisionType) {
            case "pass":
                tTrigger.mCollisionType = MapPhysics.CollisionType.pass;
                break;
            case "stop":
                tTrigger.mCollisionType = MapPhysics.CollisionType.stop;
                break;
            case "collide":
                tTrigger.mCollisionType = MapPhysics.CollisionType.collide;
                break;
            default:
                Debug.LogWarning("triggerFactory : 不正なcollisionType「" + aData.mCollisionType + "」");
                break;
        }
        //attribute
        TriggerPhysicsAttribute tAttribute = tTrigger.gameObject.AddComponent<TriggerPhysicsAttribute>();
        tAttribute.mTrigger = tTrigger;

        return tTrigger;
    }
    //<summary>トリガーを生成してworldに追加</summary>
    static private MapTrigger buildTrigger(MapFileData.Trigger aData) {
        //生成フラグ確認
        if (!flagCreate(aData)) return null;

        MapTrigger tTrigger = createTrigger(aData);

        MapBehaviour tBehaviour = MyBehaviour.create<MapBehaviour>();
        tBehaviour.name = aData.mName;

        MapPhysicsBehaviour tPhysics = tTrigger.gameObject.AddComponent<MapPhysicsBehaviour>();
        tPhysics.transform.SetParent(tBehaviour.transform, false);
        tBehaviour.mPhysicsBehaviour = tPhysics;
        tTrigger.mBehaviour = tBehaviour;
        tBehaviour.mMapPosition = new MapPosition(aData.mPosition);

        tBehaviour.transform.SetParent(mWorld.mTriggerContainer.transform, false);
        return tTrigger;
    }
}
