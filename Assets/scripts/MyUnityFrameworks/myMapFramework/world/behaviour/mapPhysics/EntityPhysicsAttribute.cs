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
    //<summary>このbehaviourが引数のbehaviourと衝突するか</summary>
    public override CollisionType canCollide(MapPhysicsAttribute aBehaviour) {
        if (aBehaviour is TilePhysicsAttribute)
            return canCollide((TilePhysicsAttribute)aBehaviour);
        if (aBehaviour is EntityPhysicsAttribute)
            return canCollide((EntityPhysicsAttribute)aBehaviour);
        if (aBehaviour is TriggerPhysicsAttribute)
            return ((TriggerPhysicsAttribute)aBehaviour).canCollide(this);

        throw new System.Exception("EntityPhysicsAttribute : 「" + aBehaviour.GetType().ToString() + "」型との衝突判定が定義されてない");
    }
    //<summary>このbehaviourが引数のtileと衝突するか</summary>
    public CollisionType canCollide(TilePhysicsAttribute aTilePhysics) {
        switch (mAttribute) {
            case Attribute.ghost:
                return CollisionType.pass;
            case Attribute.ornament:
            case Attribute.walking:
                switch (aTilePhysics.mAttribute) {
                    case TilePhysicsAttribute.Attribute.none:
                    case TilePhysicsAttribute.Attribute.end:
                    case TilePhysicsAttribute.Attribute.air:
                    case TilePhysicsAttribute.Attribute.water:
                    case TilePhysicsAttribute.Attribute.magma:
                    case TilePhysicsAttribute.Attribute.wall:
                        return CollisionType.collide;
                    case TilePhysicsAttribute.Attribute.flat:
                        return CollisionType.pass;
                }
                break;
            case Attribute.flying:
                switch (aTilePhysics.mAttribute) {
                    case TilePhysicsAttribute.Attribute.none:
                    case TilePhysicsAttribute.Attribute.end:
                    case TilePhysicsAttribute.Attribute.wall:
                        return CollisionType.collide;
                    case TilePhysicsAttribute.Attribute.flat:
                    case TilePhysicsAttribute.Attribute.water:
                    case TilePhysicsAttribute.Attribute.magma:
                    case TilePhysicsAttribute.Attribute.air:
                        return CollisionType.pass;
                }
                break;
            case Attribute.pygmy:
                switch (aTilePhysics.mAttribute) {
                    case TilePhysicsAttribute.Attribute.none:
                    case TilePhysicsAttribute.Attribute.end:
                    case TilePhysicsAttribute.Attribute.wall:
                        return CollisionType.collide;
                    case TilePhysicsAttribute.Attribute.flat:
                    case TilePhysicsAttribute.Attribute.water:
                    case TilePhysicsAttribute.Attribute.magma:
                    case TilePhysicsAttribute.Attribute.air:
                        return CollisionType.pass;
                }
                break;
        }
        Debug.LogWarning("EntityPhysicsAttribute : 定義されていない属性の当たり判定「" + mAttribute.ToString() + "」「" + aTilePhysics.mAttribute.ToString() + "」");
        return CollisionType.collide;
    }
    //<summary>このbehaviourが引数のentityと衝突するか</summary>
    public CollisionType canCollide(EntityPhysicsAttribute aEntityPhysics) {
        switch (mAttribute) {
            case Attribute.ghost:
                return CollisionType.pass;
            case Attribute.ornament:
            case Attribute.walking:
            case Attribute.flying:
                switch (aEntityPhysics.mAttribute) {
                    case Attribute.ghost:
                        return CollisionType.pass;
                    case Attribute.ornament:
                    case Attribute.walking:
                    case Attribute.flying:
                        return CollisionType.collide;
                    case Attribute.pygmy:
                        return CollisionType.pass;
                }
                break;
            case Attribute.pygmy:
                switch (aEntityPhysics.mAttribute) {
                    case Attribute.ghost:
                        return CollisionType.pass;
                    case Attribute.ornament:
                    case Attribute.walking:
                    case Attribute.flying:
                    case Attribute.pygmy:
                        return CollisionType.pass;
                }
                break;
        }
        Debug.LogWarning("EntityPhysicsAttribute : 定義されていない属性の当たり判定「" + mAttribute.ToString() + "」「" + aEntityPhysics.mAttribute.ToString() + "」");
        return CollisionType.collide;
    }
}
