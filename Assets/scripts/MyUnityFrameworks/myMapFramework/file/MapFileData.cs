using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFileData {
    private Arg mData;
    private List<Stratum> mStratumData;
    private Chip mChipData;
    private List<Shadow> mShadowData;
    private List<Ornament> mOrnamentData;
    private List<Npc> mNpcData;
    private List<Trigger> mTriggerData;
    private Event mEventData;
    //<summary>マップ名</summary>
    public string name {
        get { return mData.get<string>("name"); }
    }
    ///<summary>階層データ</summary>
    public List<Stratum> mStratums {
        get { return mStratumData; }
    }
    ///<summary>マスデータ</summary>
    public Chip mChip {
        get { return mChipData; }
    }
    ///<summary>影データ</summary>
    public List<Shadow> mShadow {
        get { return mShadowData; }
    }
    ///<summary>物データ</summary>
    public List<Ornament> mOrnaments {
        get { return mOrnamentData; }
    }
    ///<summary>npcデータ</summary>
    public List<Npc> mNpc {
        get { return mNpcData; }
    }
    ///<summary>triggerデータ</summary>
    public List<Trigger> mTrigger{
        get { return mTriggerData; }
    }
    ///<summary>イベントデータ</summary>
    public Event mEvent {
        get { return mEventData; }
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
        //shadowデータ
        mShadowData = new List<Shadow>();
        foreach(Arg tData in mData.get<List<Arg>>("shadow")) {
            mShadowData.Add(new Shadow(tData));
        }
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
        //triggerデータ
        mTriggerData = new List<Trigger>();
        foreach (Arg tData in mData.get<List<Arg>>("trigger")) {
            mTriggerData.Add(new Trigger(tData));
        }
        //イベントデータ
        mEventData = new Event(mData.get<Arg>("event"));
    }

    public class Stratum {
        private Arg mData;
        ///<summary>階層のフィールドデータ(mChipのkeyのリスト)</summary>
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
        ///<summary>プレハブへのパス</summary>
        public string mCell {
            get { return mData.get<string>("cell"); }
        }
        public int mDrawOffsetY {
            get {
                if (mData.ContainsKey("drawOffsetY"))
                    return mData.get<int>("drawOffsetY");
                else
                    return 0;
            }
        }
        ///<summary>エンカウント番号(エンカウントなしなら空文字列)</summary>
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
    public class Shadow {
        private Arg mData;
        /// <summary>影に使うspriteへのパス</summary>
        public string mSpritePath {
            get { return mData.get<string>("sprite"); }
        }
        /// <summary>colliderの形状を表したタグ</summary>
        public MyTag mCollider {
            get { return new MyTag(mData.get<string>("collider")); }
        }
        /// <summary>影を配置する座標のリスト Vector3(X,Y,Stratum)</summary>
        public List<Vector3> mPosition {
            get { return mData.get<List<Vector3>>("position"); }
        }
        /// <summary>指定座標のcellからずらす方向</summary>
        public Vector2 mOffset {
            get {
                if (mData.ContainsKey("offset")) return mData.get<Vector2>("offset");
                else return Vector2.zero;
            }
        }
        /// <summary>影をかける強さ(0~1)</summary>
        public float mShadePower {
            get {
                if (mData.ContainsKey("shadePower")) return mData.get<float>("shadePower");
                else return 0.3f;
            }
        }
        public Shadow(Arg aData) {
            mData = aData;
        }
    }
    public class Ornament {
        private Arg mData;
        ///<summary>プレハブへのパス</summary>
        public string mPath {
            get { return mData.get<string>("path"); }
        }
        ///<summary>オブジェクトの名前</summary>
        public string mName {
            get {
                if (!mData.ContainsKey("name")) return "";
                return mData.get<string>("name");
            }
        }
        ///<summary>x座標</summary>
        public float mX {
            get { return mData.get<float>("x"); }
        }
        ///<summary>y座標</summary>
        public float mY {
            get { return mData.get<float>("y"); }
        }
        ///<summary>高さ</summary>
        public float mHeight {
            get { return mData.get<int>("height"); }
        }

        public Ornament(Arg aData) {
            mData = aData;
        }
    }
    public class Npc {
        private Arg mData;
        ///<summary>プレハブへのパス</summary>
        public string mPath {
            get { return mData.get<string>("path"); }
        }
        ///<summary>オブジェクトの名前</summary>
        public string mName {
            get {
                if (!mData.ContainsKey("name")) return "";
                return mData.get<string>("name");
            }
        }
        ///<summary>向き</summary>
        public Vector2 mDirection {
            get { return mData.get<Vector2>("direction"); }
        }
        ///<summary>AIを表したタグ</summary>
        public MyTag mAi {
            get { return new MyTag(mData.get<string>("ai")); }
        }
        ///<summary>x座標</summary>
        public float mX {
            get { return mData.get<float>("x"); }
        }
        ///<summary>y座標</summary>
        public float mY {
            get { return mData.get<float>("y"); }
        }
        ///<summary>高さ</summary>
        public float mHeight {
            get { return mData.get<int>("height"); }
        }
        public Npc(Arg aData) {
            mData = aData;
        }
    }
    public class Trigger {
        private Arg mData;
        ///<summary>トリガーの名前</summary>
        public string mName {
            get {
                if (!mData.ContainsKey("name")) return "";
                return mData.get<string>("name");
            }
        }
        ///<summary>トリガーの形状を表したタグ</summary>
        public MyTag mShape {
            get { return new MyTag(mData.get<string>("shape")); }
        }
        ///<summary>x座標</summary>
        public float mX {
            get { return mData.get<float>("x"); }
        }
        //<summary>y座標</summary>
        public float mY {
            get { return mData.get<float>("y"); }
        }
        ///<summary>高さ</summary>
        public float mHeight {
            get { return mData.get<int>("height"); }
        }
        public Trigger(Arg aData) {
            mData = aData;
        }
    }
    public class Event {
        private Arg mData;
        public Event(Arg aData) {
            mData = aData;
        }
    }
}
