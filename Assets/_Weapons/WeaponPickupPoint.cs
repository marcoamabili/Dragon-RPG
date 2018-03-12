using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {

    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSfx;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update()
        {
            DestroyChildern();
            InstantiateWeapon();
            
        }

        private void DestroyChildern()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }
    }
}