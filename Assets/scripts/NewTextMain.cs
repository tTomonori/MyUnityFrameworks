using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTextMain : MonoBehaviour {
    TextDisplay mDisplay;
    // Start is called before the first frame update
    void Start() {
        mDisplay = GameObject.Find("textDisplay").GetComponent<TextDisplay>();
        mDisplay.mOnRead = () => {
            mDisplay.clear();
            mDisplay.display("いくよ?<clear>せーの<stop>テストー<br>!!!!!<next>");
        };
        mDisplay.display("じゅ<waite,0.5>ん<waite,0.5>び<waite,0.5>は<waite,0.5>いい？<stop><br>いい<size,1.5>よー</size>!<next>");
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnMouseUpAsButton() {
        mDisplay.read();
    }
}
