using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class GunBaseData : WeaponData
{
    public BaseGunType _baseGunType;
    public GunFireType _gunFireType;

    public Object _basePrefab;
    
    public string _name;
    public float _damage = 10;

    public float _accuracy = 90;
    public float _bulletTravelSpeed = 25;

    public RecoilType _recoilType;

}
