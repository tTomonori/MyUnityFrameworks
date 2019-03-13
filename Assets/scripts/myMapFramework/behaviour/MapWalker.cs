using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWalker : MapBehaviour {
    //障害物と衝突した時、障害物との距離の最大許容距離
    static private float kMaxSeparation = 0.02f;
    [SerializeField] private MapBehaviour mBehaviour;
    [SerializeField] private BoxCollider2D mCollider;
    [SerializeField] private MapAttribute mAttribute;
    private float mMaxDelta;
    private void Awake(){
        if (mCollider == null) mCollider = gameObject.GetComponent<BoxCollider2D>();
        Vector2 tSize = mCollider.size;
        mMaxDelta = Mathf.Sqrt(tSize.x * tSize.x + tSize.y * tSize.y) / 2;
        if (mMaxDelta > 1) mMaxDelta = 1;
    }
    public void move(Vector2 aVector,float aSpeed){
        Vector2 tNormal = aVector.normalized;
        //移動距離
        Vector2 tDistance = tNormal * aSpeed * Time.deltaTime;
        //delta移動距離
        Vector2 tDelta = tNormal * mMaxDelta;
        while(true){
            if(tDistance.magnitude<mMaxDelta){
                //最後の移動
                if(tDistance.magnitude!=0)
                    moveDelta(tDistance);
                return;
            }
            PassType tPassType = moveDelta(tDelta);
            tDistance -= tDelta;
            switch(tPassType){
                case PassType.stop:
                case PassType.collision:
                    //これ以上進めない
                    return;
                case PassType.through:
                case PassType.slide:
                    //まだ進める
                    break;
            }
        }
    }
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
            ColliderDistance2D tCollision = tCollided.Distance(mCollider);
            Vector2 tRemaining = DirectionOperator.disassemble(aDelta - tMoveDelta, tCollision.normal);
            switch(moveDelta(tRemaining)){
                case PassType.stop:return PassType.stop;
                case PassType.collision:return PassType.collision;
                case PassType.through:return PassType.slide;
                case PassType.slide:return PassType.slide;
            }
        }
        return tInterimPassType;
    }
    private void moveTo(Vector2 aPosition){
        setPosition(aPosition.x, aPosition.y);
    }
    //指定した座標に移動可能か
    public PassType canMove(Vector2 aPosition,out Collider2D oCollided){
        Vector2 tSize = mCollider.size;
        Collider2D[] tColliders = Physics2D.OverlapBoxAll(aPosition + new Vector2(0, tSize.y / 2), tSize, 0);
        PassType tInterimPassType = PassType.through;
        foreach(Collider2D tCollider in tColliders){
            if (tCollider == mCollider) continue;//自分自身とは当たり判定を取らない
            MapAttribute tAttribute = tCollider.gameObject.GetComponent<MapAttribute>();
            //属性なし
            if (tAttribute == null) continue;
            //イベントトリガー
            if(tAttribute.type == MapAttribute.Type.eventTrigger){
                MapEventTrigger tTrigger = tCollider.GetComponent<MapEventTrigger>();
                PassType tPassType = tTrigger.confirmPassType(mBehaviour);
                if (tPassType == PassType.collision){
                    oCollided = tCollider;
                    return PassType.collision;
                }
                if (tPassType == PassType.stop) tInterimPassType = PassType.stop;
                continue;
            }
            //地形or置物orキャラ
            if (tAttribute.isCollide(mAttribute)){
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
