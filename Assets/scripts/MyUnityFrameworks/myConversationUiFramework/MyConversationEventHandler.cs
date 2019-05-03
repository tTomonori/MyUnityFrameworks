using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyConversationEventHandler {
    public MyConversationWondow mWindow;
    //<summary>会話イベントを処理</summary>
    public bool on(Arg aData,Action<string> aCallback){
        if (aData.get<string>("event") != "conversation") return false;
        mWindow.run(aData, aCallback);
        return true;
    }
}
