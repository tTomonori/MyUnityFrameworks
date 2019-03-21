using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MapWorld : MyBehaviour {
    public class MapEventOperator{
        private MapWorld parent;
        private List<MapEvent> mEvents = new List<MapEvent>();
        public MapEventOperator(MapWorld aParent){
            parent = aParent;
        }
        //実行するイベントを登録
        public void addEvent(MapEvent aEvent){
            mEvents.Add(aEvent);
        }
        //登録されているイベントを実行
        public void runEvents(){
            foreach(MapEvent aEvent in mEvents){
                aEvent.run(this);
            }
            mEvents.Clear();
        }
        //キャラクターを取得
        public MapCharacter getCharacter(string aName){
            return parent.getCharacter(aName);
        }
    }
}
