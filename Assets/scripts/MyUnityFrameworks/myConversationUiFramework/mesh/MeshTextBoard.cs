using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
/// <link,引数,1,0,0,1,0.1,1,0,0,1>collider + color + u
/// </link>/collider + /color + /u
/// 
/// <animation,...>アニメーション
///     <animation,circle>縁を描く
///     <animation,impact,2,0.5,2>拡縮移動でフェードイン<,,サイズ倍率,時間,開始時距離>
///     <animation,tremble>揺れる
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

    private void Awake() {
        applyText();
    }
    /// <summary>テキストを適用</summary>
    public void applyText() {
        mWriting = this.findChild<MyBehaviour>("writing");
        if (mWriting != null) {
            mWriting.name = "deletedWriting";
            mWriting.deleteOnEditMode();
        }
        mWriting = null;
        regenerate();
    }
    /// <summary>再生成</summary>
    public void regenerate() {
        string tText = _Text;
        reset();
        addLast(tText);
    }
    ///<summary>表示削除</summary>
    public void clear() {
        mWriting?.delete();
        mWriting = this.createChild<MyBehaviour>("writing");
        mLines.Clear();
        createNewLine();

        _Text = "";
    }
    ///<summary>表示初期化</summary>
    public void reset() {
        mCurrentFontHeight = _DefaultFontHeight;
        mCurrentFontColor = mDefaultFontColor;
        mCurrentUnderLine = null;
        mCurrentAnimate = null;
        mCurrentColliderArgument = "";
        clear();
    }
    /// <summary>新しい行作成</summary>
    private Line createNewLine() {
        Line tLine = mWriting.createChild<Line>("Line" + mLines.Count);
        tLine.position = new Vector3(0, 0, 0);
        mLines.Add(tLine);
        //アンダーライン生成
        if (mCurrentUnderLine != null) {
            createUnderline(mCurrentUnderLine.transform.localScale.y, mCurrentUnderLine.color);
        }
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
    private void createUnderline(float aThickness, Color aColor) {
        Line tLastLine = mLines[mLines.Count - 1];
        mCurrentUnderLine = tLastLine.createChild<SpriteRenderer>("underline");
        mCurrentUnderLine.sprite = Sprite.Create(new Texture2D(100, 100), new Rect(0, 0, 100, 100), new Vector2(0, 0.5f));
        mCurrentUnderLine.color = aColor;
        mCurrentUnderLine.transform.localScale = new Vector3(0, aThickness, 1);
        mCurrentUnderLine.transform.localPosition = new Vector3(tLastLine.mCurrentWidth, 0, 0);
    }
    /// <summary>読み仮名をふる</summary>
    private void writeReading(int aTargetStrLength, string aReading) {
        //読み仮名をふる文字を含む最も前の行を探索
        int tLength = aTargetStrLength;
        int tIndex = mLines.Count - 1;
        while (true) {
            if (tLength <= mLines[tIndex].mElements.Count) break;
            if (tIndex <= 0) break;
            tLength -= mLines[tIndex].mElements.Count;
            tIndex--;
        }
        //探索した行に含まれる読み仮名をふる文字の幅を求める
        tLength = tLength > mLines[tIndex].mElements.Count ? mLines[tIndex].mElements.Count : tLength;
        float tTargetStrWidth = 0;
        Line tTargetLine = mLines[tIndex];
        int tTargetLineElementNum = tTargetLine.mElements.Count;
        for (int i = 0; i < tLength; i++) {
            tTargetStrWidth += tTargetLine.mElements[tTargetLineElementNum - i - 1].mWidth;
        }
        //読み仮名生成
        TextMeshPro tReading = createTextMesh();
        tReading.transform.SetParent(tTargetLine.transform, false);
        tReading.name = "reading";
        tReading.rectTransform.sizeDelta = new Vector2(tTargetStrWidth, 1);
        tReading.fontSize = kUnitFontSize * mReadingFontSize;
        MyBehaviour tBehaviour = tReading.gameObject.AddComponent<MyBehaviour>();
        tBehaviour.position = new Vector3(tTargetLine.mCurrentWidth - tTargetStrWidth / 2f, tTargetLine.mCurrentHeight + mReadingFontSize / 2f, 0);
        tReading.text = aReading;
        //表示領域からはみ出る場合は「...」をつけて省略
        if (tReading.preferredWidth <= tTargetStrWidth) return;
        for (; ; ) {
            int tReadingLength = tReading.text.Length;
            if (tReadingLength == 0) break;
            tReading.text = tReading.text.Remove(tReadingLength - 1);
            tReading.text += "...";
            if (tReading.preferredWidth <= tTargetStrWidth) break;
            tReading.text = tReading.text.Remove(tReadingLength - 1);
        }
    }
    /// <summary>末尾に文字追加</summary>
    public void addLast(string aText) {
        TagReader tReader = new TagReader(aText);
        while (!tReader.isEnd()) {
            TagReader.Element tElement = tReader.read();
            addLast(tElement);
        }
        _Text += aText;
    }
    /// <summary>末尾に要素を追加</summary>
    private void addLast(CharElement aElement) {
        //アニメーション要素追加
        if (mCurrentAnimate != null)
            AnimateElement.addAnimateElement(aElement);

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
        //アンダーラインを伸ばす
        if (mCurrentUnderLine != null) {
            mCurrentUnderLine.transform.localScale += new Vector3(aElement.mWidth, 0, 0);
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
    /// <summary>末尾にread結果追加</summary>
    public void addLast(TagReader.Element aElement) {
        //1文字
        if (aElement is TagReader.OneChar) {
            //改行は無視
            if (((TagReader.OneChar)aElement).mChar == "\n") return;
            string tChar = ((TagReader.OneChar)aElement).mChar;
            StringElement tStringElement = StringElement.create(tChar, this);
            _Text+= tChar;
            addLast(tStringElement);
            return;
        }
        //開始タグ
        if (aElement is TagReader.StartTag) {
            _Text += ((TagReader.StartTag)aElement).mOriginalString;
            applyStartTag(((TagReader.StartTag)aElement));
            return;
        }
        //終了タグ
        if (aElement is TagReader.EndTag) {
            _Text += ((TagReader.EndTag)aElement).mOriginalString;
            applyEndTag(((TagReader.EndTag)aElement));
            return;
        }
        Debug.LogWarning("MyTextBoard : 文字読み込み失敗");
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
                if (aTag.mArguments.Length == 0) createUnderline(0.1f, mCurrentFontColor);
                else if (aTag.mArguments.Length == 1) {
                    createUnderline(float.Parse(aTag.mArguments[0]), mCurrentFontColor);
                } else {
                    string[] tCl = new string[aTag.mArguments.Length - 1];
                    Array.Copy(aTag.mArguments, 1, tCl, 0, aTag.mArguments.Length - 1);
                    createUnderline(float.Parse(aTag.mArguments[0]), makeColor(tCl));
                }
                break;
            case "image":
                ImageElement tImageElement = ImageElement.create(aTag.mArguments[0], this);
                addLast(tImageElement);
                break;
            case "reading":
                writeReading(int.Parse(aTag.mArguments[0]), aTag.mArguments[1]);
                break;
            case "collider":
                mCurrentColliderArgument = aTag.mArguments[0];
                break;
            case "highlight":
                //触れた時の引数
                mCurrentColliderArgument = aTag.mArguments[0];
                //色
                string[] tCp = new string[aTag.mArguments.Length - 1];
                Array.Copy(aTag.mArguments, 1, tCp, 0, aTag.mArguments.Length - 1);
                mCurrentFontColor = makeColor(tCp);
                break;
            case "link":
                //触れた時の引数
                mCurrentColliderArgument = aTag.mArguments[0];
                //色
                float tR;
                bool tIsNumber = float.TryParse(aTag.mArguments[1], out tR);
                int tColorArgLength = tIsNumber ? 4 : 1;
                string[] tCp2 = new string[tColorArgLength];
                Array.Copy(aTag.mArguments, 1, tCp2, 0, tColorArgLength);
                mCurrentFontColor = makeColor(tCp2);
                //アンダーライン
                if (aTag.mArguments.Length < tColorArgLength + 2) createUnderline(0.1f, mCurrentFontColor);
                else if (aTag.mArguments.Length == tColorArgLength + 2) {
                    createUnderline(float.Parse(aTag.mArguments[tColorArgLength + 1]), mCurrentFontColor);
                } else {
                    string[] tCl2 = new string[aTag.mArguments.Length - tColorArgLength - 2];
                    Array.Copy(aTag.mArguments, tColorArgLength + 2, tCl2, 0, aTag.mArguments.Length - tColorArgLength - 2);
                    createUnderline(float.Parse(aTag.mArguments[tColorArgLength + 1]), makeColor(tCl2));
                }
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
        public MeshTextBoard mParent;
        public String mArgument;
        //表示要素にComponentを追加する
        abstract public T addComponentToChildren<T>() where T : Component;
        private void OnMouseDown() {
            if (mParent.mTapCallback != null) {
                mParent.mTapCallback(mArgument);
            }
        }
    }
    private class StringElement : CharElement {
        private TextMeshPro mPro;
        static public StringElement create(string aString, MeshTextBoard aParent) {
            StringElement tElement = MyBehaviour.create<StringElement>();
            tElement.mParent = aParent;
            tElement.name = aString;
            tElement.mPro = aParent.createTextMesh();
            tElement.mPro.text = aString;
            tElement.mWidth = tElement.mPro.preferredWidth;
            tElement.mHeight = aParent.mCurrentFontHeight;
            tElement.mPro.transform.SetParent(tElement.transform, false);
            if (aParent.mCurrentColliderArgument != "") {
                tElement.mArgument = aParent.mCurrentColliderArgument;
                BoxCollider2D tCollider = tElement.gameObject.AddComponent<BoxCollider2D>();
                tCollider.size = new Vector2(tElement.mWidth, tElement.mHeight);
            }
            return tElement;
        }
        public override T addComponentToChildren<T>() {
            return mPro.gameObject.AddComponent<T>();
        }
    }
    private class ImageElement : CharElement {
        private SpriteRenderer mRenderer;
        static public ImageElement create(string aPath, MeshTextBoard aParent) {
            ImageElement tElement = MyBehaviour.create<ImageElement>();
            tElement.mParent = aParent;
            tElement.name = "image:" + aPath;
            tElement.mRenderer = tElement.createChild<SpriteRenderer>();
            tElement.mRenderer.sprite = Resources.Load<Sprite>(aPath);
            tElement.mHeight = aParent.mCurrentFontHeight;
            tElement.mWidth = tElement.mHeight * (tElement.mRenderer.sprite.bounds.size.x / tElement.mRenderer.sprite.bounds.size.y);
            float tScale = tElement.mHeight / tElement.mRenderer.sprite.bounds.size.y;
            tElement.mRenderer.transform.localScale = new Vector3(tScale, tScale, 1);
            if (aParent.mCurrentColliderArgument != "") {
                tElement.mArgument = aParent.mCurrentColliderArgument;
                BoxCollider2D tCollider = tElement.gameObject.AddComponent<BoxCollider2D>();
                tCollider.size = new Vector2(tElement.mWidth, tElement.mHeight);
            }
            return tElement;
        }
        public override T addComponentToChildren<T>() {
            return mRenderer.gameObject.AddComponent<T>();
        }
    }

    //行(pivotは行の左下)
    private class Line : MyBehaviour {
        /// <summary>要素</summary>
        public List<CharElement> mElements { get; private set; }
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
    private class AnimateElement : MyBehaviour {
        public static AnimateElement addAnimateElement(CharElement aElement) {
            switch (aElement.mParent.mCurrentAnimate.mArguments[0]) {
                case "circle":
                    CircleElement tRotate = aElement.gameObject.AddComponent<CircleElement>();
                    tRotate.init(aElement);
                    return tRotate;
                case "impact":
                    ImpactElement tImpact = aElement.gameObject.AddComponent<ImpactElement>();
                    tImpact.init(aElement);
                    return tImpact;
                case "tremble":
                    TrembleElement tTremble = aElement.gameObject.AddComponent<TrembleElement>();
                    tTremble.init(aElement);
                    return tTremble;
            }
            return null;
        }
    }
    private class CircleElement : AnimateElement {
        private float mCurrentRad = 270;
        [SerializeField] private MyBehaviour mAnimateBehaviour;
        [SerializeField] private float mCircleRadius;
        public void init(CharElement aElement) {
            mAnimateBehaviour = aElement.addComponentToChildren<MyBehaviour>();
            mCircleRadius = aElement.mHeight / 3f;
        }
        private void Update() {
            mCurrentRad = (mCurrentRad - 360 * Time.deltaTime + 360) % 360;
            Vector2 tVector = Quaternion.Euler(0, 0, mCurrentRad) * new Vector2(1, 0) * mCircleRadius + new Vector3(0, mCircleRadius, 0);
            mAnimateBehaviour.position2D = tVector;
        }
    }
    private class ImpactElement : AnimateElement {
        private float mElapsedTime = 0;
        private Vector2 mInitialPosition;
        private Vector2 mTargetScale;
        [SerializeField] private MyBehaviour mAnimateBehaviour;
        [SerializeField] private float mInitialScale;
        [SerializeField] private float mDuration;
        [SerializeField] private float mInitialDistance;
        public void init(CharElement aElement) {
            mAnimateBehaviour = aElement.addComponentToChildren<MyBehaviour>();
            mInitialScale = aElement.mParent.mCurrentAnimate.mArguments.Length < 2 ? 5 : float.Parse(aElement.mParent.mCurrentAnimate.mArguments[1]);
            mDuration = aElement.mParent.mCurrentAnimate.mArguments.Length < 3 ? 0.3f : float.Parse(aElement.mParent.mCurrentAnimate.mArguments[2]);
            mInitialDistance = aElement.mParent.mCurrentAnimate.mArguments.Length < 4 ? aElement.mHeight : float.Parse(aElement.mParent.mCurrentAnimate.mArguments[3]);
            mInitialPosition = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 359)) * new Vector2(1, 0) * mInitialDistance;
            mTargetScale = mAnimateBehaviour.scale2D;
        }
        private void Update() {
            if (mElapsedTime >= mDuration) return;
            mElapsedTime += Time.deltaTime;
            if (mElapsedTime >= mDuration) {
                mAnimateBehaviour.scale2D = mTargetScale;
                mAnimateBehaviour.position2D = new Vector2(0, 0);
                return;
            }
            float tScale = mInitialScale - (mInitialScale - 1) * mElapsedTime / mDuration;
            mAnimateBehaviour.scale2D = mTargetScale * tScale;
            mAnimateBehaviour.position2D = mInitialPosition - mInitialPosition * mElapsedTime / mDuration;
        }
    }
    private class TrembleElement : AnimateElement {
        private float mTrembleTime = 0.05f;
        [SerializeField] private MyBehaviour mAnimateBehaviour;
        [SerializeField] private float mMoveRangeRadius;
        private void Start() {
            Vector2 tVector = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 359)) * new Vector2(1, 0) * mMoveRangeRadius;
            mAnimateBehaviour.moveBy(tVector, mTrembleTime, tremble);
        }
        public void init(CharElement aElement) {
            mAnimateBehaviour = aElement.addComponentToChildren<MyBehaviour>();
            mMoveRangeRadius = aElement.mHeight / 8f;
        }
        private void tremble() {
            Vector2 tCurrentVector = mAnimateBehaviour.position.matchLength(mMoveRangeRadius);
            Vector2 tNextVector = Quaternion.Euler(0, 0, UnityEngine.Random.Range(100, 260)) * tCurrentVector;
            if (Time.deltaTime >= mTrembleTime)
                MyBehaviour.setTimeoutToIns(0, () => { mAnimateBehaviour?.moveBy(tNextVector - tCurrentVector, mTrembleTime, tremble); });
            else
                mAnimateBehaviour.moveBy(tNextVector - tCurrentVector, mTrembleTime, tremble);
        }
    }
}
[CustomEditor(typeof(MeshTextBoard))]
public class ExampleScriptEditor : Editor {
    /// <summary>InspectorのGUIを更新</summary>
    public override void OnInspectorGUI() {
        //元のInspector部分を表示
        base.OnInspectorGUI();
        //targetを変換して対象を取得
        MeshTextBoard tBoard = target as MeshTextBoard;
        //ボタンを表示
        if (GUILayout.Button("Apply")) {
            tBoard.applyText();
        }
    }

}