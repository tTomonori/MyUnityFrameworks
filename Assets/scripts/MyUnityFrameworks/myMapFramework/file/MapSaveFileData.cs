using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaveFileData : MapFileData {
    /// <summary>マップファイルへのパス</summary>
    public string mFilePath;
    /// <summary>エンカウントのカウント</summary>
    public float mEncountCount;

    public MapSaveFileData() : base() {

    }
    public MapSaveFileData(string aFilePath) : base() {
        load(new Arg(MyJson.deserializeFile(aFilePath)));
        //マップファイルへのパス
        mFilePath = mData.get<string>("filePath");
        //エンカウント
        mEncountCount = mData.get<float>("encountCount");
    }
    /// <summary>保持内容をArgにまとめる</summary>
    public override Arg createDic() {
        Arg tDic = base.createDic();
        tDic.set("filePath", mFilePath);
        tDic.set("encountCount", mEncountCount);

        return tDic;
    }
}
