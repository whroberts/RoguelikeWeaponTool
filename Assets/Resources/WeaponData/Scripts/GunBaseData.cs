using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class GunBaseData : WeaponData
{
    public BaseGunType _baseGunType;
    public GunFireType _gunFireType;

    /*  -- not implemented
     * is based on BaseGunType
    */
    public Object _basePrefab;
    
    public string _name;
    public float _damage;

    //havent added to weapon creation window

    public float _accuracy;
    public float _bulletTravelSpeed;

    /* Limiter not implemented
     * based on GunFireType
     * Semi - None or Low or Normal or High
     * Auto - Low or Normal or High
     */
    public RecoilType _recoilType;

}
