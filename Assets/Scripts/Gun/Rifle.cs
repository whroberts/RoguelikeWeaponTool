using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Projectile;

public class Rifle : GunBase
{
    [Header("Bullet Prefab")]
    [SerializeField] GameObject _bullet = null;

    bool _shooting = false;

    protected override void Shoot(int shots)
    {
        if (!_canHoldTrigger)
        {
            StartCoroutine(ShotDelay(shots));
        }
        else if (_canHoldTrigger)
        {
            StartCoroutine(Automatic());
        }

    }

    protected override void EquipWeapon()
    {

    }

    IEnumerator ShotDelay(int shots)
    {
        for (int i = 0; i < shots; i++)
        {
            GameObject bullet = Instantiate(_bullet, _muzzleLocation.position, _muzzleLocation.rotation);

            Bullet b = bullet.GetComponent<Bullet>();

            if (b != null)
            {
                b.Speed = _bulletTravelSpeed;
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator Automatic()
    {
        if (!_shooting)
        {
            _shooting = true;
            GameObject bullet = Instantiate(_bullet, _muzzleLocation.position, _muzzleLocation.rotation);

            Bullet b = bullet.GetComponent<Bullet>();

            if (b != null)
            {
                b.Speed = _bulletTravelSpeed;
            }
            yield return new WaitForSeconds(0.1f);
            _shooting = false;
        }
    }
}
