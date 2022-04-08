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

        float f;
        bool b = float.TryParse("a", out f);
        int i = 1;
    }

    // Update is called once per frame
    void Update() {

    }
}
