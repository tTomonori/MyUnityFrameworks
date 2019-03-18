using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMap : MyBehaviour {
    public MapEventHandler mEventHandler;
    public MyMapController mController;
    private MapDataFile mFile;
    private MapPlayerCharacter mPlayer;
    private List<MapStratum> mStratums = new List<MapStratum>();
    private void Awake(){
        mController = new MyMapController(this);
    }
    public void loadMap(string aMapName){
        mFile = new MapDataFile(aMapName);
        //createFeild();
        //createOrnament();
        List<Arg> tStratumsData = mFile.stratum;
        for (int i = 0; i < tStratumsData.Count;i++){
            MapStratum tStratum = createStratum(tStratumsData[i]);
            tStratum.name = "stratum : " + i.ToString();
            tStratum.stratumNum = i;
            tStratum.transform.SetParent(transform, false);
            mStratums.Add(tStratum);
        }
    }
    public void load(Arg aSaveData){
        loadMap(aSaveData.get<string>("mapName"));
        createPlayer(aSaveData.get<Arg>("player"));
    }
    private MapStratum createStratum(Arg aData){
        MapStratum tStratum = MyBehaviour.create<MapStratum>();
        //feild
        createFeild(aData.get<List<List<int>>>("feild"), tStratum);
        //ornament
        createOrnaments(aData.get<List<Arg>>("ornament"), tStratum);
        //npc
        //event

        return tStratum;
    }
    //<summary>地面生成</summary>
    private void createFeild(List<List<int>> aData, MapStratum aStratum){
        Arg tChipData = mFile.chip;
        int tYCount = aData.Count;
        int tXCount = aData[0].Count;
        for (int y = 0; y < tYCount;y++){
            for (int x = 0; x < tXCount;x++){
                Arg tChip = tChipData.get<Arg>(aData[tYCount-1-y][x].ToString());
                MapTile tTile = createTile(tChip);
                if (tTile == null) continue;
                tTile.name = "(" + x.ToString() + "," + y.ToString() + ")";
                tTile.setPosition(x, y);
                aStratum.addTile(tTile);
            }
        }
    }
    //<summary>マップタイル生成</summary>
    private MapTile createTile(Arg aData){
        if (aData.dictionary == null) return null;
        MapTile tTile = MapBehaviour.createFromMapResource<MapTile>("tile/prefabs/" + aData.get<string>("prefab"));
        if (!aData.ContainsKey("pile")) return tTile;
        //pile
        List<Arg> tPiledDataList = aData.get<List<Arg>>("pile");
        for (int i = 0; i < tPiledDataList.Count;i++){
            Arg tData = tPiledDataList[i];
            MapTile tPiled = createTile(tData);
            tPiled.transform.SetParent(tTile.transform, false);
            tPiled.positionZ = -(i + 1) * 0.01f;
            tPiled.name = "piled"+i.ToString();
        }
        return tTile;
    }
    //<summary>リストの置物生成</summary>
    private void createOrnaments(List<Arg> aData, MapStratum aStratum){
        foreach(Arg tData in aData){
            MapOrnament tOrnament = createOrnament(tData);
            aStratum.addOrnament(tOrnament);
        }
    }
    //<summary>置物生成</summary>
    private MapOrnament createOrnament(Arg aData){
        MapOrnament tOrnament = MapBehaviour.createFromMapResource<MapOrnament>("ornament/prefabs/" + aData.get<string>("prefab"));
        tOrnament.name = aData.get<string>("prefab");
        tOrnament.setPosition(aData.get<float>("x"), aData.get<float>("y"));
        return tOrnament;
    }
    //<summary>プレイヤー生成</summary>
    private void createPlayer(Arg aData){
        MapCharacter tPlayer = createCharacter(Resources.Load<Sprite>("mymap/character/sprites/player"));

        mPlayer = tPlayer.gameObject.AddComponent<MapPlayerCharacter>();
        tPlayer.name = "player";
        tPlayer.setPosition(aData.get<float>("positionX"), aData.get<float>("positionY"));
        tPlayer.setAi("player");
        mStratums[aData.get<int>("stratum")].addCharacter(tPlayer);
    }
    //<summary>キャラクター生成</summary>
    private MapCharacter createCharacter(Sprite aSprite){
        MapCharacter tCharacter = MapBehaviour.createFromMapResource<MapCharacter>("character/prefabs/player");
        Sprite[][] tSprites = SpriteCutter.split(aSprite.texture, new Vector2(100, 100),new Vector2(0.5f,0));
        tCharacter.setSprites(tSprites);
        return tCharacter;
    }

    //stratum変更
    public void changeStratum(MapBehaviour aBehaviour,int aStratumNum){
        if (aStratumNum < 0) return;
        mStratums[aStratumNum].add(aBehaviour);
    }
}
