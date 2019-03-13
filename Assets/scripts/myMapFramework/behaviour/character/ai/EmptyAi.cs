using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapBehaviour {
    private class EmptyAi : Ai{
        public EmptyAi(MapCharacter aParent):base(aParent){
        }
    }
}
