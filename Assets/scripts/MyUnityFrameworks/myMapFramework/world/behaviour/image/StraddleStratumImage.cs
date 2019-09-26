using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 階層を跨いで移動する
/// </summary>
public class StraddleStratumImage : MapStandImage {
    /// <summary>一階層分の高さ</summary>
    static private float kHeight = 1;
    /// <summary>imageの幅</summary>
    [SerializeField] public float mWidth;
    /// <summary>同時に跨ぐ階層の最大数</summary>
    [SerializeField] public int mMaxStraddleNum;
    /// <summary>マスク</summary>
    private MaskGroup[] mMaskGroups;
    /// <summary>現在の高さ</summary>
    public float mCurrentHeight;

    protected void Awake() {
        mMaskGroups = new MaskGroup[mMaxStraddleNum];
        for (int i = 0; i < mMaxStraddleNum; ++i) {
            if (i == 0) mMaskGroups[0] = MyBehaviour.create<BottomMaskGroup>();
            else if (i == mMaxStraddleNum - 1) mMaskGroups[i] = MyBehaviour.create<TopMaskGroup>();
            else mMaskGroups[i] = MyBehaviour.create<MiddleMaskGroup>();

            mMaskGroups[i].name = "mask" + i.ToString();
            mMaskGroups[i].transform.SetParent(this.transform, false);
            mMaskGroups[i].mWidth = mWidth;
            mMaskGroups[i].mMaskNum = i;
        }
    }

    public override void setHight(float aHeight) {
        mCurrentHeight = aHeight;
        for (int i = 0; i < mMaxStraddleNum; ++i) {
            mMaskGroups[i].mask(aHeight);
        }
    }

    public abstract class MaskGroup : MyBehaviour {
        //画像(マスクする範囲)の幅
        public float mWidth;
        //maskGroupの番号(一番したが0番)
        public int mMaskNum;
        protected MyBehaviour mSquareMask1;
        protected MyBehaviour mSquareMask2;
        protected MyBehaviour mTriangleMask;
        private void Awake() {
            //矩形マスク1
            mSquareMask1 = MyBehaviour.create<MyBehaviour>();
            mSquareMask1.name = "squareMask1";
            mSquareMask1.gameObject.AddComponent<SpriteMask>().sprite = MyMap.mSquareMask;
            mSquareMask1.transform.SetParent(this.transform, false);
            //矩形マスク2
            mSquareMask2 = MyBehaviour.create<MyBehaviour>();
            mSquareMask2.name = "squareMask2";
            mSquareMask2.gameObject.AddComponent<SpriteMask>().sprite = MyMap.mSquareMask;
            mSquareMask2.transform.SetParent(this.transform, false);
            //三角形マスク
            mTriangleMask = MyBehaviour.create<MyBehaviour>();
            mTriangleMask.name = "triangleMask";
            mTriangleMask.gameObject.AddComponent<SpriteMask>().sprite = MyMap.mTriangleMask;
            mTriangleMask.transform.SetParent(this.transform, false);
        }
        public abstract void mask(float aBottomHeight);
    }
    public class TopMaskGroup : MaskGroup {
        public override void mask(float aBottomHeight) {
            //表示範囲の底辺の高さ
            float tBottomHeight = kHeight - aBottomHeight.decimalPart() + kHeight * (mMaskNum - 1);
            //マスク画像の位置,大きさ
            mSquareMask1.position = new Vector2(0, aBottomHeight);
            mSquareMask1.scale = new Vector3(mWidth, kHeight * 10, 1);
            mSquareMask1.gameObject.layer = MyMap.mStratumLayerNum[Mathf.FloorToInt(aBottomHeight) + mMaskNum];

            mSquareMask2.scale = Vector3.zero;
            mTriangleMask.scale = Vector3.zero;
        }
    }
    public class MiddleMaskGroup : MaskGroup {
        public override void mask(float aBottomHeight) {
            //表示範囲の底辺の高さ
            float tBottomHeight = kHeight - aBottomHeight.decimalPart() + kHeight * (mMaskNum - 1);
            //マスク画像の位置,大きさ
            mSquareMask1.position = new Vector2(0, aBottomHeight);
            mSquareMask1.scale = new Vector3(mWidth, kHeight, 1);
            mSquareMask1.gameObject.layer = MyMap.mStratumLayerNum[Mathf.FloorToInt(aBottomHeight) + mMaskNum];

            mSquareMask2.scale = Vector3.zero;
            mTriangleMask.scale = Vector3.zero;
        }
    }
    public class BottomMaskGroup : MaskGroup {
        public override void mask(float aBottomHeight) {
            if (aBottomHeight < 0) {
                mSquareMask1.scale = Vector3.zero;
                mSquareMask2.scale = Vector3.zero;
                mTriangleMask.scale = Vector3.zero;
            }

            //表示範囲の高さ
            float aDisplayHeight = kHeight - aBottomHeight.decimalPart();
            //マスク画像の位置,大きさ
            mSquareMask1.position = Vector2.zero;
            mSquareMask1.scale = new Vector3(mWidth, aDisplayHeight, 1);
            mSquareMask1.gameObject.layer = MyMap.mStratumLayerNum[Mathf.FloorToInt(aBottomHeight)];

            mSquareMask2.scale = Vector3.zero;
            mTriangleMask.scale = Vector3.zero;
        }
    }
}
