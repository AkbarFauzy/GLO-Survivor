using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance { get; private set; }

        [SerializeField]
        private List<GameObject> _weaponPrefabs;
        private Dictionary<string, GameObject> _weaponDictionary;

        private List<GameObject> _availableWeapons;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            InitializeWeaponDictionary();
            InitializeAvailableWeapons();
        }


        private void InitializeWeaponDictionary()
        {
            _weaponDictionary = new Dictionary<string, GameObject>();

            foreach (GameObject prefab in _weaponPrefabs)
            {
                Weapon weaponComponent = prefab.GetComponent<Weapon>();
                if (weaponComponent != null && weaponComponent.WeaponData != null)
                {
                    string weaponName = weaponComponent.WeaponData.weaponName;
                    if (!_weaponDictionary.ContainsKey(weaponName))
                    {
                        _weaponDictionary.Add(weaponName, prefab);
                    }
                    else
                    {
                        Debug.LogWarning($"Duplicate weapon detected: {weaponName}. Only the first will be used.");
                    }
                }
                else
                {
                    Debug.LogError($"Weapon prefab {prefab.name} is missing a Weapon component or WeaponData.");
                }
            }
        }

        private void InitializeAvailableWeapons()
        {
            _availableWeapons = new List<GameObject>(_weaponDictionary.Values);
        }

        public GameObject GetWeaponPrefab(string weaponName)
        {
            if (_weaponDictionary.TryGetValue(weaponName, out GameObject prefab))
            {
                return prefab;
            }
            else
            {
                Debug.LogError($"Weapon with name {weaponName} not found in WeaponManager.");
                return null;
            }
        }

        public List<GameObject> GetAllWeaponPrefabs()
        {
            return _availableWeapons;
        }

        public void RemoveMaxLevelWeapon(Weapon weapon)
        {
            GameObject weaponPrefab = GetWeaponPrefab(weapon.WeaponData.weaponName);
            if (weaponPrefab != null && _availableWeapons.Contains(weaponPrefab))
            {
                _availableWeapons.Remove(weaponPrefab);
                Debug.Log($"{weapon.WeaponData.weaponName} reached max level and is removed from available weapons.", gameObject);
            }
        }
    }
}