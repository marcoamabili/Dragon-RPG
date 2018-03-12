using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {

        Transform player;
        Vector3 offset;

        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = player.position;
        }
    }
}