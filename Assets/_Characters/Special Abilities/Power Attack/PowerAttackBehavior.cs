using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        public void Use(AbilityUseParams useParams)
        {
            PlaySound();
            DealDamage(useParams);
            PlayParticleEffect();
        }

        private void PlaySound()

        {
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
            
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = particlePrefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(particlePrefab, myParticleSystem.main.duration);
        }
    }
}