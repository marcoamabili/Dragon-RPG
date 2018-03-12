using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehavior : AbilityBehavior
    {


        public override void Use(AbilityUseParams useParams)
        {
            PlayAbilitySound();
            DealRadialDamage(useParams);
            PlayParticleEffect();
        }


        private void DealRadialDamage(AbilityUseParams useParams)
        {
            // static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as AreaEffectConfig).GetRadius(),
                Vector3.up,
                (config as AreaEffectConfig).GetRadius()
                );

            // for each hit, if damageable, deal damage to target
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget() + useParams.baseDamage;
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }


    }
}
