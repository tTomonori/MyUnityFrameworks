using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionStratumTilePhysicsAttribute : TilePhysicsAttribute {
    //<summary>当たり判定をとる階層</summary>
    [SerializeField] public CollidedStratum mCollidedStratum;
    //<summary>当たり判定をとる階層</summary>
    public enum CollidedStratum {
        //<summary>通常</summary>
        normal,
        //<summary>同じか一つ下の階層のみ</summary>
        lowOnly,
        //<summary>同じか一つ上の階層のみ</summary>
        highOnly,
        //<summary>同じ階層のみ</summary>
        equalOnly
    }

    //<summary>引数のentityがこのtileと衝突するか</summary>
    public bool canCollide(EntityPhysicsAttribute aEntity) {
        int tMyLevel = this.getStratumLevel().mLevel;
        int tOpponentLeve = aEntity.getStratumLevel().mLevel;
        switch (mCollidedStratum) {
            case CollidedStratum.normal:
                return true;
            case CollidedStratum.lowOnly:
                return (tOpponentLeve < tMyLevel);
            case CollidedStratum.highOnly:
                return (tMyLevel < tOpponentLeve);
            case CollidedStratum.equalOnly:
                return tMyLevel == tOpponentLeve;
            default:
                throw new System.Exception("ExceptionTilePhysicsAttribute : 未定義の階層当たり判定「" + mCollidedStratum + "」");
        }
    }
}
