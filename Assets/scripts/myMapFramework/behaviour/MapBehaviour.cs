using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    private Collider2D stoc;//colliderが変更されないことを前提に保持しておく
    //<summary>このオブジェクトについているcollider</summary>
    private Collider2D mCollider{
        get{
            if (stoc != null) return stoc;
            stoc = GetComponent<Collider2D>();
            return stoc;
        }
    }
    public MapStratum mStratum{
        get { return GetComponentInParent<MapStratum>(); }
    }
    public float mOffsetZ = 0;
    /// <summary>
    /// 指定したパスのプレハブを生成
    /// </summary>
    /// <returns>生成したプレハブがもつComponent</returns>
    /// <param name="aFilePath">プレハブへのパス("mymap/" + X)</param>
    /// <typeparam name="Type">取得するComponent</typeparam>
    public static Type createFromMapResource<Type>(string aFilePath){
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("mymap/" + aFilePath);
        // プレハブからインスタンスを生成
        return Instantiate(prefab).GetComponent<Type>();
    }
    //<summary>z座標を維持して座標を変更</summary>
    public void setPositionMaintainZ(float aX,float aY){
        position = new Vector3(aX, aY, positionZ);
    }
    //<summary>座標を変更</summary>
    public void setPosition(float aX,float aY){
        position = new Vector3(aX, aY, aY + mOffsetZ);
    }
    //<summary>当たり判定を無視して強制移動</summary>
    public void foceMoveBy(Vector2 aVector){
        position = new Vector3(positionX + aVector.x, positionY + aVector.y, positionY + aVector.y + mOffsetZ);
    }
    //階層を変更
    public void changeStratum(int aStratumNum){
        GetComponentInParent<MapWorld>().changeStratum(this, aStratumNum);
    }
    //<summary>(階層を考慮した上で)自分と衝突しているcolliderを返す</summary>
    public List<Collider2D> getCollided(){
        Collider2D[] tColliders = new Collider2D[12];
        mCollider.OverlapCollider(new ContactFilter2D(), tColliders);

        MapStratum tMyStratum = gameObject.GetComponentInParent<MapStratum>();
        List<Collider2D> tRes = new List<Collider2D>();
        foreach(Collider2D tCollider in tColliders){
            if (tCollider == null) break;
            MapStratum tStratum = tCollider.GetComponentInParent<MapStratum>();
            if (tStratum == null) continue;
            if (tMyStratum.canCollide(tStratum))
                tRes.Add(tCollider);
        }
        return tRes;
    }
    //<summary>(階層を考慮した上で)自分と衝突しているcolliderを返す</summary>
    public List<T> getCollided<T>() where T:Component{
        List<T> tRes = new List<T>();
        foreach(Collider2D tCollider in getCollided()){
            T t = tCollider.GetComponent<T>();
            if (t != null) tRes.Add(t);
        }
        return tRes;
    }
    //<summary>自分と衝突する可能性のあるcolliderのみを抽出する</summary>
    public List<Collider2D> selectCanCollide(Collider2D[] aColliders){
        List<Collider2D> tRes = new List<Collider2D>();
        MapStratum tMyStratum = mStratum;//自分の階層
        foreach(Collider2D tCollider in aColliders){
            if (tCollider == mCollider) continue;//自分自身は対象外
            if (!tMyStratum.canCollide(tCollider.GetComponentInParent<MapStratum>())) continue;//階層の違いにより衝突しない
            tRes.Add(tCollider);
        }
        return tRes;
    }
    //<summary>自分と衝突する可能性がある</summary>
    public bool canCollide(Collider2D aCollider){
        if (aCollider == mCollider) return false;//自分自身とは衝突しない
        if (!mStratum.canCollide(aCollider.GetComponentInParent<MapStratum>())) return false;//階層の違いにより衝突しない
        return true;
    }
}
