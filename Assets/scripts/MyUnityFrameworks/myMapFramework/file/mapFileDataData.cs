using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapFileData {
    /// <summary>階層データ</summary>
    public class Stratum {
        public Arg mData;
        ///<summary>階層のフィールドデータ(mChipのkeyのリスト)</summary>
        public List<List<int>> mFeild {
            get { return mData.get<List<List<int>>>("feild"); }
        }
        ///<summary>+0.5階層のフィールドデータ(mChipのkeyのリスト)</summary>
        public List<List<int>> mHalfHeightFeild {
            get { return mData.get<List<List<int>>>("halfHeight"); }
        }
        public Stratum(Arg aData) {
            mData = aData;
        }
    }
    /// <summary>マス情報のDic</summary>
    public class Chip {
        public Arg mData;
        public Dictionary<string, Tile> mDic;
        public Tile get(int aNum) {
            if (!mDic.ContainsKey(aNum.ToString())) return null;
            return mDic[aNum.ToString()];
        }
        public Chip(Arg aData) {
            mData = aData;
            mDic = new Dictionary<string, Tile>();
            foreach (string tKey in aData.dictionary.Keys) {
                mDic.Add(tKey, new Tile(aData.get<Arg>(tKey)));
            }
        }
    }
    /// <summary>マス情報</summary>
    public class Tile {
        public Arg mData;
        ///<summary>プレハブへのパス</summary>
        public string mTile {
            get { return mData.get<string>("tile"); }
            set { mData.set("tile", value); }
        }
        public int mDrawOffsetH {
            get {
                if (mData.ContainsKey("drawOffsetH"))
                    return mData.get<int>("drawOffsetH");
                else
                    return 0;
            }
            set { mData.set("drawOffsetH", value); }
        }
        ///<summary>エンカウント番号(エンカウントなしなら空文字列)</summary>
        public string mEncountKey {
            get {
                if (!mData.ContainsKey("encountKey")) return "";
                return mData.get<string>("encountKey");
            }
            set { mData.set("encountKey", value); }
        }
        public float mEncountFrequency {
            get {
                if (!mData.ContainsKey("encountFrequency")) return 0;
                return mData.get<float>("encountFrequency");
            }
            set { mData.set("encountFrequency", value); }
        }
        public Tile(Arg aData) {
            mData = aData;
        }
    }
    /// <summary>影</summary>
    public class Shadow {
        public Arg mData;
        /// <summary>影に使うspriteへのパス</summary>
        public string mSpritePath {
            get { return mData.get<string>("sprite"); }
            set { mData.set("sprite", value); }
        }
        /// <summary>colliderの形状を表したタグ</summary>
        public MyTag mCollider {
            get { return new MyTag(mData.get<string>("collider")); }
            set { mData.set("collider", value); }
        }
        /// <summary>影を配置する座標のリスト Vector3(X,Y,Stratum)</summary>
        public List<Vector3> mPosition {
            get { return mData.get<List<Vector3>>("position"); }
            set { mData.set("position", value); }
        }
        /// <summary>指定座標のcellからずらす方向</summary>
        public Vector2 mOffset {
            get {
                if (mData.ContainsKey("offset")) return mData.get<Vector2>("offset");
                else return Vector2.zero;
            }
            set { mData.set("offset", value); }
        }
        /// <summary>影をかける強さ(0~1)</summary>
        public float mShadePower {
            get {
                if (mData.ContainsKey("shadePower")) return mData.get<float>("shadePower");
                else return 0.3f;
            }
            set { mData.set("shadePower", value); }
        }
        public Shadow(Arg aData) {
            mData = aData;
        }
    }
    public class Behaviour {
        public Arg mData;
        ///<summary>オブジェクトの名前</summary>
        public string mName {
            get {
                if (!mData.ContainsKey("name")) return "";
                return mData.get<string>("name");
            }
            set { mData.set("name", value); }
        }
        ///<summary>x座標</summary>
        public float mX {
            get { return mData.get<float>("x"); }
            set { mData.set("x", value); }
        }
        ///<summary>y座標</summary>
        public float mY {
            get { return mData.get<float>("y"); }
            set { mData.set("y", value); }
        }
        ///<summary>高さ</summary>
        public float mHeight {
            get { return mData.get<float>("height"); }
            set { mData.set("height", value); }
        }
        /// <summary>フラグが立っている場合のみ生成する</summary>
        public MyFlagItem mCreateFlag {
            get {
                if (mData.ContainsKey("createFlag")) return MyFlagItem.create(mData.get<string>("createFlag"));
                else return null;
            }
            set {
                if (value != null) mData.set("createFlag", value.toString());
                else mData.remove("createFlag");
            }
        }
        public string mCreateFlagString {
            get {
                if (mData.ContainsKey("createFlag")) return mData.get<string>("createFlag");
                else return null;
            }
            set {
                if (value != null || value != "") mData.set("createFlag", value);
                else mData.remove("createFlag");
            }
        }
        /// <summary>フラグが立っている場合は生成しない</summary>
        public MyFlagItem mDeleteFlag {
            get {
                if (mData.ContainsKey("deleteFlag")) return MyFlagItem.create(mData.get<string>("deleteFlag"));
                else return null;
            }
            set {
                if (value != null) mData.set("deleteFlag", value.toString());
                else mData.remove("deleteFlag");
            }
        }
        public string mDeleteFlagString {
            get {
                if (mData.ContainsKey("deleteFlag")) return mData.get<string>("deleteFlag");
                else return null;
            }
            set {
                if (value != null || value != "") mData.set("deleteFlag", value);
                else mData.remove("deleteFlag");
            }
        }
    }
    /// <summary>Entity</summary>
    public class Entity : Behaviour {
        ///<summary>プレハブへのパス</summary>
        public string mPath {
            get { return mData.get<string>("path"); }
            set { mData.set("path", value); }
        }
        ///<summary>話かけられた時のイベント</summary>
        public string mSpeakDefault {
            get { return (mData.ContainsKey("speakDefault")) ? mData.get<string>("speakDefault") : ""; }
            set {
                if (value == "") mData.remove("speakDefault");
                else mData.set("speakDefault", value);
            }
        }
        ///<summary>上から話かけられた時のイベント</summary>
        public string mSpeakFromUp {
            get { return (mData.ContainsKey("speakFromUp")) ? mData.get<string>("speakFromUp") : ""; }
            set {
                if (value == "") mData.remove("speakFromUp");
                else mData.set("speakFromUp", value);
            }
        }
        ///<summary>下から話かけられた時のイベント</summary>
        public string mSpeakFromDown {
            get { return (mData.ContainsKey("speakFromDown")) ? mData.get<string>("speakFromDown") : ""; }
            set {
                if (value == "") mData.remove("speakFromDown");
                else mData.set("speakFromDown", value);
            }
        }
        ///<summary>左から話かけられた時のイベント</summary>
        public string mSpeakFromLeft {
            get { return (mData.ContainsKey("speakFromLeft")) ? mData.get<string>("speakFromLeft") : ""; }
            set {
                if (value == "") mData.remove("speakFromLeft");
                else mData.set("speakFromLeft", value);
            }
        }
        ///<summary>右から話かけられた時のイベント</summary>
        public string mSpeakFromRight {
            get { return (mData.ContainsKey("speakFromRight")) ? mData.get<string>("speakFromRight") : ""; }
            set {
                if (value == "") mData.remove("speakFromRight");
                else mData.set("speakFromRight", value);
            }
        }
        ///<summary>話かけられた時のイベントを持つかどうか</summary>
        public bool mIsSpeaker {
            get { return mData.ContainsKey("speakDefault") || mData.ContainsKey("speakFromUp") || mData.ContainsKey("speakFromDown") || mData.ContainsKey("speakFromLeft") || mData.ContainsKey("speakFromRight"); }
        }
        /// <summary>オブジェクトに渡す変数</summary>
        public Arg mArg {
            get { return mData.ContainsKey("arg") ? mData.get<Arg>("arg") : null; }
            set {
                if (value != null) mData.set("arg", value);
                else mData.remove("arg");
            }
        }
        public Entity(Arg aData) {
            mData = aData;
        }
    }
    /// <summary>置物</summary>
    public class Ornament : Entity {
        public Ornament(Arg aData) : base(aData) {

        }
        public Ornament(Ornament aData) : base(new Arg(new Dictionary<string, object>((Dictionary<string, object>)aData.mData.dictionary))) {

        }
    }
    /// <summary>キャラクター</summary>
    public class Character : Entity {
        /// <summary>移動速度</summary>
        public float mMoveSpeed {
            get { return mData.ContainsKey("moveSpeed") ? mData.get<float>("moveSpeed") : MyMap.mDefaultMoveSpeed; }
            set { mData.set("moveSpeed", value); }
        }
        ///<summary>向き</summary>
        public Vector2 mDirection {
            get { return mData.ContainsKey("direction") ? mData.get<Vector2>("direction") : new Vector2(0, 0); }
            set { mData.set("direction", value); }
        }
        ///<summary>AIを表したタグ</summary>
        public MyTag mAi {
            get { return new MyTag(mData.get<string>("ai")); }
        }
        ///<summary>AIを表したタグ</summary>
        public string mAiString {
            set {
                if (value == "" || value == null) mData.remove("ai");
                else mData.set("ai", value);
            }
        }
        ///<summary>Stateを表したタグ</summary>
        public MyTag mState {
            get { return mData.ContainsKey("state") ? new MyTag(mData.get<string>("state")) : null; }
        }
        ///<summary>Stateを表したタグ</summary>
        public string mStateString {
            set {
                if (value == "" || value == null) mData.remove("state");
                else mData.set("state", value);
            }
        }
        public Character(Arg aData) : base(aData) {

        }
        public Character(Character aData) : base(new Arg(new Dictionary<string, object>((Dictionary<string, object>)aData.mData.dictionary))) {

        }
    }
    /// <summary>トリガー</summary>
    public class Trigger : Behaviour {
        ///<summary>トリガーの形状を表したタグ</summary>
        public MyTag mShape {
            get { return new MyTag(mData.get<string>("shape")); }
        }
        /// <summary>triggerを発火させるentityの名前(リストが空なら全てのentityがtriggerになる)</summary>
        public List<string> mTriggerKey {
            get { return mData.ContainsKey("triggerKey") ? mData.get<List<string>>("triggerKey") : new List<string>(); }
            set { mData.set("triggerKey", value); }
        }
        /// <summary>triggerKeyになるキャラとの衝突判定</summary>
        public string mCollisionType {
            get { return mData.get<string>("collisionType"); }
            set { mData.set("collisionType", value); }
        }
        /// <summary>trigger侵入時に発火するイベントのKey</summary>
        public string mEnterKey {
            get { return mData.ContainsKey("enterKey") ? mData.get<string>("enterKey") : ""; }
            set { mData.set("enterKey", value); }
        }
        /// <summary>trigger内部で停止時に発火するイベントのKey</summary>
        public string mStayKey {
            get { return mData.ContainsKey("stayKey") ? mData.get<string>("stayKey") : ""; }
            set { mData.set("stayKey", value); }
        }
        /// <summary>trigger内部で移動時に発火するイベントのKey</summary>
        public string mMovedKey {
            get { return mData.ContainsKey("movedKey") ? mData.get<string>("movedKey") : ""; }
            set { mData.set("movedKey", value); }
        }
        /// <summary>trigger内部から外部へ移動時に発火するイベントのKey</summary>
        public string mExitKey {
            get { return mData.ContainsKey("exitKey") ? mData.get<string>("exitKey") : ""; }
            set { mData.set("exitKey", value); }
        }

        public Trigger(Arg aData) {
            mData = aData;
        }
    }
    /// <summary>イベント</summary>
    public class Event {
        public Arg mData;
        public IDictionary mDic {
            get { return mData.dictionary; }
        }
        public Arg get(string aKey) {
            return mData.get<Arg>(aKey);
        }
        public void set(string aKey, Arg aData) {
            mData.set(aKey, aData);
        }
        public Event(Arg aData) {
            mData = aData;
        }
    }
    /// <summary>マップへの入り口</summary>
    public class Entrance {
        public Arg mData;
        ///<summary>x座標</summary>
        public float mX {
            get { return mData.get<float>("x"); }
            set { mData.set("x", value); }
        }
        ///<summary>y座標</summary>
        public float mY {
            get { return mData.get<float>("y"); }
            set { mData.set("y", value); }
        }
        ///<summary>高さ</summary>
        public float mHeight {
            get { return mData.get<float>("height"); }
            set { mData.set("height", value); }
        }
        /// <summary>移動先となりうる範囲のサイズ</summary>
        public Vector2 mSize {
            get { return mData.get<Vector2>("size"); }
            set { mData.set("size", value); }
        }
        /// <summary>マップ移動演出時の移動方向</summary>
        public Vector2 mMoveVector {
            get { return mData.get<Vector2>("moveVector"); }
            set { mData.set("moveVector", value); }
        }
        /// <summary>マップ移動演出時の移動速度(負の値:デフォルト値適用,0:移動演出を行わない)</summary>
        public float mMoveSpeed {
            get { return mData.ContainsKey("moveSpeed") ? mData.get<float>("moveSpeed") : MyMap.mDefaultMoveSpeed; }
            set {
                if (value < 0) mData.remove("moveSpeed");
                else mData.set("moveSpeed", value);
            }
        }
        /// <summary>マップ生成完了と同時に実行するイベントのKey</summary>
        public string mFirstEventKey {
            get { return mData.ContainsKey("firstEventKey") ? mData.get<string>("firstEventKey") : ""; }
            set { mData.set("firstEventKey", value); }
        }
        /// <summary>マップ移動演出終了後に実行するイベントのKey</summary>
        public string mEventKey {
            get { return mData.ContainsKey("eventKey") ? mData.get<string>("eventKey") : ""; }
            set { mData.set("eventKey", value); }
        }
        public Entrance(Arg aData) {
            mData = aData;
        }
    }
}
