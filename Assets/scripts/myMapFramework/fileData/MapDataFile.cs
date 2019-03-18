using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataFile {
    private Arg mData;
    public List<Arg> stratum{
        get { return mData.get<List<Arg>>("stratum"); }
    }
    public List<List<int>> feild{
        get { return mData.get<List<List<int>>>("feild"); }
    }
    public Arg chip{
        get { return mData.get<Arg>("chip"); }
    }
    public List<Arg> objects{
        get { return mData.get<List<Arg>>("object"); }
    }
    public MapDataFile(string aPath){
        mData = new Arg(MyJson.deserializeFile("Assets/resources/mymap/map/" + aPath + ".json"));
    }
}
