using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAccessoryBehaviour : MyBehaviour {
    [SerializeField] public AccessoryType mType = AccessoryType.custom;
    [SerializeField] public Vector3 mPosition;
    [SerializeField] public int mPileLevel = 0;

    public enum AccessoryType {
        lie, stand, custom
    }

    private void OnValidate() {
        if (Application.isPlaying) return;

        if (mType != AccessoryType.custom)
            setMeshComponent();

        setPosition();
    }
    private void setMeshComponent() {
        Mesh2D tMesh = findChild<Mesh2D>("mesh");
        if (tMesh == null) {
            switch (mType) {
                case AccessoryType.lie:
                    tMesh = this.createChild<LieMesh>();
                    break;
                case AccessoryType.stand:
                    tMesh = this.createChild<StandMesh>();
                    break;
            }
            tMesh.name = "mesh";
            return;
        }
        switch (mType) {
            case AccessoryType.lie:
                if (tMesh is StandMesh) {
                    tMesh = tMesh.gameObject.AddComponent<LieMesh>();
                    Destroy(tMesh.GetComponent<StandMesh>());
                }
                break;
            case AccessoryType.stand:
                if (tMesh is LieMesh) {
                    tMesh = tMesh.gameObject.AddComponent<StandMesh>();
                    Destroy(tMesh.GetComponent<LieMesh>());
                }
                break;
        }
    }
    private void setPosition() {
        this.position = new MapPosition(mPosition, mPileLevel).renderPosition;
    }
}
