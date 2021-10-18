using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class MagicBaseData : WeaponData
{
    public BaseMagicType _baseMagicType;
    public MagicFireType _magicFireType;


    /*  -- not implemented
     * is based on BaseMagicType
    */
    public Object _basePrefab;
    
    public string _name;
    public float _damage;
    public float _heals;

    //havent added to weapon creation window

    public float _accuracy;
    public float _abilityTravelSpeed;
}
