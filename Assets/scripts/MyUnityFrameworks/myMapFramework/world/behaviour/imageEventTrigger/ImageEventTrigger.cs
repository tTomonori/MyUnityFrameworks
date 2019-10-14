using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEventTrigger : MapBehaviour {
    /// <summary>
    /// このトリガーの画像イベントを引数のイベントデータに追加する
    /// </summary>
    /// <param name="aData">画像イベントデータ</param>
    /// <param name="aBehaviour">画像イベントを適用するbehaviour</param>
    public virtual void plusEvent(ImageEventData aData, MapEntity aBehaviour) { }
}
