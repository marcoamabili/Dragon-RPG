﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SpinMe : MonoBehaviour
    {

        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;

        void Update()
        {
            float xDegreesPerFrame = xRotationsPerMinute / 60 * Time.deltaTime * 360;
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = yRotationsPerMinute / 60 * Time.deltaTime * 360;
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = zRotationsPerMinute / 60 * Time.deltaTime * 360;
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}