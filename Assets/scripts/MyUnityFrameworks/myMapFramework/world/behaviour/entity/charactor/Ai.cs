using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public abstract partial class Ai {
        public MapCharacter parent;
        public virtual void update() { }
        public abstract string save();
    }
}
