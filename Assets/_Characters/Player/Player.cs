using System;
using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.Assertions;
// TODO consider rewire
using RPG.CameraUI;
using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour
    {

        [SerializeField] float baseDamage = 10f;
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = .1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] Weapon currentWeaponConfig = null;
        [SerializeField] ParticleSystem criticalHitParticles = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;


        DynamicCharacterAvatar avatar;
        Dictionary<string, DnaSetter> dna;
        Enemy enemy;
        Animator anim;
        CameraRaycaster cameraRaycaster;
        GameObject weaponObject;
        SpecialAbilities abilities;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        float lastHitTime = 0f;
        
        private void Awake()
        {
            anim = GetComponent<Animator>();
            avatar = GetComponent<DynamicCharacterAvatar>(); // for UMA characters
            abilities = GetComponent<SpecialAbilities>();
        }

        private void Start()
        {
            RegisterForMouseClick();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }


        private void Update()
        {
            SetUMAAttributes(); // for UMA character

            var healthAsPercentage = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }

        }

        private void SetUMAAttributes()
        {
            // if UMA, get avatar and DNA
            if (avatar) { dna = avatar.GetDNA(); }
            if (dna != null && dna.Count != 0)
            {
                dna["breastSize"].Set(1f);
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }
        
        private void SetAttackAnimation()
        {
            anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip(); // TODO remove constant
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominanthand = RequestDominantHand();

            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominanthand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
            // TODO move to correct place and child to hand;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHand = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHand.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on player, please remove one");
            return dominantHand[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;


        }

        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemyToSet.gameObject))
            {
                AttackTarget();
            }
            else if (Input.GetMouseButton(1)) // Power attack with right mouse
            {
                transform.LookAt(enemy.transform);
                //anim.SetTrigger(ATTACK_TRIGGER);
                abilities.AttemptSpecialAbility(0);
            }

        }


        private bool IsTargetInRange(GameObject target)
        {
            float distancetoTarget = (target.transform.position - transform.position).magnitude;
            return distancetoTarget <= currentWeaponConfig.GetMaxAttackRange();
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                transform.LookAt(enemy.transform);
                anim.SetTrigger(ATTACK_TRIGGER);

                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            bool isCriticalHit = UnityEngine.Random.value <= criticalHitChance;
            float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            if (isCriticalHit)
            {
                criticalHitParticles.Play();
                return damageBeforeCritical * criticalHitMultiplier;
                
            }
            else
            {
                return damageBeforeCritical;
            }
        }

    }
}