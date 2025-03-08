using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic.UI;
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
            if (defaultGun != null)
            {
                EquipWeapon(defaultGun);
            }
            Subscribe<GameObject>(Events.OnPlayerGetWeapon, EquipWeapon);
        }

        public void EquipWeapon(GameObject weaponPrefab)
        {
            if (weaponPrefab == null)
            {
                Debug.LogError("Weapon prefab is null");
                return;
            }

            Weapon weaponPrefabComponent = weaponPrefab.GetComponent<Weapon>();
            if (weaponPrefabComponent == null)
            {
                Debug.LogError($"The weapon prefab {weaponPrefab.name} is missing a Weapon component!");
                return;
            }

            Weapon existingWeapon = equippedWeapons.Find(w => w.WeaponData == weaponPrefabComponent.WeaponData);
            if (existingWeapon != null)
            {
                LevelUpExistingWeapon(existingWeapon);
            }
            else
            {
                InstantiateAndEquipNewWeapon(weaponPrefab);
            }
        }

        private void LevelUpExistingWeapon(Weapon existingWeapon)
        {
            existingWeapon.LevelUp();
            NotifyWeaponChange(existingWeapon);

            if (existingWeapon.IsMaxLevel)
            {
                WeaponManager.Instance.RemoveMaxLevelWeapon(existingWeapon);
            }
        }

        private void InstantiateAndEquipNewWeapon(GameObject weaponPrefab)
        {
            GameObject weaponObject = Instantiate(weaponPrefab, weaponParent);
            Weapon newWeapon = weaponObject.GetComponent<Weapon>();

            if (newWeapon == null)
            {
                Debug.LogError($"Weapon prefab {weaponObject.name} is missing a Weapon component!");
                Destroy(weaponObject);
                return;
            }

            equippedWeapons.Add(newWeapon);
            NotifyWeaponChange(newWeapon);
            Debug.Log($"Player equipped {newWeapon.WeaponData.name}");
        }

        private void NotifyWeaponChange(Weapon weapon)
        {
            int index = equippedWeapons.IndexOf(weapon);
            NotifyEvents<int, Weapon>(Events.OnPlayerEquipWeapon, index, weapon);
        }

        public void UnequipWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                Debug.LogError("Weapon to unequip is null");
                return;
            }

            if (equippedWeapons.Contains(weapon))
            {
                equippedWeapons.Remove(weapon);
                Destroy(weapon.gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (equippedWeapons.Count <= 0) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(equippedWeapons[0]?.transform.position ?? transform.position, 10f);
        }
    }
}

