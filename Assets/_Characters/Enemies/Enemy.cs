using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
// TODO consider rewire
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [Header("General")]
        [SerializeField]
        GameObject projectileUsed;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] float attackRadius;
        [SerializeField] float chaseRadius;
        [SerializeField] float firingPeriodInSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float deltaYNewUISocketPosition = 1f;
        [SerializeField] Vector3 verticalAimOffset = new Vector3(0, 1f, 0);

        [Header("Attack DNA Values")]
        public float headSize = 0.5f;
        public float legsSize = 0.5f;
        public float armsLength = 0.5f;



        float currentHealthPoints = 100f;
        bool isNewUIPositionSet = false;
        bool isAttacking = false;

        AICharacterControl aiCharacterControl;
        DynamicCharacterAvatar avatar;
        Dictionary<string, DnaSetter> dna;
        Player player = null;
        GameObject UISocket;
        Vector3 initialUISocketPosition;


        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }


        private void Start()
        {
            currentHealthPoints = maxHealthPoints;
            player = GameObject.FindObjectOfType<Player>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            avatar = GetComponent<DynamicCharacterAvatar>();
            UISocket = GetComponentInChildren<EnemyUI>().gameObject;
            initialUISocketPosition = UISocket.transform.localPosition;

        }

        private void Update()
        {
            if(player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this); // To stop enemy behaviour
            }

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            FreakOut(false);
            UISocket.transform.localPosition = initialUISocketPosition;
            isNewUIPositionSet = false;

            if (distanceToPlayer <= attackRadius)
            {
                FreakOut(true);
                transform.LookAt(player.transform);
                // Fire projectile;
                if (!isAttacking)
                {
                    isAttacking = true;
                    float randomizedDelay = Random.Range(firingPeriodInSeconds - firingPeriodVariation, firingPeriodVariation + firingPeriodVariation);
                    InvokeRepeating("FireProjectile", 0f, randomizedDelay);
                }

                if (!isNewUIPositionSet)
                {
                    SetNewUIPosition(new Vector3(0f, deltaYNewUISocketPosition, 0f));
                    isNewUIPositionSet = true;
                }
            }
            else
            {
                CancelInvoke();
                isAttacking = false;
            }

            if (distanceToPlayer <= chaseRadius)
            {
                FreakOut(true);
                aiCharacterControl.SetTarget(player.transform);
                if (!isNewUIPositionSet)
                {
                    SetNewUIPosition(new Vector3(0f, deltaYNewUISocketPosition, 0f));
                    isNewUIPositionSet = true;
                }

            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }

        }


        private void FreakOut(bool active)
        {
            if (avatar == null) return;

            dna = avatar.GetDNA();
            if (dna.Count != 0)
            {
                if (active)
                {
                    dna["headSize"].Set(headSize);
                    dna["legsSize"].Set(legsSize);
                    dna["armLength"].Set(armsLength);
                    avatar.SetColor("Skin", Color.red);
                    avatar.BuildCharacter();
                }
                else
                {
                    dna["headSize"].Set(.5f);
                    dna["legsSize"].Set(.5f);
                    dna["armLength"].Set(.5f);
                    avatar.SetColor("Skin", Color.green);
                    avatar.BuildCharacter();
                }

            }
        }

        private void SetNewUIPosition(Vector3 delta)
        {
            if (avatar == null) return;
            UISocket.transform.localPosition += delta;
        }

        void FireProjectile()
        {
            GameObject projectile = Instantiate(projectileUsed, projectileSocket.transform.position, Quaternion.identity, projectileSocket.transform);
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.SetShooter(gameObject);
            projectileComponent.SetDamage(damagePerShot);

            Vector3 unitVectorToPlayer = (player.transform.position + verticalAimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            projectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;


        }

        void OnDrawGizmos()
        {

            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = new Color(0, 0, 255f, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }

    }
}