using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFileData {
    private Arg mData;
    private List<Stratum> mStratumData;
    private Chip mChipData;
    private List<Ornament> mOrnamentData;
    private List<Npc> mNpcData;
    //<summary>マップ名</summary>
    public string name {
        get { return mData.get<string>("name"); }
    }
    //<summary>階層データ</summary>
    public List<Stratum> mStratums {
        get { return mStratumData; }
    }
    //<summary>マスデータ</summary>
    public Chip mChip {
        get { return mChipData; }
    }
    //<summary>物データ</summary>
    public List<Ornament> mOrnaments {
        get { return mOrnamentData; }
    }
    //<summary>npcデータ</summary>
    public List<Npc> mNpc {
        get { return mNpcData; }
    }

    public MapFileData(string aFilePath) {
        //ファイルロード
        mData = new Arg(MyJson.deserializeFile("Assets/resources/" + MyMap.mMapResourcesDirectory + "/map/" + aFilePath + ".json"));
        //階層データ
        mStratumData = new List<Stratum>();
        foreach (Arg tData in mData.get<List<Arg>>("stratum")) {
            mStratumData.Add(new Stratum(tData));
        }
        //chipデータ
        mChipData = new Chip(mData.get<Arg>("chip"));
        //ornamentデータ
        mOrnamentData = new List<Ornament>();
        foreach(Arg tData in mData.get<List<Arg>>("ornament")){
            mOrnamentData.Add(new Ornament(tData));
        }
        //npcデータ
        mNpcData = new List<Npc>();
        foreach (Arg tData in mData.get<List<Arg>>("npc")) {
            mNpcData.Add(new Npc(tData));
        }
    }

    public class Stratum {
        private Arg mData;
        //<summary>階層のフィールドデータ(mChipのkeyのリスト)</summary>
        public List<List<int>> mFeild {
            get { return mData.get<List<List<int>>>("feild"); }
        }
        public Stratum(Arg aData) {
            mData = aData;
        }
    }
    public class Chip {
        private Dictionary<string,Cell> mData;
        public Cell get(int aNum) {
            if (!mData.ContainsKey(aNum.ToString())) return null;
            return mData[aNum.ToString()];
        }
        public Chip(Arg aData) {
            mData = new Dictionary<string, Cell>();
            foreach(string tKey in aData.dictionary.Keys) {
                mData.Add(tKey, new Cell(aData.get<Arg>(tKey)));
            }
        }
    }
    public class Cell {
        private Arg mData;
        //<summary>タイルのプレハブへのパスのリスト</summary>
        public List<string> mTile {
            get { return mData.get<List<string>>("tile"); }
        }
        //<summary>エンカウント番号(エンカウントなしなら空文字列)</summary>
        public string mEncount {
            get {
                if (!mData.ContainsKey("encount")) return "";
                return mData.get<string>("encount");
            }
        }
        public Cell(Arg aData) {
            mData = aData;
        }
    }
    public class Ornament {
        private Arg mData;
        //<summary>プレハブへのパス</summary>
        public string mPath {
            get { return mData.get<string>("path"); }
        }
        //<summary>オブジェクトの名前</summary>
        public string mName {
            get {
                if (!mData.ContainsKey("name")) return "";
                return mData.get<string>("name");
            }
        }
        //<summary>x座標</summary>
        public float mX {
            get { return mData.get<float>("x"); }
        }
        //<summary>y座標</summary>
        public float mY {
            get { return mData.get<float>("y"); }
        }
        //<summary>階層</summary>
        public int mStratum {
            get { return mData.get<int>("stratum"); }
        }

        public Ornament(Arg aData) {
            mData = aData;
        }
    }
    public class Npc {
        private Arg mData;
        //<summary>プレハブへのパス</summary>
        public string mPath {
            get { return mData.get<string>("path"); }
        }
        //<summary>オブジェクトの名前</summary>
        public string mName {
            get {
                if (!mData.ContainsKey("name")) return "";
                return mData.get<string>("name");
            }
        }
        //<summary>AIを表したタグ</summary>
        public MyTag mAi {
            get { return new MyTag(mData.get<string>("ai")); }
        }
        //<summary>x座標</summary>
        public float mX {
            get { return mData.get<float>("x"); }
        }
        //<summary>y座標</summary>
        public float mY {
            get { return mData.get<float>("y"); }
        }
        //<summary>階層</summary>
        public int mStratum {
            get { return mData.get<int>("stratum"); }
        }
        public Npc(Arg aData) {
            mData = aData;
        }
    }
}
