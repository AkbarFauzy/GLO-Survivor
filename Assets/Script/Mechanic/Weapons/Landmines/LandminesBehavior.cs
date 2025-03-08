using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic.Weapons.Projectiles;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class LandminesBehavior : IWeaponBehavior
    {
        private Landmines _weapon;
        private int _maxMines = 3; // Max number of active landmines
        private float _cooldownInterval = 1f; // Time between placing mines

        private float _cooldownTimer = 0f; // Timer for cooldown between placements
        private List<GameObject> _activeMines = new List<GameObject>(); // Track active mines

        /// <summary>
        /// Initializes the weapon behavior with the specified weapon.
        /// </summary>
        /// <typeparam name="T">The type of the weapon.</typeparam>
        /// <param name="weapon">The weapon to initialize.</param>
        public void Initialize<T>(T weapon) where T : Weapon
        {
            if (weapon is Landmines landmines)
            {
                _weapon = landmines;
            }
            else
            {
                // Handle the case where the weapon is not a Landmines
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
            }
        }

        /// <summary>
        /// Fires the weapon, placing a landmine if possible.
        /// </summary>
        public void Fire()
        {
            if (_weapon == null)
            {
                Debug.LogError("Weapon is not initialized.");
                return;
            }

            // Cooldown for placing a new mine
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.deltaTime;
                return;
            }

            // Limit the number of active mines
            if (_activeMines.Count >= _maxMines) return;

            // Place a new landmine
            GameObject mine = _weapon.Pool.GetProjectile();
            if (mine != null)
            {
                PlaceLandmine(mine);
                _activeMines.Add(mine);

                // Start cooldown timer
                _cooldownTimer = _cooldownInterval;
            }
        }

        /// <summary>
        /// Levels up the weapon, increasing the maximum number of active mines.
        /// </summary>
        public void LevelUp()
        {
            int newLevel = _weapon.Level;
            _maxMines = Mathf.Min(5, newLevel + 2); // Increase max mines, up to 5
            Debug.Log($"Landmine Weapon leveled up to {newLevel}: Now allows {_maxMines} active mines.");
        }

        /// <summary>
        /// Places a landmine at the player's position.
        /// </summary>
        /// <param name="mine">The mine to place.</param>
        private void PlaceLandmine(GameObject mine)
        {
            // Position mine where the player is standing
            mine.transform.position = _weapon.transform.position;
            mine.SetActive(true);

            // Configure the landmine's explosion behavior
            LandmineProjectile mineScript = mine.GetComponent<LandmineProjectile>();
            if (mineScript != null)
            {
                mineScript.Init(_weapon.GetRange(), _weapon.GetDamage());
                mineScript.OnExplode += HandleMineExplosion;
            }
        }

        /// <summary>
        /// Handles the explosion of a landmine, removing it from the active list.
        /// </summary>
        /// <param name="mine">The mine that exploded.</param>
        private void HandleMineExplosion(GameObject mine)
        {
            Debug.Log("Mine Explode");
            // Remove exploded mine from the active list
            _activeMines.Remove(mine);
            mine.SetActive(false);

            // Unsubscribe from the event to prevent memory leaks
            LandmineProjectile mineScript = mine.GetComponent<LandmineProjectile>();
            if (mineScript != null)
            {
                mineScript.OnExplode -= HandleMineExplosion;
            }
        }
    }
}

