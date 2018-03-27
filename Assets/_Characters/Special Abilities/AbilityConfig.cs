using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Spcial Ability General")]
        [SerializeField]
        float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AnimationClip abilityAnimation;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehavior behaviour;

        public abstract AbilityBehavior GetBehaviorComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToattachTo)
        {
            AbilityBehavior behaviourComponent = GetBehaviorComponent(objectToattachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AnimationClip GetAbilityAnimation()
        {
            RemoveAnimationEvents();
            return abilityAnimation;
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            abilityAnimation.events = new AnimationEvent[0];
        }

        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}