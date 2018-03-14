﻿using System.Collections;
using UnityEngine;

namespace RPG.Characters
{

    public abstract class AbilityBehavior : MonoBehaviour
    {
        protected AbilityConfig config;
        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public abstract void Use(GameObject target = null);


        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(particlePrefab, 
                transform.position, 
                particlePrefab.transform.rotation, 
                transform); // set world space in prefab if required
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
            
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particleObject)
        {
            while (particleObject.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particleObject);
            yield return new WaitForEndOfFrame();
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }
}