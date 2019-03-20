using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataFile {
    private Arg mData;
    private List<Stratum> mStratums;
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
    public MapDataFile(string aPath){
        mData = new Arg(MyJson.deserializeFile("Assets/resources/mymap/map/" + aPath + ".json"));
        mStratums = new List<Stratum>();
        foreach (Arg tData in mData.get<List<Arg>>("stratum")){
            mStratums.Add(new Stratum(tData));
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
}
