using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPhysicsAttribute : MapPhysicsAttribute {
    ///<summary>物体属性</summary>
    [SerializeField] public Attribute mAttribute;
    public enum Attribute {
        ghost,
        ornament,
        walking,
        flying,
        pygmy
    }

    ///この属性が付与されているentity
    [SerializeField] public MapEntity mEntity;
    public override MapBehaviour mBehaviour {
        get { return mEntity; }
        set { mEntity = (MapEntity)value; }
    }
    ///<summary>このbehaviourが引数のtileに侵入できるか</summary>
    public bool canEnter(TileGroundPhysicsAttribute aTilePhysics) {
        switch (mAttribute) {
            case Attribute.ghost:
                return true;
            case Attribute.ornament:
            case Attribute.walking:
                switch (aTilePhysics.mAttribute) {
                    case TileGroundPhysicsAttribute.Attribute.none:
                    case TileGroundPhysicsAttribute.Attribute.end:
                    case TileGroundPhysicsAttribute.Attribute.air:
                    case TileGroundPhysicsAttribute.Attribute.water:
                    case TileGroundPhysicsAttribute.Attribute.magma:
                    case TileGroundPhysicsAttribute.Attribute.wall:
                        return false;
                    case TileGroundPhysicsAttribute.Attribute.flat:
                        return true;
                }
                break;
            case Attribute.flying:
                switch (aTilePhysics.mAttribute) {
                    case TileGroundPhysicsAttribute.Attribute.none:
                    case TileGroundPhysicsAttribute.Attribute.end:
                    case TileGroundPhysicsAttribute.Attribute.wall:
                        return false;
                    case TileGroundPhysicsAttribute.Attribute.flat:
                    case TileGroundPhysicsAttribute.Attribute.water:
                    case TileGroundPhysicsAttribute.Attribute.magma:
                    case TileGroundPhysicsAttribute.Attribute.air:
                        return true;
                }
                break;
            case Attribute.pygmy:
                switch (aTilePhysics.mAttribute) {
                    case TileGroundPhysicsAttribute.Attribute.none:
                    case TileGroundPhysicsAttribute.Attribute.end:
                    case TileGroundPhysicsAttribute.Attribute.wall:
                        return false;
                    case TileGroundPhysicsAttribute.Attribute.flat:
                    case TileGroundPhysicsAttribute.Attribute.water:
                    case TileGroundPhysicsAttribute.Attribute.magma:
                    case TileGroundPhysicsAttribute.Attribute.air:
                        return true;
                }
                break;
        }
        Debug.LogWarning("EntityPhysicsAttribute : 定義されていない属性の当たり判定「" + mAttribute.ToString() + "」「" + aTilePhysics.mAttribute.ToString() + "」");
        return false;
    }
    //<summary>このbehaviourが引数のentityがいる位置に侵入できるか</summary>
    public bool canEnter(EntityPhysicsAttribute aEntityPhysics) {
        switch (mAttribute) {
            case Attribute.ghost:
                return true;
            case Attribute.ornament:
            case Attribute.walking:
            case Attribute.flying:
                switch (aEntityPhysics.mAttribute) {
                    case Attribute.ghost:
                        return true;
                    case Attribute.ornament:
                    case Attribute.walking:
                    case Attribute.flying:
                        return false;
                    case Attribute.pygmy:
                        return true;
                }
                break;
            case Attribute.pygmy:
                switch (aEntityPhysics.mAttribute) {
                    case Attribute.ghost:
                        return true;
                    case Attribute.ornament:
                    case Attribute.walking:
                    case Attribute.flying:
                    case Attribute.pygmy:
                        return true;
                }
                break;
        }
        Debug.LogWarning("EntityPhysicsAttribute : 定義されていない属性の当たり判定「" + mAttribute.ToString() + "」「" + aEntityPhysics.mAttribute.ToString() + "」");
        return false;
    }
}
