using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehavior : AbilityBehavior
    {
        Player player = null;

        private void Awake()
        {
            player = FindObjectOfType<Player>();
        }

        public override void Use(AbilityUseParams useParams)
        {
            PlayParticleEffect();
            PlayAbilitySound();
            player.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
