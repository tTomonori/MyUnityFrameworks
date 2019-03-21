using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStratum : MyBehaviour {
    private int mStratumNum;
    public int stratumNum{
        get { return mStratumNum; }
        set{
            mStratumNum = value;
            positionZ = -value;
        }
    }
    private MyBehaviour mTiles;
    private MyBehaviour mCharacters;
    private MyBehaviour mOrnaments;
    private MyBehaviour mTriggers;
    private void Awake(){
        mTiles = MyBehaviour.create<MyBehaviour>();
        mTiles.name = "tiles";
        mTiles.transform.SetParent(transform, false);
        mCharacters = MyBehaviour.create<MyBehaviour>();
        mCharacters.name = "characters";
        mCharacters.transform.SetParent(transform, false);
        mOrnaments = MyBehaviour.create<MyBehaviour>();
        mOrnaments.name = "ornaments";
        mOrnaments.transform.SetParent(transform, false);
        mTriggers = MyBehaviour.create<MyBehaviour>();
        mTriggers.name = "triggers";
        mTriggers.transform.SetParent(transform, false);
    }
    public void addTile(MapTile aTile){
        aTile.transform.SetParent(mTiles.transform,false);
    }
    public void addCharacter(MapCharacter aChara){
        aChara.transform.SetParent(mCharacters.transform,false);
    }
    public void addOrnament(MapOrnament aOrnament){
        aOrnament.transform.SetParent(mOrnaments.transform,false);
    }
    public void addTrigger(MapTrigger aTrigger){
        aTrigger.transform.SetParent(mTriggers.transform, false);
    }
    public void add(MapBehaviour aBehaviour){
        MapEntity[] tEntity = aBehaviour.GetComponents<MapEntity>();
        if (tEntity[0] is MapCharacter){
            addCharacter((MapCharacter)tEntity[0]);
            return;
        }
        Debug.Log("MapStratum : " + aBehaviour.GetType().ToString() + "型の階層を変更するのは想定外");
    }
    //指定した階層と衝突判定するか
    public bool canCollide(MapStratum aStatum){
        if (aStatum == null) return false;
        return (mStratumNum == aStatum.mStratumNum) || (mStratumNum + 1 == aStatum.mStratumNum);
    }
}
