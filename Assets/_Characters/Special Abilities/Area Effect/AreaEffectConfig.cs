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

        public override void AttachComponentTo(GameObject gameObjecttoAttachTo)
        {
            var behaviorComponent = gameObjecttoAttachTo.AddComponent<AreaEffectBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
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
