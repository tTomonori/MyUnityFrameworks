using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeshTextMain : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        TextMeshPro tp = GameObject.Find("tmp").GetComponent<TextMeshPro>();
        //Debug.Log(tp.renderedWidth + "," + tp.renderedHeight);
        tp = MyBehaviour.create<TextMeshPro>();
        tp.name = "created";
        tp.font = Resources.Load<TMP_FontAsset>("mplus-1c-bold SDF");
        tp.fontSize = 10;
        tp.rectTransform.position = new Vector3(0, 0, 0);
        tp.rectTransform.sizeDelta = new Vector2(1, 1);
        tp.text = "あ";
        Debug.Log(tp.preferredWidth + "," + tp.preferredHeight);

        GameObject p = GameObject.Find("p");
        p.transform.position = new Vector3(tp.renderedWidth, tp.renderedHeight);

        GameObject.Find("board").GetComponent<MeshTextBoard>().mTapCallback = (a) => { Debug.Log(a); };

        MeshTextBoardWriter tWriter = GameObject.Find("writer").GetComponent<MeshTextBoardWriter>();
        tWriter.mEndCallback = () => {
            tWriter.changeText(mText);
        };
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {
            MeshTextBoardWriter tWriter = GameObject.Find("writer").GetComponent<MeshTextBoardWriter>();
            tWriter.read();
        }
    }
    [System.NonSerialized] public string mText =
 "あれ<color,red>れ<size,2>れあ</size>んな<br><color,0,1,1,0.5>"
+"<animation,circle>くるくる</animation>"
+"kkj<br>"
+"<animation,impact>か<image,temp/cannotLoad>おん</animation><br>"
+"<highlight,テスト,red>テスト</highlight><br>"
+"あiu<u,0.1,green>ハハハ<reading,3,わらい><br>"
+"XYZ</u>んん<link,リンク,blue,0.1,red>LINK</link><br>"
+"<animation,tremble>あいうえお</animation><next>あああ<stop>"
+"<animation,impact>な<wait,1>ん<wait,1>て<wait,1>ね<wait,3></animation><br>"
+"<writeInterval,1>ゆっくり話そうねーー<br>"
+"んーーーー<resetWriteInterval><br>"
+"abcdefghij<next>";
}