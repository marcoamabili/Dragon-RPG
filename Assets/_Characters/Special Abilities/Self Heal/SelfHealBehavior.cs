using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehavior : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config = null;
        AudioSource audioSource = null;
        Player player = null;

        private void Awake()
        {
            player = FindObjectOfType<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(SelfHealConfig configToSet)
        {
            this.config = configToSet;
        }

        public void Use(AbilityUseParams useParams)
        {
            PlaySound();
            RegenHealth();
            PlayParticleEffect();
        }

        private void PlaySound()
        {
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }

        private void RegenHealth()
        {
            player.Heal(config.GetExtraHealth());   
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity, transform);
            ParticleSystem myParticleSystem = particlePrefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(particlePrefab, myParticleSystem.main.duration);
        }
    }
}
