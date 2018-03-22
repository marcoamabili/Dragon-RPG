using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbilityBehavior
    {

        public override void Use(GameObject target)
        {

            PlayAbilitySound();
            PlayAbilityAnimation();
            DealDamage(target);
            PlayParticleEffect();
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