using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class MagicBaseData : WeaponData
{
    public BaseMagicType _baseMagicType;
    public MagicAbilityType _magicAbilityType;


    /*  -- not implemented
     * is based on BaseMagicType
    */
    public Object _basePrefab;
    
    public string _name;

    public float _mana;
    public float _manaRechargeRate = 5;
    public float _rechargeDelay = 1;

    public float _damageValue = 0;
    public float _healingValue = 0;
    
    public float _castLaunchSpeed = 10;

    public float _coolDown = 0;
}
