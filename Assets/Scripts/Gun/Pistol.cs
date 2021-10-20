using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Projectile;

public class Pistol : GunBase
{
    [Header("Bullet Prefab")]
    [SerializeField] Bullet _bullet = null;

    protected override void Shoot()
    {
        Instantiate(_bullet, _muzzleLocation, false);
        _bullet.Speed = _bulletTravelSpeed;
    }

    protected override void EquipWeapon()
    {

    }
}
