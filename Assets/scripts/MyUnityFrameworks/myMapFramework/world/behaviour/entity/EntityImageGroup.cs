using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class EntityImageGroup<Element> : MyBehaviour where Element : EntityImage {
    static private float kHeight = 1;
    static private float kZOrderInterval = -100;

    protected float mWidth;
    protected MyBehaviour[] mCutGroup;
    protected Element[] mImages;
    protected MyBehaviour[] mSquareMasks;
    protected MyBehaviour[] mTriangleMasks;
    virtual public void make(EntityImageData aImageData) {
        mWidth = aImageData.mWidth;
        int tCutNum = aImageData.mCutNum;
        mCutGroup = new MyBehaviour[tCutNum];
        mImages = new Element[tCutNum];
        mSquareMasks = new MyBehaviour[tCutNum];
        mTriangleMasks = new MyBehaviour[tCutNum];

        for (int i = 0; i < tCutNum; ++i) {
            mCutGroup[i] = MyBehaviour.create<MyBehaviour>();
            mCutGroup[i].gameObject.AddComponent<SortingGroup>();
            mCutGroup[i].transform.SetParent(this.transform, false);
            mCutGroup[i].positionZ = i * kZOrderInterval;
            mCutGroup[i].name = "cutGroup" + i.ToString();
            mImages[i] = GameObject.Instantiate<Element>((Element)aImageData.mImage);
            mImages[i].transform.SetParent(mCutGroup[i].transform, false);
            mImages[i].name = "image";
            mSquareMasks[i] = MyBehaviour.create<MyBehaviour>();
            mSquareMasks[i].gameObject.AddComponent<SpriteMask>().sprite = Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/system/squareMask");
            mSquareMasks[i].transform.SetParent(mCutGroup[i].transform, false);
            mSquareMasks[i].name = "squareMask";
            mTriangleMasks[i] = MyBehaviour.create<MyBehaviour>();
            mTriangleMasks[i].gameObject.AddComponent<SpriteMask>().sprite = Resources.Load<Sprite>(MyMap.mMapResourcesDirectory + "/system/triangleMask");
            mTriangleMasks[i].transform.SetParent(mCutGroup[i].transform, false);
            mTriangleMasks[i].name = "triangleMask";
        }
        foreach (SpriteRenderer tRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            tRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
    public void setUpHigh(float aHeight) {
        mSquareMasks[0].scaleX = mWidth;
        mSquareMasks[0].scaleY = 1f - aHeight;
        mSquareMasks[0].positionY = 0;
        mTriangleMasks[0].scale = new Vector3(0, 0, 1);
        for (int i = 1; i < mCutGroup.Length - 1; ++i) {
            mSquareMasks[i].scaleX = mWidth;
            mSquareMasks[i].scaleY = 1;
            mSquareMasks[i].positionY = (1f - aHeight) + (i - 1) * 1;
            mTriangleMasks[i].scale = new Vector3(0, 0, 1);
        }
        mSquareMasks[mCutGroup.Length - 1].scaleX = mWidth;
        mSquareMasks[mCutGroup.Length - 1].scaleY = 4;
        mSquareMasks[mCutGroup.Length - 1].positionY = (1f - aHeight) + (mCutGroup.Length - 2) * 1;
        mTriangleMasks[mCutGroup.Length - 1].scale = new Vector3(0, 0, 1);
    }
    public void setLeftHigh(float aBottomPoint) {

    }
    public void setLeftRight(float aBottomPoint) {

    }
    public void fiddleImage(Action<Element> aFunction) {
        for(int i = 0; i < mCutGroup.Length; ++i) {
            aFunction(mImages[i]);
        }
    }
}

