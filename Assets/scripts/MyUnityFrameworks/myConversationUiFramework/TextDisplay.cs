using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 利用可能なタグリスト
/// <clear>表示削除
/// <stop>停止
/// <next>停止、再開時に表示削除
/// <waite,1>指定秒数停止
/// 
/// + MyTextBoard対応のタグ
/// </summary>
public class TextDisplay : MyBehaviour {
    //<summary>テキスト表示速度(0以下の場合は即全てのテキストを表示)</summary>
    [SerializeField] public float mWriteSpeed = 0.1f;
    //<summary>デフォルトフォント</summary>
    [SerializeField] public Font mFont;

    //<summary>テキストを読み終わった時のコールバック</summary>
    public Action mOnRead;

    //<summary>テキスト表示領域</summary>
    public MyTextBoard mBoard { get; set; }
    //表示するテキストを1文字ずつ読む
    private TagReader mReader;
    //最後に文字を表示してからの経過時間
    private float mElapsedTime = 0;
    //文字追記状態
    private WritingState mWritingState = WritingState.end;
    private enum WritingState {
        writing, stop, waiteNext, waite, end
    }
    private void Awake() {
        //テキスト表示領域生成
        mBoard = MyBehaviour.create<MyTextBoard>();
        mBoard.transform.SetParent(this.transform, false);
        mBoard.name = this.name + ":TextBoard";
        mBoard.mFont = mFont;
        adjustBoardSize();
    }
    //<summary>表示領域をこのオブジェクトのサイズに合わせて再調整</summary>
    public void adjustBoardSize() {
        mBoard.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
    }
    ///<summary>テキストを表示(既に表示されているテキストはそのまま)</summary>
    public void display(string aText) {
        mReader = new TagReader(aText);
        mElapsedTime = 0;
        mWritingState = WritingState.writing;
    }
    //<summary>表示されているテキストを削除</summary>
    public void clear() {
        mBoard.mText = "";
    }
    //<summary>次の1文字を表示</summary>
    public void next() {
        while (true) {//1文字表示するまでループ
            if (mWritingState != WritingState.writing) return;//表示停止中
            if (mReader.isEnd()) {
                //全て表示終了している
                mWritingState = WritingState.end;
                //読み終わりコールバック
                if (mOnRead != null) mOnRead();
            }
            //次の1文字もしくはタグを読んで表示
            TagReader.Element tElement = mReader.read();
            if (tElement is TagReader.OneChar) {
                //文字1文字
                mBoard.addText(((TagReader.OneChar)tElement).mChar);
                return;
            } else if (tElement is TagReader.StartTag) {
                //開始タグ
                if (!applyStartTag((TagReader.StartTag)tElement))
                    mBoard.addText(((TagReader.StartTag)tElement).mOriginalString);
                continue;
            } else if (tElement is TagReader.EndTag) {
                //終了タグ
                if (!applyEndTag((TagReader.EndTag)tElement))
                    mBoard.addText(((TagReader.EndTag)tElement).mOriginalString);
                continue;
            }
        }
    }
    //<summary>表示を停止するところまで表示、もしくは停止を解除し続きを表示</summary>
    public void read() {
        switch (mWritingState) {
            case WritingState.writing://追記中
                //停止するまでスキップ
                while (mWritingState == WritingState.writing) {
                    next();
                }
                return;
            case WritingState.stop://停止中
                //続きを表示
                mElapsedTime = 0;
                mWritingState = WritingState.writing;
                return;
            case WritingState.waiteNext://ページ送り待ち中
                //表示削除してから続きを表示
                clear();
                mElapsedTime = 0;
                mWritingState = WritingState.writing;
                return;
            case WritingState.waite://待ち中
                return;
            case WritingState.end://表示完了済み
                return;
        }
    }

    private void Update() {
        //追記停止中
        if (mWritingState != WritingState.writing) return;
        //停止するまで全て表示
        if (mWriteSpeed <= 0) {
            read();
            return;
        }
        //経過時間カウント
        mElapsedTime += Time.deltaTime;
        //経過時間に応じて文字表示
        for (; mElapsedTime >= mWriteSpeed; mElapsedTime -= mWriteSpeed) {
            if (mWritingState != WritingState.writing) return;
            next();
        }
    }
    //開始タグ適用(未対応のタグの場合はfalseを返す)
    private bool applyStartTag(TagReader.StartTag aTag) {
        switch (aTag.mTagName) {
            case "clear"://表示削除
                clear();
                return true;
            case "stop"://停止
                mWritingState = WritingState.stop;
                return true;
            case "next"://停止、再開時に表示削除
                mWritingState = WritingState.waiteNext;
                return true;
            case "waite"://指定秒数停止
                mWritingState = WritingState.waite;
                this.setTimeout(float.Parse(aTag.mArguments[0]), () => {
                    if (mWritingState == WritingState.waite)
                        mWritingState = WritingState.writing;
                });
                return true;
            default:
                return false;
        }
    }
    //終了タグ適用(未対応のタグの場合はfalseを返す)
    private bool applyEndTag(TagReader.EndTag aTag) {
        switch (aTag.mTagName) {
            default:
                return false;
        }
    }
}
