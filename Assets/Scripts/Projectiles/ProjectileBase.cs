using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class ProjectileBase : MonoBehaviour
    {
        protected abstract void Projectile();

        [HideInInspector] public float Speed;

        private void Start()
        {
            Projectile();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // feedback for collision impact
            ImpactFeedback();
            // prints what was hit

            //Debug.Log("Hit: " + collision.gameObject.name + " at location - " + collision.gameObject.transform.position);

            // For editing, every damage script is different
        }

        private void OnTriggerEnter(Collider other)
        {
            // feedback for trigger impact
            ImpactFeedback();

            // prints what was hit
            //Debug.Log("Hit: " + other.gameObject.name + " at location - " + other.gameObject.transform.position);

            // For editing, every damage script is different
        }

        protected void LaunchFeedback()
        {
            // feedback for launch
        }


        protected void ImpactFeedback()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            // stops the movement of the projectile
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;

            // hides the visuals and interactions
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            Collider col = GetComponentInChildren<Collider>();
            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();

            if (mesh != null && col != null)
            {
                mesh.enabled = false;
                col.enabled = false;
            }

            if (ps != null)
            {
                ps.Stop();
            }

            // destroys after 3 seconds

            Destroy(gameObject, 3f);
        }
    }
}

