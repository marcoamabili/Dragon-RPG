using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UMA.CharacterSystem;


namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController runtimeAnimatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Audio Source")]
        [SerializeField] [Range(0, 1)] float spatialBlend2D3D = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3  colliderCenter = new Vector3(0f, 0.9268684f, 0f);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 1.853737f;

        [Header("Movement")]
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveSpeedMultiplier = 0.7f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float moveThreshold = 1f;

        [Header("NavMesh Agent")]
        [SerializeField] float agentSpeed = 3f;
        [SerializeField] float agentAngularSpeed = 120f;
        [SerializeField] float agentAcceleration = 8f;
        [SerializeField] float agentStoppingDistance = 1.2f;
        [SerializeField] float agentRadius = 0.1f;
        [SerializeField] float agentHeight = 2f;

        DynamicCharacterAvatar avatar;
        Dictionary<string, DnaSetter> dna;
        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody myRigidbody;

        float turnAmount;
        float forwardAmount;
        bool isAlive = true;

        private void Awake()
        {
            avatar = GetComponent<DynamicCharacterAvatar>(); // for UMA characters
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();

            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = agentSpeed;
            navMeshAgent.angularSpeed = agentAngularSpeed;
            navMeshAgent.acceleration = agentAcceleration;
            navMeshAgent.stoppingDistance = agentStoppingDistance;
            navMeshAgent.radius = agentRadius;
            navMeshAgent.height = agentHeight;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = false;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = spatialBlend2D3D;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = runtimeAnimatorController;
            animator.avatar = characterAvatar;
            animator.applyRootMotion = true;

        }

        public DynamicCharacterAvatar GetAvatar()
        {
            return avatar;
        }

        public float GetAnimSpeedMultiplier()
        {
            return animator.speed;
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

        private void Update()
        {
            SetUMAAttributes();
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        public void Kill()
        {
            isAlive = false;
        }
        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }

        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMovement = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            forwardAmount = localMovement.z;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }


    }

}