using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(HealthSystem))]
    public class EnemyAI : MonoBehaviour
    {

        [Header("General")]
        [SerializeField] float chaseRadius;
        [SerializeField] float deltaYNewUISocketPosition = 1f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2f;

        //[Header("Attack DNA Values")]
        //public float headSize = 0.5f;
        //public float legsSize = 0.5f;
        //public float armsLength = 0.5f;

        //bool isNewUIPositionSet = false;
        float currentWeaponRange;
        float distanceToPlayer;
        int nextWaypointIndex;
        PlayerControl player = null;
        Character character;

        enum State {idle, patrolling, attacking, chasing }

        State state = State.idle;

        //DynamicCharacterAvatar avatar;
        //Dictionary<string, DnaSetter> dna;

        // GameObject UISocket;
        Vector3 initialUISocketPosition;
        

        private void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
            
           // avatar = GetComponent<DynamicCharacterAvatar>();
           // UISocket = GetComponentInChildren<EnemyUI>().gameObject;
           // initialUISocketPosition = UISocket.transform.localPosition;
        }

        private void Update()
        {

            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            
            // Freak out false

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }

            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                // Stop what we re doing
                // Freak out
                // Chase the player
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }

            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {

                // Stop what we re doing
                // Freak out
                // Attack the player
                StopAllCoroutines();
                state = State.attacking;
            }



            //  FreakOut(false);
            ////UISocket.transform.localPosition = initialUISocketPosition;
            //  isNewUIPositionSet = false;

            //  if (distanceToPlayer <= currentWeaponRange)
            //  {
            //      FreakOut(true);
            //      transform.LookAt(player.transform);
            //      // Fire projectile;
            //      if (!isAttacking)
            //      {
            //          isAttacking = true;
            //          float randomizedDelay = Random.Range(firingPeriodInSeconds - firingPeriodVariation, firingPeriodVariation + firingPeriodVariation);
            //         // InvokeRepeating("FireProjectile", 0f, randomizedDelay);
            //      }

            //      if (!isNewUIPositionSet)
            //      {
            //          SetNewUIPosition(new Vector3(0f, deltaYNewUISocketPosition, 0f));
            //          isNewUIPositionSet = true;
            //      }
            //  }
            //  else
            //  {
            //      CancelInvoke();
            //      isAttacking = false;
            //  }

            //  if (distanceToPlayer <= chaseRadius)
            //  {
            //      FreakOut(true);
            //      //aiCharacterControl.SetTarget(player.transform);
            //      if (!isNewUIPositionSet)
            //      {
            //          SetNewUIPosition(new Vector3(0f, deltaYNewUISocketPosition, 0f));
            //          isNewUIPositionSet = true;
            //      }

            //  }
            //  else
            //  {
            //      //aiCharacterControl.SetTarget(transform);
            //  }

        }


        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }

        }

        IEnumerator Patrol()
        {
            state = State.patrolling;
            while (patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(0.5f); // todo parameterize
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position , nextWaypointPos) < waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
            

        }

        void OnDrawGizmos()
        {

            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0, 0, 255f, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }




        //private void FreakOut(bool active)
        //{
        //    if (avatar == null) return;

        //    dna = avatar.GetDNA();
        //    if (dna.Count != 0)
        //    {
        //        if (active)
        //        {
        //            dna["headSize"].Set(headSize);
        //            dna["legsSize"].Set(legsSize);
        //            dna["armLength"].Set(armsLength);
        //            avatar.SetColor("Skin", Color.red);
        //            avatar.BuildCharacter();
        //        }
        //        else
        //        {
        //            dna["headSize"].Set(.5f);
        //            dna["legsSize"].Set(.5f);
        //            dna["armLength"].Set(.5f);
        //            avatar.SetColor("Skin", Color.green);
        //            avatar.BuildCharacter();
        //        }

        //    }
        //}

        //private void SetNewUIPosition(Vector3 delta)
        //{
        //    if (avatar == null) return;
        //  //  UISocket.transform.localPosition += delta;
        //}



    }
}