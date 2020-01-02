using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapShadowUpdater {
    public static void updateShadow(MapEntity aEntity) {
        if (aEntity.mEntityRenderBehaviour.mShadow == null) return;
        switch (aEntity.mEntityPhysicsBehaviour.mScaffoldRigide.mAttribute) {
            case MapScaffoldRigide.ScaffoldRigideAttribute.normal:
                aEntity.mEntityRenderBehaviour.mShadow.positionY = 0;
                break;
            case MapScaffoldRigide.ScaffoldRigideAttribute.fly:
                aEntity.mEntityRenderBehaviour.mShadow.positionY = -MapHeightUpdateSystem.getDistanceToScaffold(aEntity);
                break;
                //case MapScaffoldRigide.ScaffoldRigideAttribute.dug:
        }
    }
}
