using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAttribute : MyBehaviour {
    [SerializeField] public Attribute mAttribute;
    public Type type{
        get{
            if (mAttribute < Attribute.ghost) return Type.terrain;
            if (mAttribute < Attribute.empty) return Type.entity;
            return Type.eventTrigger;
        }
    }
    public enum Attribute{
        //地形
        none,
        air,
        flat,
        water,
        magma,
        wall,
        bridge,
        ladder,
        //もの
        ghost,
        ornament,
        character,
        flying,
        pygmy,
        //イベント
        empty,
        environment,
        force
    }
    public enum Type{
        terrain,entity,eventTrigger
    }

    public bool isCollide(MapAttribute aAttribute){
        if (aAttribute.type == Type.entity)
            return MapAttribute.isCollide(aAttribute.mAttribute, mAttribute);
        if (type == Type.entity)
            return MapAttribute.isCollide(mAttribute, aAttribute.mAttribute);
        return false;
    }
    //第一引数がentity前提
    static bool isCollide(Attribute aEntity,Attribute aAttribute){
        switch(aEntity){
            case Attribute.ghost:
                return false;
            case Attribute.ornament:
                switch(aAttribute){
                    case Attribute.wall:
                    case Attribute.ladder:
                    case Attribute.ornament:
                    case Attribute.character:
                    case Attribute.flying:
                    case Attribute.pygmy:
                        return true;
                }
                return false;
            case Attribute.character:
                switch(aAttribute){
                    //地形
                    case Attribute.none:return true;
                    case Attribute.air:return true;
                    case Attribute.flat:return false;
                    case Attribute.water:return true;
                    case Attribute.magma:return true;
                    case Attribute.wall:return true;
                    case Attribute.bridge:return false;
                    case Attribute.ladder:return false;
                    //もの
                    case Attribute.ghost:return false;
                    case Attribute.ornament:return true;
                    case Attribute.character:return true;
                    case Attribute.flying:return true;
                    case Attribute.pygmy:return false;
                }
                return false;
            case Attribute.flying:
                switch(aAttribute){
                    //地形
                    case Attribute.none: return true;
                    case Attribute.air: return true;
                    case Attribute.flat: return false;
                    case Attribute.water: return false;
                    case Attribute.magma: return true;
                    case Attribute.wall: return true;
                    case Attribute.bridge: return false;
                    case Attribute.ladder: return true;
                    //もの
                    case Attribute.ghost: return true;
                    case Attribute.ornament: return true;
                    case Attribute.character: return true;
                    case Attribute.flying: return true;
                    case Attribute.pygmy: return false;
                }
                return false;
            case Attribute.pygmy:
                switch (aAttribute){
                    //地形
                    case Attribute.none: return true;
                    case Attribute.air: return false;
                    case Attribute.flat: return false;
                    case Attribute.water: return false;
                    case Attribute.magma: return false;
                    case Attribute.wall: return true;
                    case Attribute.bridge: return false;
                    case Attribute.ladder: return false;
                    //もの
                    case Attribute.ghost: return false;
                    case Attribute.ornament: return true;
                    case Attribute.character: return false;
                    case Attribute.flying: return false;
                    case Attribute.pygmy: return true;
                }
                return false;
            default:return false;
        }
    }
}
