﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehavior : AbilityBehavior
    {

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            PlayAbilityAnimation();
            DealRadialDamage();
            PlayParticleEffect();
        }


        private void DealRadialDamage()
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
                var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerControl>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }


    }
}
