﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Special Ability/Power Attack")]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDamage = 10f;

       public override void AttachComponentTo(GameObject gameObjecttoAttachTo)
        {
            var behaviorComponent = gameObjecttoAttachTo.AddComponent<PowerAttackBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}