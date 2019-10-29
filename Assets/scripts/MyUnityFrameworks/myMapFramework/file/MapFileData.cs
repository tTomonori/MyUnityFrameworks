using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapFileData {
    protected Arg mData;
    ///<summary>マップ名</summary>
    public string mMapName;
    /// <summary>カメラに写すフィールド外の最大距離</summary>
    public float mFieldMargin;
    /// <summary>カメラのorthographicSize(デフォルト値を適用するなら負の値)</summary>
    public float mCameraSize;
    /// <summary>MapFramework外部で使う変数</summary>
    public Arg mArg;
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

    public MapFileData() {
        mData = new Arg();
        mStratums = new List<Stratum>();
        mChip = new Chip(new Arg());
        mShadows = new List<Shadow>();
        mOrnaments = new List<Ornament>();
        mCharacters = new List<Character>();
        mTriggers = new List<Trigger>();
        mEvents = new Event(new Arg());
        mEntrances = new Dictionary<string, Entrance>();
    }
    public MapFileData(string aFilePath) {
        //ファイルロード
        load(new Arg(MyJson.deserializeFile("Assets/resources/" + MyMap.mMapResourcesDirectory + "/map/" + aFilePath + ".json")));
    }
    protected void load(Arg aData) {
        mData = aData;

        //マップ名
        mMapName = mData.get<string>("name");
        //field margin
        mFieldMargin = mData.ContainsKey("fieldMargin") ? mData.get<float>("fieldMargin") : 0;
        //camera size
        mCameraSize = mData.ContainsKey("cameraSize") ? mData.get<float>("cameraSize") : -1;
        //フレームワーク外部用変数
        mArg = mData.ContainsKey("arg") ? mData.get<Arg>("arg") : new Arg();
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
    /// <summary>保持内容をArgにまとめる</summary>
    public virtual Arg createDic() {
        Arg tDic = new Arg();
        List<Arg> tList;
        //マップ名
        tDic.set("name", mMapName);
        //field margin
        tDic.set("fieldMargin", mFieldMargin);
        //camera size
        if (mCameraSize > 0)
            tDic.set("cameraSize", mCameraSize);
        //フレームワーク外部用変数
        tDic.set("arg", mArg.dictionary);
        //階層データ
        tList = new List<Arg>();
        foreach (Stratum tStratum in mStratums) {
            tList.Add(tStratum.mData);
        }
        tDic.set("stratum", tList);
        //chipデータ
        tDic.set("chip", mChip.mData);
        //shadowデータ
        tList = new List<Arg>();
        foreach (Shadow tShadow in mShadows) {
            tList.Add(tShadow.mData);
        }
        tDic.set("shadow", tList);
        //ornamentデータ
        tList = new List<Arg>();
        foreach (Ornament tOrnament in mOrnaments) {
            tList.Add(tOrnament.mData);
        }
        tDic.set("ornament", tList);
        //characterデータ
        tList = new List<Arg>();
        foreach (Character tCharacter in mCharacters) {
            tList.Add(tCharacter.mData);
        }
        tDic.set("character", tList);
        //triggerデータ
        tList = new List<Arg>();
        foreach (Trigger tTrigger in mTriggers) {
            tList.Add(tTrigger.mData);
        }
        tDic.set("trigger", tList);
        //イベントデータ
        tDic.set("event", mEvents.mData);
        //入り口データ
        Dictionary<string, Dictionary<string, object>> tEntranceDic = new Dictionary<string, Dictionary<string, object>>();
        foreach (KeyValuePair<string, Entrance> tPair in mEntrances) {
            tEntranceDic.Add(tPair.Key, (Dictionary<string, object>)tPair.Value.mData.dictionary);
        }
        tDic.set("entrance", tEntranceDic);

        return tDic;
    }
}
