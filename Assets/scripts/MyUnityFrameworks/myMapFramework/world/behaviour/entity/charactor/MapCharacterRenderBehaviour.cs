using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterRenderBehaviour : MapEntityRenderBehaviour {
    public MapCharacterImage mImage {
        get { return mBody as MapCharacterImage; }
        set { mBody = value; }
    }
}
