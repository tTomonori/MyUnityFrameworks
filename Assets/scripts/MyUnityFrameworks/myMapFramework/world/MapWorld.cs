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

    public void addStratum(MapStratum aStratum) {
        int tStratumLevel = mStratums.Count;
        aStratum.mStratumLevel = tStratumLevel;
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
    }
    public void addCharacter(MapCharacter aCharacter,string aName,float aX,float aY,int aStratumLevel) {
        aCharacter.name = "charactor:" + aName;
        aCharacter.transform.SetParent(mStratums[aStratumLevel].mCharacters.transform, false);
        aCharacter.mMapPosition = new Vector2(aX, aY);
        aCharacter.positionZ = MapZOrderCalculator.calculateOrderOfEntity(aX, aY, aStratumLevel);
        mCharacters.Add(aCharacter);
    }
    public void addOrnament(MapOrnament aOrnament, string aName, float aX, float aY, int aStratumLevel) {
        aOrnament.name = "ornament:" + aName;
        aOrnament.transform.SetParent(mStratums[aStratumLevel].mOrnaments.transform, false);
        aOrnament.mMapPosition = new Vector2(aX, aY);
        aOrnament.positionZ = MapZOrderCalculator.calculateOrderOfEntity(aX, aY, aStratumLevel);
    }
}
