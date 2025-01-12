using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic.Weapons.Projectiles;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class LandminesBehavior : IWeaponBehavior
    {
        private Landmines weapon;
        private int maxMines = 3; // Max number of active landmines
        private float cooldownInterval = 1f; // Time between placing mines

        private float cooldownTimer = 0f; // Timer for cooldown between placements
        private List<GameObject> activeMines = new List<GameObject>(); // Track active mines

        public void Initialize<T>(T weapon) where T : Weapon
        {
            if (weapon is Landmines forceField)
            {
                this.weapon = forceField;
            }
            else
            {
                // Handle the case where the weapon is not a TeslaWeapon
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
            }
        }

        public void Fire()
        {
            // Cooldown for placing a new mine
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
                return;
            }
            Debug.Log(activeMines.Count);
            // Limit the number of active mines
            if (activeMines.Count >= maxMines) return;

            // Place a new landmine
            GameObject mine = weapon.Pool.GetProjectile();
            if (mine != null)
            {
                PlaceLandmine(mine);
                activeMines.Add(mine);

                // Start cooldown timer
                cooldownTimer = cooldownInterval;
            }
        }

        public void LevelUp(int newLevel)
        {
            maxMines = Mathf.Min(5, newLevel + 2); // Increase max mines, up to 5
            Debug.Log($"Landmine Weapon leveled up to {newLevel}: Now allows {maxMines} active mines.");
        }

        private void PlaceLandmine(GameObject mine)
        {
            // Position mine where the player is standing
            mine.transform.position = weapon.transform.position;
            mine.SetActive(true);

            // Configure the landmine's explosion behavior
            LandmineProjectile mineScript = mine.GetComponent<LandmineProjectile>();
            if (mineScript != null)
            {
                mineScript.Init(weapon.GetRange(), weapon.GetDamage());
                mineScript.OnExplode += HandleMineExplosion;
            }
        }

        private void HandleMineExplosion(GameObject mine)
        {
            Debug.Log("Mine Explode");
            // Remove exploded mine from the active list
            activeMines.Remove(mine);
            mine.SetActive(false);
        }
    }
}

