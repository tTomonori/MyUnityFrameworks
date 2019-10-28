using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaveFileData {
    private Arg mData;
    /// <summary>マップファイルへのパス</summary>
    public string mFilePath;
    /// <summary>エンカウントのカウント</summary>
    public float mEncountCount;
    /// <summary>ornamentデータ</summary>
    public List<SavedOrnament> mOrnaments;
    /// <summary>npcデータ</summary>
    public List<SavedNpc> mNpcs;

    public MapSaveFileData() {
        mData = new Arg();
        mOrnaments = new List<SavedOrnament>();
        mNpcs = new List<SavedNpc>();
    }
    public MapSaveFileData(string aFilePath) {
        //ファイルロード
        mData = new Arg(MyJson.deserializeFile("Assets/resources/" + aFilePath + ".json"));
        //マップファイルへのパス
        mFilePath = mData.get<string>("filePath");
        //エンカウント
        mEncountCount = mData.get<float>("encountCount");
        //ornament
        mOrnaments = new List<SavedOrnament>();
        foreach (Arg tData in mData.get<List<Arg>>("ornament")) {
            mOrnaments.Add(new SavedOrnament(tData));
        }
        //npc
        mNpcs = new List<SavedNpc>();
        foreach (Arg tData in mData.get<List<Arg>>("npc")) {
            mNpcs.Add(new SavedNpc(tData));
        }
    }
    /// <summary>保持内容をArgにまとめる</summary>
    public Arg createDic() {
        Arg tDic = new Arg();
        tDic.set("filePath", mFilePath);
        tDic.set("encountCount", mEncountCount);

        List<Arg> tList = new List<Arg>();
        //ornament
        foreach (SavedOrnament tOrnament in mOrnaments) {
            tList.Add(tOrnament.mData);
        }
        tDic.set("ornament", tList);
        //npc
        tList = new List<Arg>();
        foreach (SavedNpc tNpc in mNpcs) {
            tList.Add(tNpc.mData);
        }
        tDic.set("npc", tList);
        return tDic;
    }
    public class SavedOrnament : MapFileData.Ornament {
        public SavedOrnament(Arg aData) : base(aData) { }
        public SavedOrnament(MapFileData.Ornament aData) : base(aData) {
            if (!(aData is SavedOrnament)) return;
            mData.remove("save");
        }
        public Arg mSave {
            get { return mData.ContainsKey("save") ? mData.get<Arg>("save") : null; }
            set {
                if (value == null) mData.remove("save");
                else mData.set("save", value);
            }
        }
    }
    public class SavedNpc : MapFileData.Character {
        public SavedNpc(Arg aData) : base(aData) { }
        public SavedNpc(MapFileData.Character aData) : base(aData) {
            if (!(aData is SavedNpc)) return;
            mData.remove("save");
        }
        public Arg mSave {
            get { return mData.ContainsKey("save") ? mData.get<Arg>("save") : null; }
            set {
                if (value == null) mData.remove("save");
                else mData.set("save", value);
            }
        }
    }
}
