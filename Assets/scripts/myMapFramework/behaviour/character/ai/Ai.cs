using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapEntity {
    public abstract class Ai{
        protected MapCharacter parent;
        public Ai(MapCharacter aParent){
            parent = aParent;
        }
        public virtual void update(){}
        static public Ai convertToInstance(string aAiName,MapCharacter aParent){
            switch(aAiName){
                case "player":return new PlayerAi(aParent);
                default :return new EmptyAi(aParent);
            }
        }
    }
}
