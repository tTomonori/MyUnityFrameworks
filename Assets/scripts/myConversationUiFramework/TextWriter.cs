using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class TextWriter : MonoBehaviour {
    static float kScale = 100;
    private Board mBoard;
    private List<Line> mLines=new List<Line>();
    private RectTransform mRect;
    private RectTransform mLineContainer;
    private WriterReader mReader;
    private float mWaited = 0;
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
        createNewLine();
        mReader = new WriterReader();
        mColor = mFontColor;
        mSize = mFontSize;
	}
	
	void Update () {
        if (mReader.isEnd()) return;
        mWaited += Time.deltaTime;
        int tNum = Mathf.FloorToInt(mWaited / mWriteInterval);
        mWaited = mWaited % mWriteInterval;
        //文字表示
        for (int i = 0; i < tNum; i++)
            writeNextChar();
        //表示位置調整
        mBoard.updated();
	}
    //表示する文章を追加
    public void write(string aText){
        mReader.add(aText);
    }
    //次の文字を表示
    private void writeNextChar(){
        string tStr = getNextChar();
        if (tStr == "") return;
        Text tText = createText();
        tText.text = tStr;
        writeText(tText);
    }
    //次の文字を取得
    private string getNextChar(){
        string tNext;
        while(true){
            tNext = mReader.read();
            if(tNext=="<"){
                readTag();
                continue;
            }
            return tNext;
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