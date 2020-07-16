using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/// <summary>
/// 利用可能なタグリスト
/// <size,2>文字サイズ変更(デフォルトフォントサイズの倍率)
/// </size>文字サイズをデフォルトに戻す
/// 
/// <color,red>文字カラー変更(red,green,blue,black,white,yellow,orange,purple)
/// <color,0,0,1,1>文字カラー変更(第四引数は省略可(=1))
/// </color>文字カラーをデフォルトに戻す
/// 
/// <br>改行
/// 
/// <u,0.2,1,0,0,1>アンダーライン(第二引数:太さ, 第三~六引数:色・省略時は文字カラーと同じ)
/// </u>アンダーライン終了
/// 
/// <image,path>画像(高さ=フォントサイズで表示)(path → resouces/ + X)
/// 
/// <reading,2,かな>読み仮名(このタグの直前の第二引数文字数に対して第三引数の仮名を振る)
/// 
/// <collider,引数,>文字にcolliderを付ける(触れたら引数をcallback)
/// </collider>colliderを付けない
/// <highlight,引数,1,0,0,1>collider + color
/// </highlight>/collider + /color
/// <link,引数,1,0,0,1,0.1>collider + color + u
/// </link>/collider + /color + /u
/// 
/// <animate,...>アニメーション
///     <animate,rotate,180>回転
///     <animate,tremble,10>揺れる
/// </animate>アニメーションキャンセル
/// </summary>
public class MeshTextBoard : MyBehaviour {
    /// <summary>文字の高さが1になるFontSize</summary>
    static private float kUnitFontSize = 10;

    private MyBehaviour mWriting;
    private List<Line> mLines = new List<Line>();

    private float mCurrentFontHeight;
    private Color mCurrentFontColor;
    private SpriteRenderer mCurrentUnderLine;
    private TagReader.StartTag mCurrentAnimate;
    private string mCurrentColliderArgument = "";

    /// <summary>文字に触れた時のcallback</summary>
    public Action<string> mTapCallback;

    //<summary>デフォルトフォント</summary>
    public TMP_FontAsset mDefaultFontAsset {
        get { return _DefaultFontAsset; }
        set { _DefaultFontAsset = value; regenerate(); }
    }
    [SerializeField] private TMP_FontAsset _DefaultFontAsset;
    //<summary>デフォルト文字サイズ</summary>
    public int mDefaultFontHeight {
        get { return _DefaultFontHeight; }
        set { _DefaultFontHeight = value; regenerate(); }
    }
    [SerializeField] private int _DefaultFontHeight = 1;
    //<summary>デフォルト文字カラー</summary>
    public Color mDefaultFontColor {
        get { return _DefaultFontColor; }
        set { _DefaultFontColor = value; regenerate(); }
    }
    [SerializeField] private Color _DefaultFontColor = new Color(0, 0, 0, 1);
    //<summary>行間隔</summary>
    public float mLineSpacing {
        get { return _LineSpacing; }
        set { _LineSpacing = value; regenerate(); }
    }
    [SerializeField] private float _LineSpacing = 0.3f;
    //<summary>一行の幅(0以下の場合は改行しない)</summary>
    public float mLineWidth {
        get { return _LineWidth; }
        set { _LineWidth = value; regenerate(); }
    }
    [SerializeField] private float _LineWidth = 5;
    //<summary>読み仮名のサイズ</summary>
    public float mReadingFontSize {
        get { return _ReadingFontSize; }
        set { _ReadingFontSize = value; regenerate(); }
    }
    [SerializeField] private float _ReadingFontSize = 0.3f;
    /// <summary>横方向の揃え</summary>
    public HorizontalAlign mHorizontalAlign {
        get { return _HorizontalAlign; }
        set { _HorizontalAlign = value; regenerate(); }
    }
    [SerializeField] private HorizontalAlign _HorizontalAlign = HorizontalAlign.left;
    public enum HorizontalAlign { left, center, right }
    /// <summary>縦方向の揃え</summary>
    public VerticalAlign mVerticalAlign {
        get { return _VerticalAlign; }
        set { _VerticalAlign = value; regenerate(); }
    }
    [SerializeField] private VerticalAlign _VerticalAlign = VerticalAlign.top;
    public enum VerticalAlign { top, middle, bottom }
    /// <summary>文字の揃え</summary>
    public CharacterAlign mCharacterAlign {
        get { return _CharacterAlign; }
        set { _CharacterAlign = value; regenerate(); }
    }
    [SerializeField] private CharacterAlign _CharacterAlign = CharacterAlign.midLine;
    public enum CharacterAlign { middle, midLine, capLine }

    //<summary>表示する文字列</summary>
    public string mText {
        get { return _Text; }
        set { _Text = value; regenerate(); }
    }
    [SerializeField, TextArea(5, 10)] private string _Text = "";

    private void OnValidate() {
        mWriting = this.findChild<MyBehaviour>("writing");
        if (mWriting != null) {
            mWriting.name = "deletedWriting";
            mWriting.deleteOnEditMode();
        }
        mWriting = null;
        regenerate();
    }
    private void Awake() {
        if (mWriting != null) {
            mWriting.name = "deletedWriting";
            mWriting.deleteOnEditMode();
        }
        mWriting = null;
        regenerate();
    }
    /// <summary>再生成</summary>
    public void regenerate() {
        mCurrentFontHeight = _DefaultFontHeight;
        mCurrentFontColor = mDefaultFontColor;
        mCurrentUnderLine = null;
        mCurrentAnimate = null;
        mCurrentColliderArgument = "";

        mWriting?.delete();
        mWriting = this.createChild<MyBehaviour>("writing");
        mLines.Clear();
        createNewLine();

        string tText = _Text;
        _Text = "";
        addLast(tText);
    }
    /// <summary>新しい行作成</summary>
    private Line createNewLine() {
        Line tLine = mWriting.createChild<Line>("Line" + mLines.Count);
        tLine.position = new Vector3(0, 0, 0);
        mLines.Add(tLine);
        return tLine;
    }
    /// <summary>文字mesh生成</summary>
    private TextMeshPro createTextMesh() {
        TextMeshPro tP = MyBehaviour.create<TextMeshPro>();
        tP.rectTransform.sizeDelta = new Vector2(1, 1);
        tP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        tP.font = _DefaultFontAsset;
        tP.fontSize = kUnitFontSize * mCurrentFontHeight;
        tP.color = mCurrentFontColor;
        switch (_CharacterAlign) {
            case CharacterAlign.middle:
                tP.alignment = TextAlignmentOptions.Center;
                break;
            case CharacterAlign.midLine:
                tP.alignment = TextAlignmentOptions.Midline;
                break;
            case CharacterAlign.capLine:
                tP.alignment = TextAlignmentOptions.Capline;
                break;
        }
        return tP;
    }
    /// <summary>アンダーライン生成</summary>
    private void createUnderline(float aThickness) {

    }
    /// <summary>末尾に文字追加</summary>
    public void addLast(string aText) {
        TagReader tReader = new TagReader(aText);
        while (!tReader.isEnd()) {
            TagReader.Element tElement = tReader.read();
            //1文字
            if (tElement is TagReader.OneChar) {
                StringElement tStringElement = StringElement.create(((TagReader.OneChar)tElement).mChar, this);
                addLast(tStringElement);
                continue;
            }
            //開始タグ
            if (tElement is TagReader.StartTag) {
                applyStartTag(((TagReader.StartTag)tElement));
                continue;
            }
            //終了タグ
            if (tElement is TagReader.EndTag) {
                applyEndTag(((TagReader.EndTag)tElement));
                continue;
            }
            Debug.LogWarning("MyTextBoard : 文字読み込み失敗　次の文字「" + tReader.mNext.ToString() + "」");
        }
        _Text += aText;
    }
    /// <summary>末尾に要素を追加</summary>
    private void addLast(CharElement aElement) {
        Line tLastLine = mLines[mLines.Count - 1];
        Line tAddedLine, tSemiLastLine;
        //幅を考慮し追加可能か確認
        if (mLineWidth < tLastLine.mCurrentWidth + aElement.mWidth) {
            //追加不可
            Line tNewLine = createNewLine();
            tNewLine.add(aElement);
            tAddedLine = tNewLine;
            tSemiLastLine = tLastLine;
        } else {
            //追加可
            tLastLine.add(aElement);
            tAddedLine = tLastLine;
            tSemiLastLine = mLines.Count == 1 ? null : mLines[mLines.Count - 2];
        }
        //行の位置を調整
        //横方向
        switch (mHorizontalAlign) {
            case HorizontalAlign.left:
                break;
            case HorizontalAlign.center:
                tAddedLine.positionX = -tAddedLine.mCurrentWidth / 2f;
                break;
            case HorizontalAlign.right:
                tAddedLine.positionX = -tAddedLine.mCurrentWidth;
                break;
        }
        //縦方向
        if (tSemiLastLine == null) {
            tAddedLine.positionY = -tAddedLine.mCurrentHeight;
        } else {
            tAddedLine.positionY = tSemiLastLine.positionY - mLineSpacing - tAddedLine.mCurrentHeight;
        }
        switch (mVerticalAlign) {
            case VerticalAlign.top:
                break;
            case VerticalAlign.middle:
                mWriting.positionY = -tAddedLine.positionY / 2f;
                break;
            case VerticalAlign.bottom:
                mWriting.positionY = -tAddedLine.positionY;
                break;
        }
    }
    /// <summary>開始タグ適用</summary>
    private void applyStartTag(TagReader.StartTag aTag) {
        switch (aTag.mTagName) {
            case "size":
                mCurrentFontHeight = float.Parse(aTag.mArguments[0]);
                break;
            case "color":
                mCurrentFontColor = makeColor(aTag.mArguments);
                break;
            case "br":
                createNewLine();
                break;
            case "u":
                if (aTag.mArguments.Length == 0) createUnderline(0.1f);
                else createUnderline(float.Parse(aTag.mArguments[0]));
                break;
            case "image":
                ImageElement tImageElement = ImageElement.create(aTag.mArguments[0], this);
                addLast(tImageElement);
                break;
            case "reading":
            case "collider":
                mCurrentColliderArgument = aTag.mArguments[0];
                break;
            case "highlight":
                //触れた時の引数
                mCurrentColliderArgument = aTag.mArguments[0];
                //色
                string[] tCp = new string[aTag.mArguments.Length - 1];
                Array.Copy(aTag.mArguments, tCp, aTag.mArguments.Length - 1);
                mCurrentFontColor = makeColor(tCp);
                break;
            case "link":
                //触れた時の引数
                mCurrentColliderArgument = aTag.mArguments[0];
                //色
                string[] tCp2 = new string[aTag.mArguments.Length - 1];
                Array.Copy(aTag.mArguments, tCp2, aTag.mArguments.Length - 1);
                mCurrentFontColor = makeColor(tCp2);
                //アンダーライン
                if (aTag.mArguments.Length < 6) createUnderline(0.1f);
                else createUnderline(float.Parse(aTag.mArguments[0]));
                break;
            case "animation":
                mCurrentAnimate = aTag;
                break;
        }
    }
    /// <summary>終了タグ適用</summary>
    private void applyEndTag(TagReader.EndTag aTag) {
        switch (aTag.mTagName) {
            case "size":
                mCurrentFontHeight = mDefaultFontHeight;
                break;
            case "color":
                mCurrentFontColor = mDefaultFontColor;
                break;
            case "u":
                mCurrentUnderLine = null;
                break;
            case "collider":
                mCurrentColliderArgument = "";
                break;
            case "highlight":
                mCurrentFontColor = mDefaultFontColor;
                mCurrentColliderArgument = "";
                break;
            case "link":
                mCurrentFontColor = mDefaultFontColor;
                mCurrentUnderLine = null;
                mCurrentColliderArgument = "";
                break;
            case "animation":
                mCurrentAnimate = null;
                break;
        }
    }
    /// <summary>色を生成</summary>
    private Color makeColor(string[] aArguments) {
        if (aArguments.Length == 0) {
            return mDefaultFontColor;
        } else if (aArguments.Length == 1) {
            switch (aArguments[0]) {
                case "red": return new Color(1, 0, 0, 1);
                case "green": return new Color(0, 1, 0, 1);
                case "blue": return new Color(0, 0, 1, 1);
                case "black": return new Color(0, 0, 0, 1);
                case "white": return new Color(1, 1, 1, 1);
                case "yellow": return new Color(0, 1, 1, 1);
                case "orange": return new Color(1, 1, 0, 1);
                case "purple": return new Color(1, 0, 1, 1);
            }
        } else if (aArguments.Length == 3) {
            return new Color(float.Parse(aArguments[0]), float.Parse(aArguments[1]), float.Parse(aArguments[2]), 1);
        } else if (aArguments.Length == 4) {
            return new Color(float.Parse(aArguments[0]), float.Parse(aArguments[1]), float.Parse(aArguments[2]), float.Parse(aArguments[3]));
        }
        //生成失敗
        string tArrayString = "[";
        foreach (string tS in aArguments) {
            tArrayString += tS + ",";
        }
        tArrayString.Remove(tArrayString.Length - 1);
        tArrayString += "]";
        Debug.LogWarning("MeshTextBoard : 色生成失敗" + tArrayString);
        return new Color(0, 0, 0, 1);
    }


    //行に入れる要素
    private abstract class CharElement : MyBehaviour {
        public float mWidth { get; protected set; }
        public float mHeight { get; protected set; }
    }
    private class StringElement : CharElement {
        private TextMeshPro mPro;
        static public StringElement create(string aString, MeshTextBoard aParent) {
            StringElement aElement = MyBehaviour.create<StringElement>();
            aElement.name = aString;
            aElement.mPro = aParent.createTextMesh();
            aElement.mPro.text = aString;
            aElement.mWidth = aElement.mPro.preferredWidth;
            aElement.mHeight = aParent.mCurrentFontHeight;
            aElement.mPro.transform.SetParent(aElement.transform, false);
            return aElement;
        }
    }
    private class ImageElement : CharElement {
        private SpriteRenderer mRenderer;
        static public ImageElement create(string aPath, MeshTextBoard aParent) {
            ImageElement aElement = MyBehaviour.create<ImageElement>();
            aElement.name = "image:" + aPath;
            aElement.mRenderer = aElement.createChild<SpriteRenderer>();
            aElement.mRenderer.sprite = Resources.Load<Sprite>(aPath);
            aElement.mHeight = aParent.mCurrentFontHeight;
            aElement.mWidth = aElement.mHeight * (aElement.mRenderer.sprite.bounds.size.x / aElement.mRenderer.sprite.bounds.size.y);
            float tScale = aElement.mHeight / aElement.mRenderer.sprite.bounds.size.y;
            aElement.mRenderer.transform.localScale = new Vector3(tScale, tScale, 1);
            return aElement;
        }
    }

    //行(pivotは行の左下)
    private class Line : MyBehaviour {
        /// <summary>要素</summary>
        private List<CharElement> mElements;
        /// <summary>現在の幅</summary>
        public float mCurrentWidth { get; private set; }
        /// <summary>現在の高さ</summary>
        public float mCurrentHeight { get; private set; }
        public Line() {
            mElements = new List<CharElement>();
        }
        /// <summary>この行に追加</summary>
        public void add(CharElement aElement) {
            //追加
            aElement.transform.SetParent(this.transform);
            aElement.position = new Vector3(mCurrentWidth + aElement.mWidth / 2f, aElement.mHeight / 2f, 0);
            mElements.Add(aElement);
            //現在の幅更新
            mCurrentWidth += aElement.mWidth;
            //現在の高さ更新
            if (mCurrentHeight < aElement.mHeight) mCurrentHeight = aElement.mHeight;
        }
    }
}
