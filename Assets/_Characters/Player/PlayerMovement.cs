using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI; // TODO consider rewiring

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(AICharacterControl))]
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;
        ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
        AICharacterControl aICharacterControl = null;
        CameraRaycaster cameraRaycaster;
        Vector3 currentDestination;
        Vector3 clickPoint;
        GameObject walkTarget = null;

        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            currentDestination = transform.position;
            aICharacterControl = GetComponent<AICharacterControl>();

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnClickEnemy;
            walkTarget = new GameObject("walkTarget");
        }

        private void OnClickEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                aICharacterControl.SetTarget(enemy.transform);
            }
        }

        private void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                aICharacterControl.SetTarget(walkTarget.transform);
            }
        }

       


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // TODO add to menu
            //if (Input.GetKeyDown(KeyCode.G))// G for Gamepad.
            //{
            //    isInDirectMode = !isInDirectMode;
            //    currentDestination = transform.position; // clear the click target
            //}

            //if (isInDirectMode)
            //{
            //    ProcessDirectMovement();
            //}


        }

        // TODO make this called again
        private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            // calculate camera relative direction to move:

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);

        }

    }

}