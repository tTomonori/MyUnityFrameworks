using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public abstract partial class Ai {
        protected MapCharacter parent;
        public Ai(MapCharacter aParent) {
            parent = aParent;
        }
        public virtual void start() { }
        public virtual void update() { }
    }
}
