using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class MapCharacter : MapEntity {
    public MapFileData.Character mFileData;
    /// <summary>移動処理で使うデータ</summary>
    public MovingData mMovingData;
    public MapCharacterImage mCharacterImage {
        get { return mCharacterRenderBehaviour.mImage; }
    }

    [SerializeField] public MapCharacterRenderBehaviour mCharacterRenderBehaviour;
    public override MapRenderBehaviour mRenderBehaviour {
        get { return mCharacterRenderBehaviour; }
        set { mCharacterRenderBehaviour = (MapCharacterRenderBehaviour)value; }
    }
    public override MapEntityRenderBehaviour mEntityRenderBehaviour {
        get { return mCharacterRenderBehaviour; }
        set { mCharacterRenderBehaviour = (MapCharacterRenderBehaviour)value; }
    }

    private MapCharacter.Ai mAi;
    private MapCharacter.State mState;

    /// <summary>AI復元する時に必要となるデータ</summary>
    public string saveAi() {
        return mAi.save();
    }
    /// <summary>Stateを復元する時に必要となるデータ</summary>
    public string saveState() {
        return mState.save();
    }

    //<summary>MapWorld内に配置された直後に呼ばれる</summary>
    public override void placed() {
        MapHeightUpdateSystem.updateHeight(this);
        mMovingData.mPrePosition = mMapPosition;
        mMovingData.mDeltaPrePosition = mMovingData.mPrePosition;
        MapTriggerUpdater.initTriggerDataOfMovingData(this);
    }
    //状態遷移
    public void transitionState(MapCharacter.State aState) {
        if (mState != null)
            mState.exit();
        aState.parent = this;
        mState = aState;
        mState.enter();
    }
    //Ai設定
    public void setAi(MapCharacter.Ai aAi) {
        aAi.parent = this;
        mAi = aAi;
    }

    //更新
    public void updateInternalState() {
        mAi.update();
        mState.update();
    }
    //プレイヤーが操作するキャラかどうか
    public bool isPlayer() {
        return mAi is MapCharacter.PlayerAi || mOriginalAi is MapCharacter.PlayerAi;
    }
    //操作状態
    public Operation getOperation() {
        if (mAi is JackedAi) return Operation.jacked;
        if (mState is StandingState) return Operation.free;
        if (mState is WalkingState) return Operation.free;
        return Operation.busy;
    }
    public enum Operation {
        free, jacked, busy
    }
}

[CustomEditor(typeof(MapCharacter))]
public class InspectorOfMapCharacter : InspectorOfMapBehaviour {
    public MapCharacter mCharacter { get => target as MapCharacter; }
    public override void OnInspectorGUI() {
        //元のInspector部分を表示
        base.OnInspectorGUI();
        //ボタンを表示
        if (GUILayout.Button("createTemplate")) {
            evacuate();
            createRender();
            createPhysics();
        }
    }
    public void createRender() {
        MapCharacterRenderBehaviour tRender = mCharacter.createChild<MapCharacterRenderBehaviour>("render");
        mCharacter.mCharacterRenderBehaviour = tRender;

        //body
        tRender.createChild("body");
        //shadow
        tRender.createChild("shadow");
    }
    public void createPhysics() {
        MapEntityPhysicsBehaviour tPhysics = mCharacter.createChild<MapEntityPhysicsBehaviour>("physics");
        mCharacter.mPhysicsBehaviour = tPhysics;
        //属性
        EntityPhysicsAttribute tAttribute = tPhysics.createChild<EntityPhysicsAttribute>("attribute");
        tAttribute.mAttribute = EntityPhysicsAttribute.Attribute.walking;
        tAttribute.mEntity = mCharacter;
        tPhysics.mAttribute = tAttribute;
        //属性collider
        BoxCollider tAttributeCollider = tAttribute.gameObject.AddComponent<BoxCollider>();
        tAttributeCollider.size = new Vector3(0.6f, 0.9f, 0.3f);
        tAttributeCollider.center = new Vector3(0, 0.45f, 0.15f);

        //足場rigide
        MapScaffoldRigide tRigide = tPhysics.createChild<MapScaffoldRigide>("scaffoldRigide");
        tPhysics.mScaffoldRigide = tRigide;
        //足場rigide collider
        BoxCollider tRidigeCollider = tRigide.gameObject.AddComponent<BoxCollider>();
        tRidigeCollider.size = new Vector3(0.6f, 0.9f, 0.01f);
        tRidigeCollider.center = new Vector3(0, 0.45f, 0.005f);
    }
}