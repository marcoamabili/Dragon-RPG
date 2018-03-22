using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehavior : AbilityBehavior
    {
        PlayerControl player = null;

        private void Awake()
        {
            player = FindObjectOfType<PlayerControl>();
        }

        public override void Use(GameObject target)
        {
            PlayParticleEffect();
            PlayAbilitySound();
            PlayAbilityAnimation();
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
