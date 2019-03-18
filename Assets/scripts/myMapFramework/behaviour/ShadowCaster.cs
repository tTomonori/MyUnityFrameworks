using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCaster : MapBehaviour {
    private SpriteRenderer mShadow;
    [SerializeField] private Sprite mShadowForm;
    [SerializeField] private float mOffsetZ;
    [SerializeField] private int mTargetStratumNum;

	void Start () {
        MyBehaviour tBehaviour = MyBehaviour.create<MyBehaviour>();
        mShadow = tBehaviour.gameObject.AddComponent<SpriteRenderer>();
        mShadow.sprite = mShadowForm;
        mShadow.color = new Color(0, 0, 0, 0.4f);

        int tDifference = mStratum.stratumNum - mTargetStratumNum;
        if (tDifference > 0){
            tBehaviour.position = new Vector3(0, -tDifference, -1+mOffsetZ);
        }else{
            tBehaviour.position = new Vector3(0.1f, -0.1f, 0.001f);
        }
        tBehaviour.transform.SetParent(transform, false);
    }
}