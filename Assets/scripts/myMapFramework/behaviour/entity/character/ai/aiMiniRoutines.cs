using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public abstract partial class Ai{
        //<summary>指定距離移動(障害物などがあった場合は指定距離未満移動になる可能性あり)</summary>
        protected void addMoveByRoutine(Vector2 aVector,float aSpeed,Action aCallback){
            //残りの移動距離
            Vector2 tRemains = aVector;

            Action tRoutine = () => { };
            tRoutine = () =>{
                //移動終了
                if (tRemains.magnitude < 0.001f){
                    removeMiniRoutine(tRoutine);
                    aCallback();
                    return;
                }
                //移動
                Vector2 tDistance = calculateDistance(aVector, aSpeed, tRemains);
                tRemains -= tDistance;
                move(tDistance);
            };
            addMiniRoutine(tRoutine);
        }
    }
}
