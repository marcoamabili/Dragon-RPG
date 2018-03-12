using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Special Ability/Area Effect")]
    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damagetoEachTarget = 15f;

        public override AbilityBehavior GetBehaviorComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<AreaEffectBehavior>();
        }

        public float GetDamageToEachTarget()
        {
            return damagetoEachTarget;
        }

        public float GetRadius()
        {
            return radius;
        }
    }
}
