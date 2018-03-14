using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core; // TODO consider rewire

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter; // so can inspect when paused
        const float DESTROY_DELAY = 0.01f;
        float damageCaused; // other classes can set

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;

            if (shooter && layerCollidedWith != shooter.layer)
            {
               // DamageIfDamageable(collision);
            }

            Destroy(gameObject, DESTROY_DELAY);
        }

        // TODO reimplement

        //private void DamageIfDamageable(Collision collision)
        //{
        //    var damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));

        //    if (damageableComponent)
        //    {
        //        (damageableComponent as IDamageable).TakeDamage(damageCaused);
        //    }
        //}

        internal float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }
    }
}