using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject map = GameObject.Find("map");
        Arg tSave = new Arg(MyJson.deserializeResourse("save/save"));

        MyMap.mDisplay = map;

        MyMapInputPad pad = GameObject.Find("pad").GetComponent<MyMapInputPad>();
        MyMap.MyMapController controller = new MyMap.MyMapController();
        pad.mController = controller;
        MyMap.mController = controller;

        MyMap.mCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        MyMap.mEventHandler = new TestHandler();

        MyMap.load(tSave);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class TestHandler:MyMapEventHandler{
    private MyBehaviour mFade;
    /// <summary>
    /// マップ移動イベント発火通知
    /// </summary>
    /// <param name="aData">イベントデータ</param>
    /// <param name="aOnFadeOutEnd">フェードアウト演出完了時に呼ぶ</param>
    public void onMoveMap(Arg aData, Action aOnFadeOutEnd){
        mFade = MyBehaviour.create<MyBehaviour>();
        mFade.positionZ = 1;
        SpriteRenderer tRenderer = mFade.gameObject.AddComponent<SpriteRenderer>();
        tRenderer.color = new Color(0.4f,0.4f,1);
        tRenderer.sprite = Resources.Load<Sprite>("myMap/tile/sprites/grass0");
        mFade.transform.SetParent(GameObject.Find("Main Camera").transform, false);
        mFade.scaleBy(new Vector3(20, 20), 1, aOnFadeOutEnd);
        mFade.rotateForever(360);
    }
    /// <summary>
    /// マップ移動イベント時の新しいマップ生成完了通知
    /// </summary>
    /// <param name="aOnFadeInEnd">フェードイン演出終了時に呼ぶ</param>
    public void onCreatedMap(Action aOnFadeInEnd){
        mFade.scaleBy(new Vector3(-21, -21), 1, () => {
            mFade.delete();
        });
        MyBehaviour.setTimeoutToIns(0.5f, aOnFadeInEnd);
    }
    /// <summary>
    /// 戦闘イベント発火通知
    /// </summary>
    /// <param name="aData">戦闘データ</param>
    /// <param name="aEndBattle">戦闘終了時に呼ぶ</param>
    public void onBattleStart(Arg aData, Action<bool> aEndBattle){
        Debug.Log("battle : "+aData.get<string>("test"));
        aEndBattle(true);
    }
}