using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    [Header("Bullet Prefab")]
    [SerializeField] Bullet _bullet = null;

    protected override void Shoot()
    {
        Instantiate(_bullet, _muzzleLocation, false);
    }

    protected override void EquipWeapon()
    {

    }
}
