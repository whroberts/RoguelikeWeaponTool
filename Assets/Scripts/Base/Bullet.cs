using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Bullet : ProjectileBase
    {
        protected override void DoIt()
        {
            // plays feedback when instantiated 
            LaunchFeedback();

            Rigidbody rb;
            rb = this.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * _speed;

            Destroy(gameObject, 8f);
        }

    }
}


