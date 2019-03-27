using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStratum : MyBehaviour {
    private int mStratumNum;
    public int stratumNum{
        get { return mStratumNum; }
        set{
            mStratumNum = value;
            positionZ = -value;
        }
    }
    private MyBehaviour mTiles;
    private MyBehaviour mCharacters;
    private MyBehaviour mOrnaments;
    private MyBehaviour mTriggers;
    private MyBehaviour mWalls;
    private void Awake(){
        mTiles = MyBehaviour.create<MyBehaviour>();
        mTiles.name = "tiles";
        mTiles.transform.SetParent(transform, false);
        mCharacters = MyBehaviour.create<MyBehaviour>();
        mCharacters.name = "characters";
        mCharacters.transform.SetParent(transform, false);
        mOrnaments = MyBehaviour.create<MyBehaviour>();
        mOrnaments.name = "ornaments";
        mOrnaments.transform.SetParent(transform, false);
        mTriggers = MyBehaviour.create<MyBehaviour>();
        mTriggers.name = "triggers";
        mTriggers.transform.SetParent(transform, false);
        mWalls = MyBehaviour.create<MyBehaviour>();
        mWalls.name = "walls";
        mWalls.transform.SetParent(transform, false);
    }
    public void addTile(MapTile aTile){
        aTile.transform.SetParent(mTiles.transform,false);
    }
    public void addCharacter(MapCharacter aChara){
        aChara.transform.SetParent(mCharacters.transform,false);
    }
    public void addOrnament(MapOrnament aOrnament){
        aOrnament.transform.SetParent(mOrnaments.transform,false);
    }
    public void addTrigger(MapTrigger aTrigger){
        aTrigger.transform.SetParent(mTriggers.transform, false);
    }
    public void add(MapBehaviour aBehaviour){
        if (aBehaviour is MapCharacter){
            addCharacter((MapCharacter)aBehaviour);
            return;
        }
        if(aBehaviour is MapTrigger){
            addTrigger((MapTrigger)aBehaviour);
            return;
        }
        MapEntity[] tEntity = aBehaviour.GetComponents<MapEntity>();
        if (tEntity[0] is MapCharacter){
            addCharacter((MapCharacter)tEntity[0]);
            return;
        }
        Debug.Log("MapStratum : " + aBehaviour.GetType().ToString() + "型の階層を変更するのは想定外");
    }
    //指定した階層と衝突判定するか
    public bool canCollide(MapStratum aStatum,MapStratum.ContactFilter aFilter=null){
        if (aStatum == null) return false;
        ContactFilter tFilter = (aFilter != null) ? aFilter : new ContactFilter(0,1);
        int tDifference = aStatum.mStratumNum - mStratumNum;
        return (tFilter.mMinStratumOffset <= tDifference) && (tDifference <= tFilter.mMaxStratumOffset);
    }
    //フィールドの周囲に壁を生成
    public void createWall(Vector2 aSize){
        MyBehaviour tWall;
        BoxCollider2D tCollider;
        //上
        tWall = MyBehaviour.create<MyBehaviour>();
        tCollider = tWall.gameObject.AddComponent<BoxCollider2D>();
        tCollider.size = new Vector2(aSize.x + 2, 1);
        tWall.position2D = new Vector2(aSize.x / 2 - 0.5f, aSize.y);
        tWall.transform.SetParent(mWalls.transform, false);
        tWall.gameObject.AddComponent<MapAttribute>().mAttribute = MapAttribute.Attribute.none;
        tWall.name = "wallUp";
        //下
        tWall = MyBehaviour.create<MyBehaviour>();
        tCollider = tWall.gameObject.AddComponent<BoxCollider2D>();
        tCollider.size = new Vector2(aSize.x + 2, 1);
        tWall.position2D = new Vector2(aSize.x / 2 - 0.5f, -1);
        tWall.transform.SetParent(mWalls.transform, false);
        tWall.gameObject.AddComponent<MapAttribute>().mAttribute = MapAttribute.Attribute.none;
        tWall.name = "wallDown";
        //左
        tWall = MyBehaviour.create<MyBehaviour>();
        tCollider = tWall.gameObject.AddComponent<BoxCollider2D>();
        tCollider.size = new Vector2(1, aSize.y + 2);
        tWall.position2D = new Vector2(-1, aSize.y / 2 - 0.5f);
        tWall.transform.SetParent(mWalls.transform, false);
        tWall.gameObject.AddComponent<MapAttribute>().mAttribute = MapAttribute.Attribute.none;
        tWall.name = "wallLeft";
        //右
        tWall = MyBehaviour.create<MyBehaviour>();
        tCollider = tWall.gameObject.AddComponent<BoxCollider2D>();
        tCollider.size = new Vector2(1, aSize.y + 2);
        tWall.position2D = new Vector2(aSize.x, aSize.y / 2 - 0.5f);
        tWall.transform.SetParent(mWalls.transform, false);
        tWall.gameObject.AddComponent<MapAttribute>().mAttribute = MapAttribute.Attribute.none;
        tWall.name = "wallRight";
    }
    public class ContactFilter{
        public int mMinStratumOffset = 0;
        public int mMaxStratumOffset = 0;
        public ContactFilter(){}
        public ContactFilter(int aMin,int aMax){
            mMinStratumOffset = aMin;
            mMaxStratumOffset = aMax;
        }
    }
}