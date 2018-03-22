using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;
using System;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] Image healthBar;
    [SerializeField] AudioClip[] ouchSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] float deathVanishSeconds = 2f;

    Animator animator;
    AudioSource audioSource;
    Character characterMovement;

    const string DEATH_TRIGGER = "Death";
    float currentHealthPoints;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    private void Start()
    {
        characterMovement = GetComponent<Character>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetCurrentMaxHealth();
    }

    void Update ()
    {
        UpdateHealthBar();	
	}

    public void Heal(float points)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0, maxHealthPoints);
    }

    public void TakeDamage(float damage)
    {
        bool characterDies = (currentHealthPoints - damage) <= 0;
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
        var clip = ouchSounds[UnityEngine.Random.Range(0, ouchSounds.Length)];
        audioSource.PlayOneShot(clip);
        if (characterDies)
        {
            StartCoroutine(KillCharacter());
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar) // Enemies may not have health bar to update
        {
            healthBar.fillAmount = healthAsPercentage;
        }
    }

    private void SetCurrentMaxHealth()
    {
        currentHealthPoints = maxHealthPoints;
    }

    IEnumerator KillCharacter()
    {
        StopAllCoroutines();
        characterMovement.Kill();
        animator.SetTrigger(DEATH_TRIGGER);

        var playerComponent = GetComponent<PlayerControl>();
        if (playerComponent && playerComponent.isActiveAndEnabled)
        {
            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play(); // override existing sounds

            yield return new WaitForSecondsRealtime(audioSource.clip.length); // TODO use audio clip length
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else // assume it's enemy for now
        {
            DestroyObject(gameObject, deathVanishSeconds);
        }

        

    }


}
