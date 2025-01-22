using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic.Weapons;
using UnityEngine;

namespace Survivor.Mechanic.Loot
{
    public class LootChest : MonoBehaviour
    {
        [SerializeField] private AudioClip pickupSound;
        [SerializeField] private ParticleSystem upgradeEffect;

        private void OnTriggerEnter(Collider other)
        {
            // Check if the player interacts with the chest
            if (other.CompareTag("Player"))
            {
                PlayerWeaponSystem weaponSystem = other.GetComponent<PlayerWeaponSystem>();
                if (weaponSystem == null)
                {
                    Debug.LogError("PlayerWeaponSystem not found on the player!");
                    return;
                }

                // Attempt to upgrade a weapon
                UpgradeRandomWeapon(weaponSystem);

                // Provide feedback (optional)
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }
                if (upgradeEffect != null)
                {
                    Instantiate(upgradeEffect, transform.position, Quaternion.identity);
                }

                // Destroy the loot chest
                Destroy(gameObject);
            }
        }

        private void UpgradeRandomWeapon(PlayerWeaponSystem weaponSystem)
        {
            // Filter for weapons that can still be upgraded
            List<Weapon> upgradableWeapons = weaponSystem.equippedWeapons.FindAll(w => !w.IsMaxLevel);

            if (upgradableWeapons.Count == 0)
            {
                Debug.Log("No upgradable weapons available!");
                return;
            }

            // Select a random weapon and upgrade it
            int randomIndex = Random.Range(0, upgradableWeapons.Count);
            Weapon selectedWeapon = upgradableWeapons[randomIndex];

            selectedWeapon.LevelUp();
            Debug.Log($"Upgraded {selectedWeapon.WeaponData.weaponName} to level {selectedWeapon.Level}");
        }


    }


}
