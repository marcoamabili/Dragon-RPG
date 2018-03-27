using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        public Transform gripTransform;
        
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] AudioClip[] attackSounds;
        [SerializeField] float timeBetweenAnimationCycles = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = 0.5f;

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public float GetDamageDelay()
        {
            return damageDelay;
        }

        public float GetTimeBetweenAnimationCycles()
        {
            return timeBetweenAnimationCycles;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }

        public AudioClip GetAttackSound()
        {
            return attackSounds[Random.Range(0, attackSounds.Length)];
        }
    }
}