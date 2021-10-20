using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Bullet : ProjectileBase
    {
        protected override void DoIt()
        {
            Debug.Log("Shot Bullet");
            // plays feedback when instantiated 
            LaunchFeedback();

            Rigidbody rb;
            rb = this.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * Speed;

            Destroy(gameObject, 8f);
        }

    }
}


