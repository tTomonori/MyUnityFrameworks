using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharaterImageGroup : EntityImageGroup<MapCharacterImageH> {
    //<summary>移動方向によって画像を変更</summary>
     public void moved(Vector2 aVector) {
        fiddleImage((obj) => {
            obj.moved(aVector);
        });
    }
    //<summary>向きを設定</summary>
     public void setDirection(Vector2 aVector) {
        fiddleImage((obj) => {
            obj.setDirection(aVector);
        });
    }
    //<summary>現在の向きを取得</summary>
    public Vector2 getDirection() {
        return mImages[0].getDirection();
    }

    public override void make(EntityImageData aImageData) {
        base.make(aImageData);
        fiddleImage((obj) => {
            obj.processSprite();
            obj.setDirection(new Vector2(1, 0));
        });
    }
}
