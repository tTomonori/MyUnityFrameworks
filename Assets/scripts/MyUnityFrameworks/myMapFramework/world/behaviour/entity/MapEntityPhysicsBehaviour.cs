using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEntityPhysicsBehaviour : MapPhysicsBehaviour {
    /// <summary>物理属性</summary>
    public EntityPhysicsAttribute mAttribute;
    /// <summary>物理属性のcollider</summary>
    public Collider mAttriubteCollider;
    /// <summary>足場と衝突する属性</summary>
    public MapScaffoldRigide mScaffoldRigide;
    /// <summary>足場と衝突する属性のcollider</summary>
    public Collider mScaffoldRigideCollider;
}

[CustomEditor(typeof(MapEntityPhysicsBehaviour))]
public class InspectorOfMapEntityPhysicsBehaviour : Editor {
    public MapEntityPhysicsBehaviour mPhysicsBehaviour { get => target as MapEntityPhysicsBehaviour; }
    public override void OnInspectorGUI() {
        //元のInspector部分を表示
        base.OnInspectorGUI();
        //ボタンを表示
        if (GUILayout.Button("set link")) {
            //各メンバ変数の割り当て
            mPhysicsBehaviour.mAttribute = mPhysicsBehaviour.GetComponentInChildren<EntityPhysicsAttribute>();
            mPhysicsBehaviour.mAttriubteCollider = mPhysicsBehaviour.mAttribute?.GetComponent<Collider>();
            mPhysicsBehaviour.mScaffoldRigide = mPhysicsBehaviour.GetComponentInChildren<MapScaffoldRigide>();
            mPhysicsBehaviour.mScaffoldRigideCollider = mPhysicsBehaviour.mScaffoldRigide?.GetComponent<Collider>();
        }
    }
}