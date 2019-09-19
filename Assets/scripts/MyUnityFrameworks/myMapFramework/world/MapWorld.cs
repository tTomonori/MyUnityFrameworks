using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorld : MyBehaviour {
    //<summary>階層オブジェクトの入れ物</summary>
    public MyBehaviour mStratumContainer;
    //<summary>階層オブジェクト</summary>
    public List<MapStratum> mStratums = new List<MapStratum>();
    //<summary>キャラのリスト</summary>
    public List<MapCharacter> mCharacters = new List<MapCharacter>();
    private void Awake() {
        mStratumContainer = MyBehaviour.create<MyBehaviour>();
        mStratumContainer.name = "stratums";
        mStratumContainer.transform.SetParent(this.transform, false);
    }
    //behaviour追加
    public void addStratum(MapStratum aStratum) {
        int tStratumLevel = mStratums.Count;
        aStratum.mStratumLevel = new MapStratumLevel(tStratumLevel);
        aStratum.name = "stratum:" + tStratumLevel.ToString();
        aStratum.transform.SetParent(mStratumContainer.transform, false);
        aStratum.positionZ = MapZOrderCalculator.calculateOrderOfStratum(tStratumLevel);
        mStratums.Add(aStratum);
    }
    public void addCell(MapCell aCell,int aX,int aY,int aStratumLevel) {
        aCell.name = "cell:(" + aX.ToString() + "," + aY.ToString() + ")";
        aCell.transform.SetParent(mStratums[aStratumLevel].mMapCells.transform, false);
        aCell.mMapPosition = new Vector2(aX, aY);
        aCell.positionZ = MapZOrderCalculator.calculateOrderOfCell(aX, aY);
        aCell.mStratumLevel = new MapStratumLevel(aStratumLevel);
        aCell.placed();
    }
    public void addCharacter(MapCharacter aCharacter,string aName,float aX,float aY,int aStratumLevel) {
        aCharacter.name = "charactor:" + aName;
        aCharacter.transform.SetParent(mStratums[aStratumLevel].mCharacters.transform, false);
        aCharacter.mMapPosition = new Vector2(aX, aY);
        aCharacter.positionZ = MapZOrderCalculator.calculateOrderOfEntity(aX, aY, aStratumLevel);
        aCharacter.mStratumLevel = new MapStratumLevel(aStratumLevel);
        mCharacters.Add(aCharacter);
        aCharacter.placed();
    }
    public void addOrnament(MapOrnament aOrnament, string aName, float aX, float aY, int aStratumLevel) {
        aOrnament.name = "ornament:" + aName;
        aOrnament.transform.SetParent(mStratums[aStratumLevel].mOrnaments.transform, false);
        aOrnament.mMapPosition = new Vector2(aX, aY);
        aOrnament.positionZ = MapZOrderCalculator.calculateOrderOfEntity(aX, aY, aStratumLevel);
        aOrnament.mStratumLevel = new MapStratumLevel(aStratumLevel);
        aOrnament.placed();
    }
    public void addTrigger(MapTrigger aTrigger, string aName, float aX, float aY, int aStratumLevel) {
        aTrigger.name = "trigger:" + aName;
        aTrigger.transform.SetParent(mStratums[aStratumLevel].mTriggers.transform, false);
        aTrigger.mMapPosition = new Vector2(aX, aY);
        aTrigger.mStratumLevel = new MapStratumLevel(aStratumLevel);
        aTrigger.placed();
    }
    //behaivourの階層変更
    public void moveStratumLevel(MapCell aCell,int aStratumLevel) {
        aCell.transform.SetParent(mStratums[aStratumLevel].mMapCells.transform, false);
    }
    public void moveStratumLevel(MapCharacter aCharacter, int aStratumLevel) {
        aCharacter.transform.SetParent(mStratums[aStratumLevel].mCharacters.transform, false);
    }
    public void moveStratumLevel(MapOrnament aOrnament, int aStratumLevel) {
        aOrnament.transform.SetParent(mStratums[aStratumLevel].mOrnaments.transform, false);
    }
    public void moveStratumLevel(MapTrigger aTrigger,int aStratumLevel) {
        aTrigger.transform.SetParent(mStratums[aStratumLevel].mTriggers.transform, false);
    }

    private void Update() {
        MapWorldUpdater.updateWorld(this);
    }
}
