using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOrnament : MapEntity {
    public MapFileData.Ornament mFileData;
    [SerializeField] private MapEntityImage _Image;
    public override MapEntityImage mImage {
        get { return _Image; }
        set { _Image = value; }
    }

    /// <summary>ornamentを復元する時に必要となるデータ</summary>
    public virtual Arg save() {
        return null;
    }
    /// <summary>変数適用</summary>
    public virtual void setArg(Arg aData) {

    }
}
