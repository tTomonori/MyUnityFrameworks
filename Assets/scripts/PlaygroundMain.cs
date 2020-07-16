using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaygroundMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TextMeshPro tp = GameObject.Find("GameObject").GetComponent<TextMeshPro>();
        Debug.Log(tp.ignoreRectMaskCulling);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
