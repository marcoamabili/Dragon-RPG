using System;
using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.Assertions;
// TODO consider rewire
using RPG.CameraUI;
using RPG.Core; 
using RPG.Weapons;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] AudioClip[] ouchSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;

        // Temporarily serialized for debugging
        [SerializeField] AbilityConfig[] abilities;

        float currentHealthPoints = 0f;
        const string ATTACK_TRIGGER = "Attack";
        const string DEATH_TRIGGER = "Death";
        DynamicCharacterAvatar avatar;
        Dictionary<string, DnaSetter> dna;
        Enemy enemy = null;
        Animator anim;
        AudioSource audioSource = null;
        CameraRaycaster cameraRaycaster = null;
        float lastHitTime = 0f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / (float)maxHealthPoints;
            }
        }

        public void TakeDamage(float damage)
        {  
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            audioSource.clip = ouchSounds[UnityEngine.Random.Range(0, ouchSounds.Length)];
            audioSource.Play();
            if ( currentHealthPoints <= 0)
            {
                StartCoroutine(KillPlayer());
            }
            
        }

        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0, maxHealthPoints);
        }

        IEnumerator KillPlayer()
        {
            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            anim.SetTrigger(DEATH_TRIGGER);
            yield return new WaitForSecondsRealtime(audioSource.clip.length); // TODO use audio clip length
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            avatar = GetComponent<DynamicCharacterAvatar>();
            AttachInitialAbilities();
            SetCurrentMaxHealth();
            RegisterForMouseClick();
            PutWeaponInHand();
            SetupRuntimeAnimator();

        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachComponentTo(gameObject);
            }
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator()
        {
            anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO remove constant
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominanthand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominanthand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
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

        }

        private void AttemptSpecialAbility(int abilityIndex)
        {
            
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distancetoTarget = (target.transform.position - transform.position).magnitude;
            return distancetoTarget <= weaponInUse.GetMaxAttackRange();
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                transform.LookAt(enemy.transform);
                anim.SetTrigger(ATTACK_TRIGGER);
                //print(weaponInUse.GetAdditionalDamage());
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private void Update()
        {

            if (healthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }

            // for UMA
            //if (avatar) { dna = avatar.GetDNA(); }
            //if (dna != null)
            //{
            //    if (dna.Count != 0)
            //    {
            //        {
            //            dna["breastSize"].Set(1f);
            //        }
            //    }
            //}
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 0; keyIndex < abilities.Length; keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    AttemptSpecialAbility(keyIndex);
                }
            }
        }
    }
}