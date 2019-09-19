using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapCharacter : MapEntity {
    public abstract class State {
        public MapCharacter parent;
        public virtual void update() { }
        public virtual void enter() { }
        public virtual void exit() { }

        //<summary>移動入力(入力を受け付けたらtrue)</summary>
        public virtual bool move(Vector2 aVector, float aMaxMoveDistance = float.PositiveInfinity) {
            return false;
         }
    }
}