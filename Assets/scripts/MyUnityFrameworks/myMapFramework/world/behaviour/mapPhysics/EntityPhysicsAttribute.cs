using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPhysicsAttribute : MapPhysicsAttribute {
    //<summary>物体属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        ghost,
        ornament,
        walking,
        flying,
        pygmy
    }

    //この属性が付与されているentity
    private MapEntity _Entity;
    //<summary>この属性が付与されているentity</summary>
    public MapEntity mEntity {
        get {
            if (_Entity == null)
                _Entity = GetComponent<MapEntity>();
            return _Entity;
        }
    }
    //<summary>この属性が付与されているbehaviourがいる階層</summary>
    public override MapStratumLevel getStratumLevel() {
        return mEntity.mStratumLevel;
    }
    //<summary>このbehaviourが引数のtileと衝突するか</summary>
    public MapPhysics.CollisionType canCollide(TilePhysicsAttribute aTilePhysics) {
        switch (mAttribute) {
            case Attribute.ghost:
                return MapPhysics.CollisionType.pass;
            case Attribute.ornament:
            case Attribute.walking:
                switch (aTilePhysics.mAttribute) {
                    case TilePhysicsAttribute.Attribute.none:
                    case TilePhysicsAttribute.Attribute.end:
                    case TilePhysicsAttribute.Attribute.air:
                    case TilePhysicsAttribute.Attribute.water:
                    case TilePhysicsAttribute.Attribute.magma:
                    case TilePhysicsAttribute.Attribute.wall:
                        return MapPhysics.CollisionType.collide;
                    case TilePhysicsAttribute.Attribute.flat:
                        return MapPhysics.CollisionType.pass;
                }
                break;
            case Attribute.flying:
                switch (aTilePhysics.mAttribute) {
                    case TilePhysicsAttribute.Attribute.none:
                    case TilePhysicsAttribute.Attribute.end:
                    case TilePhysicsAttribute.Attribute.wall:
                        return MapPhysics.CollisionType.collide;
                    case TilePhysicsAttribute.Attribute.flat:
                    case TilePhysicsAttribute.Attribute.water:
                    case TilePhysicsAttribute.Attribute.magma:
                    case TilePhysicsAttribute.Attribute.air:
                        return MapPhysics.CollisionType.pass;
                }
                break;
            case Attribute.pygmy:
                switch (aTilePhysics.mAttribute) {
                    case TilePhysicsAttribute.Attribute.none:
                    case TilePhysicsAttribute.Attribute.end:
                    case TilePhysicsAttribute.Attribute.wall:
                        return MapPhysics.CollisionType.collide;
                    case TilePhysicsAttribute.Attribute.flat:
                    case TilePhysicsAttribute.Attribute.water:
                    case TilePhysicsAttribute.Attribute.magma:
                    case TilePhysicsAttribute.Attribute.air:
                        return MapPhysics.CollisionType.pass;
                }
                break;
        }
        Debug.LogWarning("EntityPhysicsAttribute : 定義されていない属性の当たり判定「" + mAttribute.ToString() + "」「" + aTilePhysics.mAttribute.ToString() + "」");
        return MapPhysics.CollisionType.collide;
    }
    //<summary>このbehaviourが引数のentityと衝突するか</summary>
    public MapPhysics.CollisionType canCollide(EntityPhysicsAttribute aEntityPhysics) {
        switch (mAttribute) {
            case Attribute.ghost:
                return MapPhysics.CollisionType.pass;
            case Attribute.ornament:
            case Attribute.walking:
            case Attribute.flying:
                switch (aEntityPhysics.mAttribute) {
                    case Attribute.ghost:
                        return MapPhysics.CollisionType.pass;
                    case Attribute.ornament:
                    case Attribute.walking:
                    case Attribute.flying:
                        return MapPhysics.CollisionType.collide;
                    case Attribute.pygmy:
                        return MapPhysics.CollisionType.pass;
                }
                break;
            case Attribute.pygmy:
                switch (aEntityPhysics.mAttribute) {
                    case Attribute.ghost:
                        return MapPhysics.CollisionType.pass;
                    case Attribute.ornament:
                    case Attribute.walking:
                    case Attribute.flying:
                    case Attribute.pygmy:
                        return MapPhysics.CollisionType.pass;
                }
                break;
        }
        Debug.LogWarning("EntityPhysicsAttribute : 定義されていない属性の当たり判定「" + mAttribute.ToString() + "」「" + aEntityPhysics.mAttribute.ToString() + "」");
        return MapPhysics.CollisionType.collide;
    }
}
