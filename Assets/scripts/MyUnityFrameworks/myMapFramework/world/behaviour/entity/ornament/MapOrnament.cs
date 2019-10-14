using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOrnament : MapEntity {
    [SerializeField] private MapEntityImage _Image;
    public override MapEntityImage mImage {
        get { return _Image; }
        set { _Image = value; }
    }
}
