﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;


namespace RPG.Characters
{

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip[] audioClips = null;
        [SerializeField] AnimationClip abilityAnimation = null;

        protected AbilityBehavior behavior;

        public abstract AbilityBehavior GetBehaviorComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToAttachTo)
        {
            AbilityBehavior behaviorComponent = GetBehaviorComponent(objectToAttachTo);
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public void Use(GameObject target)
        {
            behavior.Use(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetRandomAbilitySound()
        {
            
            return audioClips[Random.Range(0, audioClips.Length)];
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

    }

}