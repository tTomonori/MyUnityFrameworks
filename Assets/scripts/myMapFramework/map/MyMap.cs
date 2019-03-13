using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMap : MyBehaviour {
    public MapEventHandler mEventHandler;
    public MyMapController mController;
    private MapDataFile mFile;
    private MyBehaviour mTiles;
    private MyBehaviour mOrnaments;
    private MyBehaviour mCharacters;
    private void Awake(){
        mController = new MyMapController(this);
        //tiles
        mTiles = MyBehaviour.create<MyBehaviour>();
        mTiles.transform.parent = this.transform;
        mTiles.position = new Vector3();
        mTiles.name = "tiles";
        mTiles.positionZ = 900;
        //ornaments
        mOrnaments = MyBehaviour.create<MyBehaviour>();
        mOrnaments.transform.parent = this.transform;
        mOrnaments.position = new Vector3();
        mOrnaments.name = "ornaments";
        //characters
        mCharacters = MyBehaviour.create<MyBehaviour>();
        mCharacters.transform.parent = this.transform;
        mCharacters.position = new Vector3();
        mCharacters.name = "characters";
    }
    public void loadMap(string aMapName){
        mFile = new MapDataFile(aMapName);
        createFeild();
        createOrnament();
    }
    public void load(Arg aSaveData){
        loadMap(aSaveData.get<string>("mapName"));
        createPlayer(aSaveData.get<Arg>("player"));
    }
    //<summary>マップタイル生成</summary>
    private void createFeild(){
        List<List<int>> tFeildData = mFile.feild;
        Arg tChipData = mFile.chip;
        for (int y = 0; y < tFeildData.Count;y++){
            for (int x = 0; x < tFeildData[0].Count;x++){
                Arg tChip = tChipData.get<Arg>(tFeildData[y][x].ToString());
                MapTile tTile = MapBehaviour.createFromMapResource<MapTile>("tile/prefabs/" + tChip.get<string>("prefab"));
                tTile.transform.SetParent(mTiles.transform, false);
                tTile.name = "(" + x.ToString() + "," + y.ToString() + ")";
                tTile.setPositionMaintainZ(x, y);
            }
        }
    }
    //<summary>置物生成</summary>
    private void createOrnament(){
        List<Arg> tOrnamentData = mFile.objects;
        foreach(Arg tData in tOrnamentData){
            MapOrnament tOrnament = MapBehaviour.createFromMapResource<MapOrnament>("ornament/prefabs/" + tData.get<string>("prefab"));
            tOrnament.transform.parent = mOrnaments.transform;
            tOrnament.name = tData.get<string>("prefab");
            tOrnament.setPosition(tData.get<float>("x"), tData.get<float>("y"));
        }
    }
    //<summary>プレイヤー生成</summary>
    private void createPlayer(Arg aData){
        MapCharacter tPlayer = createCharacter(Resources.Load<Sprite>("mymap/character/sprites/player"));

        tPlayer.transform.parent = mCharacters.transform;
        tPlayer.name = "player";
        tPlayer.setPosition(aData.get<float>("positionX"), aData.get<float>("positionY"));
        tPlayer.setAi("player");
    }
    private MapCharacter createCharacter(Sprite aSprite){
        MapCharacter tCharacter = MapBehaviour.createFromMapResource<MapCharacter>("character/prefabs/player");
        Sprite[][] tSprites = SpriteCutter.split(aSprite.texture, new Vector2(100, 100),new Vector2(0.5f,0));
        tCharacter.setSprites(tSprites);
        return tCharacter;
    }
}
