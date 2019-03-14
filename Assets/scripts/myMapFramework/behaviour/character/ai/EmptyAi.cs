using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    private class EmptyAi : Ai{
        public EmptyAi(MapCharacter aParent):base(aParent){
        }
    }
}
