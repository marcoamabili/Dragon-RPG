using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbilityBehavior
    {

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            DealDamage(target);
            PlayParticleEffect();
            PlayAbilityAnimation();
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            var damageable = target.GetComponent<HealthSystem>();
            if (damageable)
            {
                damageable.TakeDamage(damageToDeal);
            }
            
            
        }

    }
}