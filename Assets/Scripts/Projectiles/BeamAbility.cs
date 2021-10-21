using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class BeamAbility : ProjectileBase
    {

        ParticleSystem _ps;
        protected override void Projectile()
        {
            // plays feedback when instantiated 
            LaunchFeedback();

            Rigidbody rb;
            rb = this.GetComponent<Rigidbody>();

            _ps = GetComponentInChildren<ParticleSystem>();

        }
    }
}
