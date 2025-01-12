using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class PlayerWeaponSystem : Observer
    {
        public Transform weaponParent;

        public List<Weapon> equippedWeapons = new List<Weapon>();

        private void Start()
        {
            if (WeaponManager.Instance == null)
            {
                Debug.LogError("Weapon Manager not in Scene");
                return;
            }

            GameObject defaultGun = WeaponManager.Instance.GetWeaponPrefab("Default Gun");

            EquipWeapon(defaultGun);

            Subscribe<GameObject>(Events.OnPlayerGetWeapon, EquipWeapon);
        }

        public void EquipWeapon(GameObject weaponPrefab)
        {
            // Get the Weapon component from the prefab
            Weapon weaponPrefabComponent = weaponPrefab.GetComponent<Weapon>();

            if (weaponPrefabComponent == null)
            {
                Debug.LogError($"The weapon prefab {weaponPrefab.name} is missing a Weapon component!");
                return;
            }

            // Check if the weapon is already equipped
            WeaponData weaponData = weaponPrefabComponent.WeaponData;
            Weapon existingWeapon = equippedWeapons.Find(w => w.WeaponData == weaponData);

            if (existingWeapon != null)
            {
                // Level up the existing weapon
                Debug.Log(existingWeapon);
                existingWeapon.LevelUp();

                if (existingWeapon.IsMaxLevel) {
                    WeaponManager.Instance.RemoveMaxLevelWeapon(existingWeapon);
                }
            }
            else
            {
                // Instantiate the weapon prefab
                GameObject weaponObject = Instantiate(weaponPrefab, weaponParent);
                Weapon newWeapon = weaponObject.GetComponent<Weapon>();

                if (newWeapon == null)
                {
                    Debug.LogError($"Weapon prefab {weaponObject.name} is missing a Weapon component!");
                    return;
                }

                // Add the weapon to the equipped list
                equippedWeapons.Add(newWeapon);
                Debug.Log($"Player Equip{newWeapon.WeaponData.name}");
            }

        }

        public void UnequipWeapon(Weapon weapon)
        {
            if (equippedWeapons.Contains(weapon))
            {
                equippedWeapons.Remove(weapon);
                Destroy(weapon.gameObject); // Destroy the weapon prefab instance
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (equippedWeapons.Count <= 0) return;

            // Visualize detection radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(equippedWeapons[0] != null ? equippedWeapons[0].transform.position : transform.position, 10f);
        }

    }
}

