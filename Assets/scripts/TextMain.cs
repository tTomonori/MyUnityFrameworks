using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMain : MonoBehaviour {

    // Use this for initialization
    void Start() {

        TextWriter tWriter = GameObject.Find("writer").GetComponent<TextWriter>();
        tWriter.write("おはよー!!これはテストだよー!!みってるー??<br>これから表示テストをするよー!!<br><color,blue>あお</color>色<color,red>あか</color>色<br>どうだ!<br>え、<size,2>うわっ</size><br>いやあ" +
        "<size,1.2>あ" +
        "<size,1.4>あ" +
        "<size,1.6>あ" +
        "<size,1.8>あ" +
        "<size,1.6>あ" +
        "<size,1.4>あ" +
        "<size,1.2>あ</size>  なんてことだ!!??<clear>" +
        "なんてね<image,conversation/cannotLoad>なんてね"
        );

        //MyConversationWondow tWindow = GameObject.Find("conversationWindow").GetComponent<MyConversationWondow>();
    }

    // Update is called once per frame
    void Update() {

    }
}