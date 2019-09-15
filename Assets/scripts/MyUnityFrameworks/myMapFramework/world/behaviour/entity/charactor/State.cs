using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public abstract class State {
        protected MapCharacter parent;
        public State(MapCharacter aParent) {
            parent = aParent;
        }
        public virtual void update() { }
        public virtual void enter() { }
        public virtual void exit() { }
    }
}