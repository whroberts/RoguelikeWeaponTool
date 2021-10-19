using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        //ShootBullet();
    }

    /*
    private void ShootBullet()
    {
        Rigidbody rb;
        rb = _bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * _travelSpeed;

        Destroy(gameObject, 8f);
    }
    */
}
