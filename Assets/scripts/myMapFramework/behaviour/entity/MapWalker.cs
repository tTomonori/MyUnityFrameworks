using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapStepper))] 
public class MapWalker : MapBehaviour {
    //障害物と衝突した時、障害物との距離の最大許容距離
    static private float kMaxSeparation = 0.02f;
    [SerializeField] private MapEntity mEntity;
    private MapStepper mStepper;
    private float mMaxDelta;
    private void Awake(){
        //移動用パラメータ
        Vector2 tSize = mEntity.boxCollider.size;
        mMaxDelta = Mathf.Sqrt(tSize.x * tSize.x + tSize.y * tSize.y) / 2;
        if (mMaxDelta > 1) mMaxDelta = 1;
        //borderStepper
        mStepper = gameObject.GetComponent<MapStepper>();
    }
    //<summary>指定方向に指定速度になるように移動</summary>
    public PassType move(Vector2 aVector,float aSpeed){
        //移動距離
        Vector2 tDistance = aVector.normalized * aSpeed * Time.deltaTime;
        return move(tDistance);
    }
    /// <summary>
    /// 指定方向に指定速度になるように、指定最大距離を超えないように移動
    /// </summary>
    /// <returns>移動結果</returns>
    /// <param name="aVector">移動方向</param>
    /// <param name="aSpeed">移動速度</param>
    /// <param name="aMax">最大移動距離.</param>
    /// <param name="aStuckAtMax">指定最大距離に引っかかったら<c>true</c></param>
    public PassType move(Vector2 aVector,float aSpeed,Vector2 aMax,out bool oStuckAtMax){
        oStuckAtMax = false;
        //移動距離
        Vector2 tDistance = aVector.normalized * aSpeed * Time.deltaTime;
        if ((tDistance.x * aMax.x > 0) && Mathf.Abs(tDistance.x) > Mathf.Abs(aMax.x)){
            tDistance.x = aMax.x;
            oStuckAtMax = true;
        }
        if ((tDistance.y * aMax.y > 0) && Mathf.Abs(tDistance.y) > Mathf.Abs(aMax.y)){
            tDistance.y = aMax.y;
            oStuckAtMax = true;
        }
        PassType tPassType = move(tDistance);
        if (tPassType != PassType.through) oStuckAtMax = false;
        return tPassType;
    }
    //<summary>指定距離移動</summary>
    public PassType move(Vector2 aVector){
        Vector2 tNormal = aVector.normalized;
        //移動距離
        Vector2 tDistance = aVector;
        //delta移動距離
        Vector2 tDelta = tNormal * mMaxDelta;
        while(true){
            if(tDistance.magnitude<mMaxDelta){
                //最後の移動
                if(tDistance.magnitude!=0)
                    return moveDelta(tDistance);
                return PassType.through;
            }
            PassType tPassType = moveDelta(tDelta);
            tDistance -= tDelta;
            switch(tPassType){
                case PassType.stop:
                    //これ以上進めない
                    return PassType.stop;
                case PassType.collision:
                    //これ以上進めない
                    return PassType.collision;
                case PassType.through:
                case PassType.slide:
                    //まだ進める
                    break;
            }
        }
    }
    //<summary>ちょびちょび移動の一回分</summary>
    private PassType moveDelta(Vector2 aDelta){
        //衝突したcollider
        Collider2D tCollided;
        PassType tInterimPassType = canMove(position2D + aDelta,out tCollided);
        if (tInterimPassType == PassType.through){
            moveTo(position2D + aDelta);
            return PassType.through;
        }
        //最後に検証した距離と、次に検証する距離の差
        Vector2 tDD = aDelta;
        //暫定移動先
        Vector2? tInterimTo = null;
        if (tInterimPassType == PassType.stop) tInterimTo = position2D + aDelta;
        //次に検証する移動距離
        Vector2 tDelta = aDelta;

        while(tDD.magnitude>MapWalker.kMaxSeparation){
            Collider2D tCollidedOut;
            switch(canMove(position2D+tDelta,out tCollidedOut)){
                case PassType.through:
                    if (tInterimPassType == PassType.collision) tInterimPassType = PassType.through;
                    tInterimTo = position2D + tDelta;
                    tDelta += tDD/2;
                    break;
                case PassType.stop:
                    tInterimPassType = PassType.stop;
                    tInterimTo = position2D + tDelta;
                    tDelta -= tDD / 2;
                    break;
                case PassType.collision:
                    tCollided = tCollidedOut;
                    tDelta -= tDD / 2;
                    break;
            }
            tDD = tDD / 2;
        }
        //移動距離
        Vector2 tMoveDelta = (tInterimTo==null)?new Vector2():(Vector2)tInterimTo - position2D;
        //移動
        if(tInterimTo!=null)
            moveTo((Vector2)tInterimTo);
        //壁に沿って移動
        if(tInterimPassType == PassType.collision){
            ColliderDistance2D tCollision = tCollided.Distance(mEntity.boxCollider);
            Vector2 tRemaining = DirectionOperator.disassemble(aDelta - tMoveDelta, tCollision.normal);
            if (tRemaining.magnitude == aDelta.magnitude) return PassType.stop;//垂直方向を取ったのにベクトルの大きさが変わらなかった
            switch(moveDelta(tRemaining)){
                case PassType.stop:return PassType.stop;
                case PassType.collision:return PassType.collision;
                case PassType.through:return PassType.slide;
                case PassType.slide:return PassType.slide;
            }
        }
        return tInterimPassType;
    }
    //<summary>指定した位置へ移動</summary>
    private void moveTo(Vector2 aPosition){
        setPosition(aPosition.x, aPosition.y);
        mStepper.step();
    }
    //指定した座標に移動可能か
    public PassType canMove(Vector2 aPosition,out Collider2D oCollided){
        Vector2 tSize = mEntity.boxCollider.size;
        Collider2D[] tColliders = Physics2D.OverlapBoxAll(aPosition + new Vector2(0, tSize.y / 2), tSize, 0);
        PassType tInterimPassType = PassType.through;
        //衝突する可能性があるcolliderのみ抽出してforeach
        foreach(Collider2D tCollider in selectCanCollide(tColliders)){
            MapAttribute tAttribute = tCollider.gameObject.GetComponent<MapAttribute>();
            //属性なし
            if (tAttribute == null) continue;
            //イベントトリガー
            if(tAttribute.type == MapAttribute.Type.trigger){
                MapTrigger tTrigger = tCollider.GetComponent<MapTrigger>();
                PassType tPassType = tTrigger.confirmPassType(mEntity);
                if (tPassType == PassType.collision){
                    oCollided = tCollider;
                    return PassType.collision;
                }
                if (tPassType == PassType.stop) tInterimPassType = PassType.stop;
                continue;
            }
            //地形or置物orキャラ
            if (tAttribute.isCollide(mEntity.attribute)){
                oCollided = tCollider;
                return PassType.collision;
            }
        }
        oCollided = null;
        return tInterimPassType;
    }
    public enum PassType{
        through,stop,collision,slide
    }
}
