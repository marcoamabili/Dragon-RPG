﻿using System.Collections;
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
        [SerializeField] float minTimeBetweenHits = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public float GetMinTimeBetweenHits()
        {
            return minTimeBetweenHits;
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
    }
}