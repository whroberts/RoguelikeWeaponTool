using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class CastAbility : ProjectileBase
    {
        protected override void Projectile()
        {
            // plays feedback when instantiated 
            LaunchFeedback();

            Rigidbody rb;
            rb = this.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * Speed;

            Destroy(gameObject, 8f);
        }
    }

}
