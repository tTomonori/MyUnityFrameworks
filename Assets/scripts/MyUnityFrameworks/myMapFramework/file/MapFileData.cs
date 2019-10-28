using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapFileData {
    private Arg mData;
    ///<summary>マップ名</summary>
    public string mMapName;
    ///<summary>階層データ</summary>
    public List<Stratum> mStratums;
    ///<summary>マスデータ</summary>
    public Chip mChip;
    ///<summary>影データ</summary>
    public List<Shadow> mShadows;
    ///<summary>物データ</summary>
    public List<Ornament> mOrnaments;
    ///<summary>npcデータ</summary>
    public List<Character> mCharacters;
    ///<summary>triggerデータ</summary>
    public List<Trigger> mTriggers;
    ///<summary>イベントデータ</summary>
    public Event mEvents;
    /// <summary>マップの入り口データ</summary>
    public Dictionary<string, Entrance> mEntrances;

    public MapFileData(string aFilePath) {
        //ファイルロード
        mData = new Arg(MyJson.deserializeFile("Assets/resources/" + MyMap.mMapResourcesDirectory + "/map/" + aFilePath + ".json"));

        //マップ名
        mMapName = mData.get<string>("name");
        //階層データ
        mStratums = new List<Stratum>();
        foreach (Arg tData in mData.get<List<Arg>>("stratum")) {
            mStratums.Add(new Stratum(tData));
        }
        //chipデータ
        mChip = new Chip(mData.get<Arg>("chip"));
        //shadowデータ
        mShadows = new List<Shadow>();
        foreach (Arg tData in mData.get<List<Arg>>("shadow")) {
            mShadows.Add(new Shadow(tData));
        }
        //ornamentデータ
        mOrnaments = new List<Ornament>();
        foreach (Arg tData in mData.get<List<Arg>>("ornament")) {
            mOrnaments.Add(new Ornament(tData));
        }
        //characterデータ
        mCharacters = new List<Character>();
        foreach (Arg tData in mData.get<List<Arg>>("character")) {
            mCharacters.Add(new Character(tData));
        }
        //triggerデータ
        mTriggers = new List<Trigger>();
        foreach (Arg tData in mData.get<List<Arg>>("trigger")) {
            mTriggers.Add(new Trigger(tData));
        }
        //イベントデータ
        mEvents = new Event(mData.get<Arg>("event"));
        //入り口データ
        mEntrances = new Dictionary<string, Entrance>();
        Arg tEntrance = mData.get<Arg>("entrance");
        foreach (KeyValuePair<string, object> tPair in (Dictionary<string, object>)tEntrance.dictionary) {
            mEntrances.Add(tPair.Key, new Entrance(tEntrance.get<Arg>(tPair.Key)));
        }
    }
}
