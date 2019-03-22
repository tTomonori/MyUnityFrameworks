using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataFile {
    private Arg mData;
    private List<Stratum> mStratums;
    private List<EncountData> mEncountDataList;
    public string name{
        get { return mData.get<string>("name"); }
    }
    public List<Stratum> stratums{
        get { return mStratums; }
    }
    public Arg chip{
        get { return mData.get<Arg>("chip"); }
    }
    public Arg events{
        get { return mData.get<Arg>("event"); }
    }
    public List<EncountData> encountData{
        get { return mEncountDataList; }
    }
    public MapDataFile(string aPath){
        //ファイルロード
        mData = new Arg(MyJson.deserializeFile("Assets/resources/mymap/map/" + aPath + ".json"));
        //階層データ
        mStratums = new List<Stratum>();
        foreach (Arg tData in mData.get<List<Arg>>("stratum")){
            mStratums.Add(new Stratum(tData));
        }
        //エンカウントデータ
        mEncountDataList = new List<EncountData>();
        foreach(Arg tData in mData.get<List<Arg>>("encount")){
            mEncountDataList.Add(new EncountData(tData));
        }
    }

    public class Stratum{
        private Arg mData;
        public List<List<int>> mFeild{
            get { return mData.get<List<List<int>>>("feild"); }
        }
        public List<Arg> mOrnament{
            get { return mData.get<List<Arg>>("ornament"); }
        }
        public List<Arg> mNpc{
            get { return mData.get<List<Arg>>("npc"); }
        }
        public List<Arg> mTrigger{
            get { return mData.get<List<Arg>>("trigger"); }
        }
        public Stratum(Arg aData){
            mData = aData;
        }
    }
    public class EncountData{
        private Arg mData;
        private List<Vector3> mTroutList;
        public List<Vector3> mTrout{
            get { return mTroutList; }
        }
        public float mMagnification{
            get { return mData.get<float>("magnification"); }
        }
        public Arg mEncountData{
            get { return mData.get<Arg>("data"); }
        }
        public EncountData(Arg aData){
            mData = aData;
            mTroutList = new List<Vector3>();
            foreach(List<int> tPosition in mData.get<List<List<int>>>("trout")){
                mTroutList.Add(new Vector3(tPosition[0], tPosition[1], tPosition[2]));
            }
        }
    }
}
