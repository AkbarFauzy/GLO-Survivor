using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Player;
using Survivor.Mechanic.Weapons;
using UnityEngine;

namespace Survivor.Mechanic.UI
{
/*    [ExecuteAlways]*/
    public class PowerUpSelector : Observer
    {
        [SerializeField] private PlayerWeaponSystem _playerWeaponSystem;
        [SerializeField] private GameObject _panel;
        [SerializeField] private List<PowerUpOptionsCard> options;
        [SerializeField] private List<GameObject> _items;
        private List<Weapon> _equippedWeapons;

        private void Awake()
        {
            _panel.SetActive(false);
        }
        private void Start()
        {
            foreach (var option in options)
            {
                option.AddObserver(this);
            }

            _equippedWeapons = new List<Weapon>();
            if (WeaponManager.Instance != null)
            {
                _items = WeaponManager.Instance.GetAllWeaponPrefabs();
            }
            else
            {
                Debug.LogError("WeaponManager is Missing or No Weapon in WeaponManager");
            }

            Subscribe<Player>(Events.OnPlayerLevelUp, OnPlayerLevelUp);
            Subscribe<GameObject>(Events.PlayerGetWeapon, OnPlayerGetWeapon);
        }

        private void Update()
        {
            // Validation
            if (!Application.isPlaying)
            {
                if (_playerWeaponSystem == null)
                {
                    Debug.LogError($"PlayerWeaponSystem reference is missing in {gameObject.name}");
                }
            }
          
        }

        private void OnDisable()
        {
            foreach (var option in options) {
                option.gameObject.SetActive(false);
            }
        }

        public List<GameObject> GetRandomWeapons(int maxSelections = 3)
        {
            if (_items.Count == 0)
            {
                Debug.Log("No weapons available to select.");
                return null;
            }

            List<GameObject> selectedWeapons = new List<GameObject>();
            List<GameObject> tempPool = new List<GameObject>(_items);

            int selections = Mathf.Min(maxSelections, tempPool.Count);

            for (int i = 0; i < selections; i++)
            {
                int randomIndex = Random.Range(0, tempPool.Count);
                selectedWeapons.Add(tempPool[randomIndex]);
                tempPool.RemoveAt(randomIndex);
            }

            return selectedWeapons;
        }

        private void OnPlayerLevelUp(Player player)
        {
            _panel.SetActive(true);

            List<GameObject> randomItems = GetRandomWeapons();

            if (randomItems != null)
            {
                for (int i = 0; i < randomItems.Count; i++)
                {
                    GameObject randomItem = randomItems[i];
                    Weapon randomWeapon = randomItem.GetComponent<Weapon>();

                    // Check if the random weapon is already equipped
                    Weapon equippedWeapon = _playerWeaponSystem.equippedWeapons.Find(w =>
                        w.WeaponData.weaponName == randomWeapon.WeaponData.weaponName);

                    if (equippedWeapon != null)
                    {
                        // Swap with equipped weapon
                        randomItem = equippedWeapon.gameObject;
                    }

                    // Setup weapon in the corresponding PowerUpOptionsCard
                    options[i].SetupWeapon(randomItem);
                    options[i].gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.Log("No weapons available to select.");
            }

            Time.timeScale = 0;
            gameObject.SetActive(true);
        }                                                

        private void OnPlayerGetWeapon(GameObject weapon)
        {
            NotifyEvents<GameObject>(Events.OnPlayerGetWeapon, weapon);
/*            SaveSelectedItem(weapon);*/
            _panel.SetActive(false);
            Time.timeScale = 1;
        }

        private void SaveSelectedItem(GameObject weaponPrefab)
        {
            Weapon weaponPrefabComponent = weaponPrefab.GetComponent<Weapon>();

            if (weaponPrefabComponent == null)
            {
                Debug.LogError($"The weapon prefab {weaponPrefab.name} is missing a Weapon component!");
                return;
            }

            WeaponData weaponData = weaponPrefabComponent.WeaponData;
            Weapon existingWeapon = _equippedWeapons.Find(w => w.WeaponData == weaponData);

            if (existingWeapon != null)
            {
                existingWeapon.LevelUp();
            }
            else
            {
                _equippedWeapons.Add(weaponPrefabComponent);
                Debug.Log($"Player Equip{ weaponPrefabComponent.WeaponData.name}");
            }
        }
    }
}
