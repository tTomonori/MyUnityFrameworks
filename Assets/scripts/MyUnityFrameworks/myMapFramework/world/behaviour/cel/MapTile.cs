using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class MapTile : MapBehaviour {
    /// <summary>足場の形状(描画順序決定用)</summary>
    [SerializeField] public ScaffoldType mScaffoldType = ScaffoldType.custom;
    public enum ScaffoldType {
        flat, stand, custom, none, upHigh, downHigh, leftHigh, rightHigh, upHighHalf, downHighHalf, leftHighHalf, rightHighHalf, upHighDouble, downHighDouble, leftHighDouble, rightHighDouble
    }

    [SerializeField] private MapRenderBehaviour _RenderBehaviour;
    [SerializeField] private MapPhysicsBehaviour _PhysicsBehaviour;
    public override MapRenderBehaviour mRenderBehaviour { get => _RenderBehaviour; set => _RenderBehaviour = value; }
    public override MapPhysicsBehaviour mPhysicsBehaviour { get => _PhysicsBehaviour; set => _PhysicsBehaviour = value; }
}

[CustomEditor(typeof(MapTile))]
public class InspectorOfMapTile : InspectorOfMapBehaviour {
    public MapTile mTile { get => target as MapTile; }
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
        MapRenderBehaviour tRender = mTile.createChild<MapRenderBehaviour>("render");
        mTile.mRenderBehaviour = tRender;
        switch (mTile.mScaffoldType) {
            case MapTile.ScaffoldType.flat:
                LieMesh flat = tRender.createChild<LieMesh>("flat");
                break;
            case MapTile.ScaffoldType.stand:
                StandMesh stand = tRender.createChild<StandMesh>("stand");
                stand.positionY = -0.5f;
                stand.positionZ = -0.5f;
                break;
            case MapTile.ScaffoldType.upHigh:
                SlopeMesh upHigh = tRender.createChild<SlopeMesh>("slope");
                upHigh.mDirection = new Vector2(0, 1);
                upHigh.mLowHeight = 0;
                upHigh.mHighHeight = 1;
                break;
            case MapTile.ScaffoldType.downHigh:
                SlopeMesh downHigh = tRender.createChild<SlopeMesh>("slope");
                downHigh.mDirection = new Vector2(0, -1);
                downHigh.mLowHeight = 0;
                downHigh.mHighHeight = 1;
                break;
            case MapTile.ScaffoldType.leftHigh:
                SlopeMesh leftHigh = tRender.createChild<SlopeMesh>("slope");
                leftHigh.mDirection = new Vector2(-1, 0);
                leftHigh.mLowHeight = 0;
                leftHigh.mHighHeight = 1;
                break;
            case MapTile.ScaffoldType.rightHigh:
                SlopeMesh rightHigh = tRender.createChild<SlopeMesh>("slope");
                rightHigh.mDirection = new Vector2(1, 0);
                rightHigh.mLowHeight = 0;
                rightHigh.mHighHeight = 1;
                break;
            case MapTile.ScaffoldType.upHighHalf:
                SlopeMesh upHighHalf = tRender.createChild<SlopeMesh>("slope");
                upHighHalf.mDirection = new Vector2(0, 1);
                upHighHalf.mSize = new Vector2(1, 0.5f);
                upHighHalf.mLowHeight = 0;
                upHighHalf.mHighHeight = 1;
                upHighHalf.position = new MapPosition(new Vector3(0, 0, 0.25f)).renderPosition;
                break;
            case MapTile.ScaffoldType.downHighHalf:
                SlopeMesh downHighHalf = tRender.createChild<SlopeMesh>("slope");
                downHighHalf.mDirection = new Vector2(0, -1);
                downHighHalf.mSize = new Vector2(1, 0.5f);
                downHighHalf.mLowHeight = 0;
                downHighHalf.mHighHeight = 1;
                downHighHalf.position = new MapPosition(new Vector3(0, 0, -0.25f)).renderPosition;
                break;
            case MapTile.ScaffoldType.leftHighHalf:
                SlopeMesh leftHighHalf = tRender.createChild<SlopeMesh>("slope");
                leftHighHalf.mDirection = new Vector2(-1, 0);
                leftHighHalf.mSize = new Vector2(0.5f, 1);
                leftHighHalf.mLowHeight = 0;
                leftHighHalf.mHighHeight = 1;
                leftHighHalf.position = new MapPosition(new Vector3(-0.25f, 0, 0)).renderPosition;
                break;
            case MapTile.ScaffoldType.rightHighHalf:
                SlopeMesh rightHighHalf = tRender.createChild<SlopeMesh>("slope");
                rightHighHalf.mDirection = new Vector2(1, 0);
                rightHighHalf.mSize = new Vector2(0.5f, 1);
                rightHighHalf.mLowHeight = 0;
                rightHighHalf.mHighHeight = 1;
                rightHighHalf.position = new MapPosition(new Vector3(0.25f, 0, 0)).renderPosition;
                break;
            case MapTile.ScaffoldType.upHighDouble:
                SlopeMesh upHighDouble = tRender.createChild<SlopeMesh>("slope");
                upHighDouble.mDirection = new Vector2(0, 1);
                upHighDouble.mLowHeight = 0;
                upHighDouble.mHighHeight = 0.5f;
                upHighDouble.position = new MapPosition(new Vector3(0, -0.25f, 0)).renderPosition;
                break;
            case MapTile.ScaffoldType.downHighDouble:
                SlopeMesh downHighDouble = tRender.createChild<SlopeMesh>("slope");
                downHighDouble.mDirection = new Vector2(0, -1);
                downHighDouble.mLowHeight = 0;
                downHighDouble.mHighHeight = 0.5f;
                downHighDouble.position = new MapPosition(new Vector3(0, -0.25f, 0)).renderPosition;
                break;
            case MapTile.ScaffoldType.leftHighDouble:
                SlopeMesh leftHighDouble = tRender.createChild<SlopeMesh>("slope");
                leftHighDouble.mDirection = new Vector2(-1, 0);
                leftHighDouble.mLowHeight = 0;
                leftHighDouble.mHighHeight = 0.5f;
                leftHighDouble.position = new MapPosition(new Vector3(0, -0.25f, 0)).renderPosition;
                break;
            case MapTile.ScaffoldType.rightHighDouble:
                SlopeMesh rightHighDouble = tRender.createChild<SlopeMesh>("slope");
                rightHighDouble.mDirection = new Vector2(1, 0);
                rightHighDouble.mLowHeight = 0;
                rightHighDouble.mHighHeight = 0.5f;
                rightHighDouble.position = new MapPosition(new Vector3(0, -0.25f, 0)).renderPosition;
                break;
        }
    }
    public void createPhysics() {
        MapPhysicsBehaviour tPhysics = mTile.createChild<MapPhysicsBehaviour>("physics");
        mTile.mPhysicsBehaviour = tPhysics;
        //属性
        if (mTile.mScaffoldType != MapTile.ScaffoldType.custom && mTile.mScaffoldType != MapTile.ScaffoldType.none) {
            TileGroundPhysicsAttribute tAttribute = tPhysics.createChild<TileGroundPhysicsAttribute>("attribute");
            tAttribute.mAttribute = TileGroundPhysicsAttribute.Attribute.flat;
            tAttribute.mTile = mTile;
            switch (mTile.mScaffoldType) {
                case MapTile.ScaffoldType.flat:
                    BoxCollider flat = tAttribute.gameObject.AddComponent<BoxCollider>();
                    flat.size = new Vector3(1, 1, 1);
                    flat.center = new Vector3(0, 0.5f, 0);
                    break;
                case MapTile.ScaffoldType.stand:
                    BoxCollider stand = tAttribute.gameObject.AddComponent<BoxCollider>();
                    stand.size = new Vector3(1, 1, 0.1f);
                    stand.center = new Vector3(0, 0, -0.45f);
                    break;
                case MapTile.ScaffoldType.upHigh:
                case MapTile.ScaffoldType.downHigh:
                case MapTile.ScaffoldType.leftHigh:
                case MapTile.ScaffoldType.rightHigh:
                case MapTile.ScaffoldType.upHighHalf:
                case MapTile.ScaffoldType.downHighHalf:
                case MapTile.ScaffoldType.leftHighHalf:
                case MapTile.ScaffoldType.rightHighHalf:
                case MapTile.ScaffoldType.upHighDouble:
                case MapTile.ScaffoldType.downHighDouble:
                case MapTile.ScaffoldType.leftHighDouble:
                case MapTile.ScaffoldType.rightHighDouble:
                    BoxCollider slope = tAttribute.gameObject.AddComponent<BoxCollider>();
                    slope.size = new Vector3(1, 1.5f, 1);
                    slope.center = new Vector3(0, 0.75f, 0);
                    break;
            }
        }
        //足場
        if (mTile.mScaffoldType != MapTile.ScaffoldType.stand && mTile.mScaffoldType != MapTile.ScaffoldType.custom && mTile.mScaffoldType != MapTile.ScaffoldType.none) {
            MapScaffold tScaffold = tPhysics.createChild<MapScaffold>("scaffold");
            switch (mTile.mScaffoldType) {
                case MapTile.ScaffoldType.flat:
                    BoxCollider flat = tScaffold.gameObject.AddComponent<BoxCollider>();
                    flat.size = new Vector3(1, 0.01f, 1);
                    flat.center = new Vector3(0, 0.005f, 0);
                    break;
                case MapTile.ScaffoldType.upHigh:
                    MyPillarMeshCollider upHigh = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    upHigh.mLength = 1;
                    upHigh.mPoints = new Vector3[3] {
                        new Vector3(0.5f,0,-0.5f),
                        new Vector3(0.5f,1,0.5f),
                        new Vector3(0.5f,0,0.5f)
                    };
                    upHigh.createCollider();
                    break;
                case MapTile.ScaffoldType.downHigh:
                    MyPillarMeshCollider downHigh = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    downHigh.mLength = 1;
                    downHigh.mPoints = new Vector3[3] {
                        new Vector3(0.5f,0,-0.5f),
                        new Vector3(0.5f,1,-0.5f),
                        new Vector3(0.5f,0,0.5f)
                    };
                    downHigh.createCollider();
                    break;
                case MapTile.ScaffoldType.leftHigh:
                    MyPillarMeshCollider leftHigh = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    leftHigh.mLength = 1;
                    leftHigh.mPoints = new Vector3[3] {
                        new Vector3(-0.5f,0,-0.5f),
                        new Vector3(-0.5f,1,-0.5f),
                        new Vector3(0.5f,0,-0.5f)
                    };
                    leftHigh.createCollider();
                    break;
                case MapTile.ScaffoldType.rightHigh:
                    MyPillarMeshCollider rightHigh = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    rightHigh.mLength = 1;
                    rightHigh.mPoints = new Vector3[3] {
                        new Vector3(-0.5f,0,-0.5f),
                        new Vector3(0.5f,1,-0.5f),
                        new Vector3(0.5f,0,-0.5f)
                    };
                    rightHigh.createCollider();
                    break;
                case MapTile.ScaffoldType.upHighHalf:
                    MyPillarMeshCollider upHighHalf = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    upHighHalf.mLength = 1;
                    upHighHalf.mPoints = new Vector3[3] {
                        new Vector3(0.5f,0,0),
                        new Vector3(0.5f,1,0.5f),
                        new Vector3(0.5f,0,0.5f)
                    };
                    upHighHalf.createCollider();
                    break;
                case MapTile.ScaffoldType.downHighHalf:
                    MyPillarMeshCollider downHighHalf = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    downHighHalf.mLength = 1;
                    downHighHalf.mPoints = new Vector3[3] {
                        new Vector3(0.5f,0,-0.5f),
                        new Vector3(0.5f,1,-0.5f),
                        new Vector3(0.5f,0,0)
                    };
                    downHighHalf.createCollider();
                    break;
                case MapTile.ScaffoldType.leftHighHalf:
                    MyPillarMeshCollider leftHighHalf = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    leftHighHalf.mLength = 1;
                    leftHighHalf.mPoints = new Vector3[3] {
                        new Vector3(-0.5f,0,-0.5f),
                        new Vector3(-0.5f,1,-0.5f),
                        new Vector3(0,0,-0.5f)
                    };
                    leftHighHalf.createCollider();
                    break;
                case MapTile.ScaffoldType.rightHighHalf:
                    MyPillarMeshCollider rightHighHalf = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    rightHighHalf.mLength = 1;
                    rightHighHalf.mPoints = new Vector3[3] {
                        new Vector3(0,0,-0.5f),
                        new Vector3(0.5f,1,-0.5f),
                        new Vector3(0.5f,0,-0.5f)
                    };
                    rightHighHalf.createCollider();
                    break;
                case MapTile.ScaffoldType.upHighDouble:
                    MyPillarMeshCollider upHighDouble = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    upHighDouble.mLength = 1;
                    upHighDouble.mPoints = new Vector3[3] {
                        new Vector3(0.5f,0,-0.5f),
                        new Vector3(0.5f,0.5f,0.5f),
                        new Vector3(0.5f,0,0.5f)
                    };
                    upHighDouble.createCollider();
                    break;
                case MapTile.ScaffoldType.downHighDouble:
                    MyPillarMeshCollider downHighDouble = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    downHighDouble.mLength = 1;
                    downHighDouble.mPoints = new Vector3[3] {
                        new Vector3(0.5f,0,-0.5f),
                        new Vector3(0.5f,0.5f,-0.5f),
                        new Vector3(0.5f,0,0.5f)
                    };
                    downHighDouble.createCollider();
                    break;
                case MapTile.ScaffoldType.leftHighDouble:
                    MyPillarMeshCollider leftHighDouble = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    leftHighDouble.mLength = 1;
                    leftHighDouble.mPoints = new Vector3[3] {
                        new Vector3(-0.5f,0,-0.5f),
                        new Vector3(-0.5f,0.5f,-0.5f),
                        new Vector3(0.5f,0,-0.5f)
                    };
                    leftHighDouble.createCollider();
                    break;
                case MapTile.ScaffoldType.rightHighDouble:
                    MyPillarMeshCollider rightHighDouble = tScaffold.gameObject.AddComponent<MyPillarMeshCollider>();
                    rightHighDouble.mLength = 1;
                    rightHighDouble.mPoints = new Vector3[3] {
                        new Vector3(-0.5f,0,-0.5f),
                        new Vector3(0.5f,0.5f,-0.5f),
                        new Vector3(0.5f,0,-0.5f)
                    };
                    rightHighDouble.createCollider();
                    break;
            }
        }
    }
}