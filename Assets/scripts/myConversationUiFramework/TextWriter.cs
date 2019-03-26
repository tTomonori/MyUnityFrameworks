using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class TextWriter : MonoBehaviour {
    //文字オブジェクトのscale
    static float kScale = 100;
    //表示位置管理
    private Board mBoard;
    //文字の行
    private List<Line> mLines=new List<Line>();
    //自分のtransform
    private RectTransform mRect;
    //Lineの親要素
    private RectTransform mLineContainer;
    private MyBehaviour mLineContainerBehaviour;
    //文字を１文字ずつ読む用
    private WriterReader mReader = new WriterReader();
    //文字を表示するタイミング管理用
    private float mWaited = 0;
    //全部表示し終わった時のコールバック
    public Action mCallback;
    //表示停止
    private bool mStop = false;
    //goが呼ばれた時の関数(タグ処理で設定する)
    private Action mOnGo;
    //終了フラグ
    private bool mIsEnd = true;
    private Color mColor;
    private int mSize;
    [SerializeField] public int mFontSize = 100;
    [SerializeField] public Color mFontColor = new Color(1, 1, 1, 1);
    [SerializeField] public Font mFont;
    [SerializeField] public float mWriteInterval = 0.1f;
    [SerializeField] public float mScrollSpeed = 2.5f;

	void Start () {
        mRect = GetComponent<RectTransform>();
        mBoard = MyBehaviour.create<Board>();
        mBoard.name = "board";
        mBoard.outer = this;
        mBoard.transform.SetParent(transform, false);
        mLineContainer = MyBehaviour.create<RectTransform>();
        mLineContainer.name = "lineContainer";
        mLineContainer.localPosition = new Vector3(-mRect.sizeDelta.x/2, mRect.sizeDelta.y/2, 0);
        mLineContainer.transform.SetParent(mBoard.transform, false);
        mLineContainerBehaviour = mLineContainer.gameObject.AddComponent<MyBehaviour>();
        createNewLine();
        mColor = mFontColor;
        mSize = mFontSize;
	}
	
	void Update () {
        if (mIsEnd || mStop) return;
        mWaited += Time.deltaTime;
        int tNum = Mathf.FloorToInt(mWaited / mWriteInterval);
        mWaited = mWaited % mWriteInterval;
        //文字表示
        writeNextString(tNum);
	}
    //読む,読み飛ばす
    public void go(){
        //タグの関数
        if(mOnGo!=null){
            mOnGo();
            mOnGo = null;
            writeNextString(1);
            return;
        }
        //読み飛ばす
        writeNextString(9999);
    }
    //表示する文章を追加
    public void write(string aText){
        mIsEnd = false;
        mReader.add(aText);
    }
    //表示リセット
    public void clear(){
        //行を削除
        mLineContainerBehaviour.deleteChildren();
        mLines.Clear();
        createNewLine();
        //表示位置をリセット
        mBoard.reset();
    }
    //指定文字数表示
    private void writeNextString(int aNum){
        for (int i = 0; i < aNum;i++){
            if (mStop) return;//書き込み停止
            if(mReader.isEnd()){//終了
                mIsEnd = true;
                mCallback();
                return;
            }
            //次の１文字表示
            writeNextChar();
        }
        mBoard.updated();
    }
    //次の文字を表示
    private void writeNextChar(){
        string tNext;
        while (!mStop){
            //次の１文字
            tNext = mReader.read();
            //終了
            if (tNext == "") return;
            //タグ
            if (tNext == "<"){
                readTag();
                continue;
            }
            //１文字表示
            Text tText = createText();
            tText.text = tNext;
            writeText(tText);
            return;
        }
    }
    //タグ処理
    private void readTag(){
        string[] tTag = mReader.readTo('>').Split(',');
        switch(tTag[0]){
            case "br"://改行
                createNewLine();
                break;
            case "color"://文字の色変更
                switch(tTag[1]){
                    case "red": mColor = new Color(1, 0, 0, 1); break;
                    case "green": mColor = new Color(0, 1, 0, 1); break;
                    case "blue":mColor = new Color(0, 0, 1, 1);break;
                }
                break;
            case "/color"://文字の色をデフォルトに戻す
                mColor = mFontColor;
                break;
            case "size"://文字サイズ
                mSize = (int)(float.Parse(tTag[1]) * mFontSize);
                break;
            case "/size"://文字サイズをデフォルトに戻す
                mSize = mFontSize;
                break;
            case "stop"://文字の書き込みを停止
                mStop = true;
                mOnGo = () => { mStop = false; };
                break;
            case "clear"://文字の書き込みを停止し,再開時に表示リセット
                mStop = true;
                mOnGo = () => { clear(); mStop = false; };
                break;
        }
    }
    //文字を表示
    public void writeText(Text aText){
        if (mLines[0].add(aText)) return;
        createNewLine();
        mLines[0].add(aText);
    }
    //新しい行を生成
    private void createNewLine(){
        RectTransform tRect = MyBehaviour.create<RectTransform>();
        tRect.name = "line";
        tRect.sizeDelta = new Vector2(mRect.sizeDelta.x, 1);
        Line tLine = new Line(tRect);
        tLine.mPositionY = (mLines.Count == 0) ? 0 : mLines[0].getBottom();
        tRect.SetParent(mLineContainer, false);
        mLines.Insert(0, tLine);
    }
    private Text createText(){
        Text tText = MyBehaviour.create<Text>();
        tText.rectTransform.sizeDelta = new Vector2(1, 1);
        tText.rectTransform.localScale = new Vector3(1/kScale, 1/kScale, 1/kScale);
        tText.rectTransform.pivot = new Vector2(0, 1);
        tText.fontSize = mSize;
        tText.color = mColor;
        tText.horizontalOverflow = HorizontalWrapMode.Overflow;
        tText.verticalOverflow = VerticalWrapMode.Overflow;
        tText.font = mFont;
        tText.gameObject.layer = gameObject.layer;
        return tText;
    }

    //一行
    public class Line {
        private RectTransform mRect;
        private float mRight;
        private float mHeight;
        private List<Text> mTexts=new List<Text>();
        private List<Text> mCloned = new List<Text>();
        public float mPositionY{
            get { return mRect.localPosition.y; }
            set{
                Vector3 tPosition = mRect.localPosition;
                mRect.localPosition = new Vector3(tPosition.x, value, tPosition.z);
            }
        }
        public Line(RectTransform aObject){
            mRect = aObject;
        }
        //文字を追加
        public bool add(Text aText){
            //はみ出る
            if (mRect.sizeDelta.x < mRight + aText.preferredWidth / TextWriter.kScale) return false;
            //表示
            aText.transform.SetParent(mRect.transform, false);
            aText.rectTransform.localPosition = new Vector3(mRight, -mHeight + aText.preferredHeight / TextWriter.kScale);
            mTexts.Add(aText);
            //上側で見切れても映るようにする
            Text tClone = Instantiate(aText);
            tClone.transform.SetParent(mRect.transform, false);
            tClone.alignment = TextAnchor.LowerLeft;
            tClone.rectTransform.localPosition = new Vector3(mRight, -mHeight);
            mCloned.Add(tClone);
                
            mRight += aText.preferredWidth / TextWriter.kScale;
            if (mHeight < aText.preferredHeight / TextWriter.kScale){
                mHeight = aText.preferredHeight / TextWriter.kScale;
                correctHeight();
            }
            return true;
        }
        //文字の高さを合わせる
        private void correctHeight(){
            foreach(Text tText in mTexts){
                tText.transform.localPosition = new Vector3(tText.transform.localPosition.x, -mHeight + tText.preferredHeight / TextWriter.kScale);
            }
            foreach (Text tText in mCloned){
                tText.transform.localPosition = new Vector3(tText.transform.localPosition.x, -mHeight);
            }
        }
        public float getBottom(){
            return mRect.localPosition.y - mHeight;
        }
    }

    //表示位置管理
    public class Board:MyBehaviour{
        public TextWriter outer;
        private Coroutine mMoveCoroutine;
        private float mToY=0;
        public void updated(){
            float tY = getCorrectY();
            if (tY <= mToY) return;
            mToY = tY;
            if (mMoveCoroutine != null) StopCoroutine(mMoveCoroutine);
            mMoveCoroutine = moveToWithSpeed(new Vector2(0, mToY), outer.mScrollSpeed);
        }
        private float getCorrectY(){
            return -outer.mLines[0].getBottom() - outer.mRect.sizeDelta.y;
        }
        public void reset(){
            if (mMoveCoroutine != null) StopCoroutine(mMoveCoroutine);
            positionY = 0;
            mToY = 0;
        }
    }

    //文字読み込み
    public class WriterReader{
        private StringReader mReader;
        public WriterReader(){
            mReader = new StringReader("");
        }
        //読む文字列を追加
        public void add(string aText){
            mReader = new StringReader(mReader.ReadToEnd() + aText);
        }
        //次の１文字を読む
        public string read(){
            int tStr = mReader.Read();
            if (tStr == -1) return "";
            return Convert.ToChar(tStr).ToString();
        }
        //指定した文字まで読み,指定した文字の前までの文字列を返す
        public string readTo(char c){
            string tRes = "";
            char tNext;
            while(true){
                tNext = Convert.ToChar(mReader.Read());
                if (tNext == c) break;
                tRes += tNext;
            }
            return tRes;
        }
        //全て読み終えているならtrue
        public bool isEnd(){
            return mReader.Peek() == -1;
        }
    }
}