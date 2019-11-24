using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapOrnament : MapEntity {
    public MapFileData.Ornament mFileData;
    [SerializeField] public MapEntityRenderBehaviour mOrnamentRenderBehaviour;
    public override MapRenderBehaviour mRenderBehaviour {
        get { return mOrnamentRenderBehaviour; }
        set { mOrnamentRenderBehaviour = (MapEntityRenderBehaviour)value; }
    }
    public override MapEntityRenderBehaviour mEntityRenderBehaviour {
        get { return mOrnamentRenderBehaviour; }
        set { mOrnamentRenderBehaviour = value; }
    }

    /// <summary>ornamentを復元する時に必要となるデータ</summary>
    public virtual Arg save() {
        return null;
    }
    /// <summary>変数適用</summary>
    public virtual void setArg(Arg aData) {

    }
}

[CustomEditor(typeof(MapOrnament))]
public class InspectorOfMapOrnament : InspectorOfMapBehaviour {
    public MapOrnament mOrnament { get => target as MapOrnament; }
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
        MapEntityRenderBehaviour tRender = mOrnament.createChild<MapEntityRenderBehaviour>("render");
        mOrnament.mEntityRenderBehaviour = tRender;

        MapEntityImage tImage = tRender.createChild<MapEntityImage>("image");
    }
    public void createPhysics() {
        MapEntityPhysicsBehaviour tPhysics = mOrnament.createChild<MapEntityPhysicsBehaviour>("physics");
        mOrnament.mEntityPhysicsBehaviour = tPhysics;
        //属性
        EntityPhysicsAttribute tAttribute = tPhysics.createChild<EntityPhysicsAttribute>("attribute");
        tAttribute.mAttribute = EntityPhysicsAttribute.Attribute.ornament;
        tAttribute.mEntity = mOrnament;
        tPhysics.mAttribute = tAttribute;
        //足場rigide
        MapScaffoldRigide tRigide = tAttribute.gameObject.AddComponent<MapScaffoldRigide>();
        tPhysics.mScaffoldRigide = tRigide;
    }
}