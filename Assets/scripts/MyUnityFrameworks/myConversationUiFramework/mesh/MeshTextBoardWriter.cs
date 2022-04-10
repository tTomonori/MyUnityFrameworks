using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 利用可能なタグリスト
/// <clear>表示削除
/// <stop>停止
/// <next>停止、再開時に表示削除
/// <wait,1>指定秒数停止
/// <writeInterval,0.1>追加表示間隔変更
/// <resetWriteInterval>追加表示間隔をデフォルトに戻す
/// 
/// + MeshTextBoard対応のタグ
/// </summary>
public class MeshTextBoardWriter : MeshTextBoard {
    private WritingStatus mCurrentWritingStatus;
    private TagReader mReader;
    private float mElapsedTime = 0;
    private float mLeftWaitTime = 0;
    private float mCurrentWriteInterval;
    //<summary>全て表示が終わったときのコールバック</summary>
    public Action mEndCallback;
    //<summary>デフォルト文字表示速度</summary>
    public float mDefaultWriteInterval {
        get { return _DefaultWriteInterval; }
        set { _DefaultWriteInterval = value; }
    }
    [SerializeField] private float _DefaultWriteInterval = 0.1f;
    public enum WritingStatus { writing, stopping, stopNext, waiting, ended };
    private void Awake() {
        mReader = new TagReader(mText);
        mCurrentWritingStatus = WritingStatus.writing;
        mCurrentWriteInterval = mDefaultWriteInterval;
        reset();
    }
    private void Update() {
        switch (mCurrentWritingStatus) {
            case WritingStatus.writing:
                mElapsedTime += Time.deltaTime;
                break;
            case WritingStatus.stopping:
                return;
            case WritingStatus.stopNext:
                return;
            case WritingStatus.waiting:
                mLeftWaitTime -= Time.deltaTime;
                if (mLeftWaitTime > 0) return;
                mCurrentWritingStatus = WritingStatus.writing;
                mElapsedTime += -mLeftWaitTime;
                mLeftWaitTime = 0;
                break;
            case WritingStatus.ended:
                return;
        }
        //次を表示
        float tRemainder = write(mElapsedTime);
        mElapsedTime = tRemainder;
    }
    ///<summary>表示文字を追加</summary>
    public void addText(string aText) {
        mReader.add(aText);
        if (mCurrentWritingStatus == WritingStatus.ended) {
            write(1);
        }
    }
    ///<summary>表示文字を変更</summary>
    public void changeText(string aText) {
        clear();
        mReader = new TagReader(aText);
        if (mCurrentWritingStatus == WritingStatus.ended) {
            write(1);
        }
    }
    ///<summary>文字送り,停止を解除して表示再開</summary>
    public void read() {
        switch (mCurrentWritingStatus) {
            case WritingStatus.writing:
                writeSkip();
                break;
            case WritingStatus.stopping:
                mCurrentWritingStatus = WritingStatus.writing;
                write(1);
                return;
            case WritingStatus.stopNext:
                mCurrentWritingStatus = WritingStatus.writing;
                clear();
                write(1);
                return;
            case WritingStatus.waiting:
                mCurrentWritingStatus = WritingStatus.writing;
                mLeftWaitTime = 0;
                write(1);
                break;
            case WritingStatus.ended:
                mEndCallback();
                return;
        }
    }
    ///<summary>次の要素を指定数適用</summary>
    public void write(int aNum) {
        mCurrentWritingStatus = WritingStatus.writing;
        int tDisplayedElementNum = 0;
        TagReader.Element tElement;
        while (tDisplayedElementNum < aNum) {
            tElement = writeNext();
            if (mCurrentWritingStatus != WritingStatus.writing)
                return;
            if (isDisplayElement(tElement)) {
                tDisplayedElementNum++;
            }
        }
    }
    ///<summary>次の要素を指定時間経過分適用(余った時間を返す)</summary>
    public float write(float aTime) {
        float tRemainder = aTime;
        TagReader.Element tElement;
        while (tRemainder >= mCurrentWriteInterval) {
            tRemainder -= mCurrentWriteInterval;
            //表示する要素が表示されるまで適用
            while (true) {
                tElement = writeNext();
                if (mCurrentWritingStatus != WritingStatus.writing)
                    return 0;
                if (isDisplayElement(tElement)) break;
            }
        }
        return tRemainder;
    }
    ///<summary>次の要素を停止またはWriteInterval変更まで適用</summary>
    public void writeSkip() {
        float tPreInterval = mCurrentWriteInterval;
        TagReader.Element tElement;
        while (true) {
            //表示する要素が表示されるまで適用
            while (true) {
                tElement = writeNext();
                if (mCurrentWritingStatus != WritingStatus.writing)
                    return;
                if (isDisplayElement(tElement)) break;
            }
            if (tPreInterval != mCurrentWriteInterval) break;
        }
    }
    ///<summary>次の要素を1つ適用</summary>
    private TagReader.Element writeNext() {
        //全て表示済み
        if (mReader.isEnd()) {
            mCurrentWritingStatus = WritingStatus.ended;
            mElapsedTime = 0;
            mEndCallback();
            return null;
        }
        bool isValidTag = false;
        TagReader.Element tElement = mReader.read();
        //改行は無視して次を表示
        if (tElement is TagReader.OneChar && ((TagReader.OneChar)tElement).mChar == "\n") {
            return writeNext();
        }
        //開始タグ
        if (tElement is TagReader.StartTag) {
            isValidTag = applyWriterTag((TagReader.StartTag)tElement);
        }
        addLast(tElement);
        return tElement;
    }
    ///<summary>タグ適用(適用可能なタグだった場合true)</summary>
    private bool applyWriterTag(TagReader.StartTag aTag) {
        switch (aTag.mTagName) {
            case "clear":
                clear();
                return true;
            case "stop":
                mCurrentWritingStatus = WritingStatus.stopping;
                mElapsedTime = 0;
                return true;
            case "next":
                mCurrentWritingStatus = WritingStatus.stopNext;
                mElapsedTime = 0;
                return true;
            case "wait":
                mCurrentWritingStatus = WritingStatus.waiting;
                mLeftWaitTime = float.Parse(aTag.mArguments[0]);
                mElapsedTime = 0;
                break;
            case "writeInterval":
                mCurrentWriteInterval = float.Parse(aTag.mArguments[0]);
                mElapsedTime = 0;
                return true;
            case "resetWriteInterval":
                mCurrentWriteInterval = mDefaultWriteInterval;
                mElapsedTime = 0;
                return true;
        }
        return false;
    }
    ///<summary>表示する要素の場合にtrue</summary>
    public bool isDisplayElement(TagReader.Element aElement) {
        return aElement is TagReader.OneChar || (aElement is TagReader.StartTag && ((TagReader.StartTag)aElement).mTagName == "image");
    }
}
