using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class ShurikenBehavior : IWeaponBehavior
    {
        private Shuriken weapon;

        private float orbitRadius = 3f; // Distance from the player
        private float orbitSpeed = 180f; // Speed of rotation in degrees per second
        private int shurikenCount = 1; // Number of shurikens orbiting
        private List<GameObject> activeShurikens = new List<GameObject>(); // Track active shurikens
        private Transform playerTransform; // Reference to the player

        public void Initialize<T>(T weapon) where T : Weapon
        {
            if (weapon is Shuriken shurikenScript)
            {
                this.weapon = shurikenScript;
                playerTransform = weapon.transform;
                                                  
                for (int i = 0; i < shurikenCount; i++)
                {
                    GameObject shuriken = weapon.Pool.GetProjectile();
                    if (shuriken != null)
                    {
                        activeShurikens.Add(shuriken);
                        PositionShuriken(shuriken, i);
                    }
                }
            }
        }

        public void Fire()
        {
            // Continuously rotate the shurikens around the player
            for (int i = 0; i < activeShurikens.Count; i++)
            {
                RotateShuriken(activeShurikens[i], i);
            }
        }

        public void LevelUp(int newLevel)
        {
            shurikenCount = Mathf.Min(5, newLevel + 1); // Max 5 shurikens
            Debug.Log($"Shuriken Weapon leveled up to {newLevel}: Now has {shurikenCount} shurikens.");

            // Add new shurikens if needed
            while (activeShurikens.Count < shurikenCount)
            {
                GameObject shuriken = weapon.Pool.GetProjectile();
                if (shuriken != null)
                {
                    activeShurikens.Add(shuriken);
                    PositionShuriken(shuriken, activeShurikens.Count - 1);
                }
            }
        }

        private void PositionShuriken(GameObject shuriken, int index)
        {
            // Position the shuriken evenly spaced around the player
            float angle = (360f / shurikenCount) * index;
            Vector3 offset = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius,
                0f
            );
            shuriken.transform.position = playerTransform.position + offset;
            shuriken.SetActive(true);
        }

        private void RotateShuriken(GameObject shuriken, int index)
        {
            // Calculate the new angle based on orbit speed
            float angle = (360f / shurikenCount) * index + Time.time * orbitSpeed;
            Vector3 offset = new Vector3(
                  Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius,
                  Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius,
                  0f
              );

            // Update the position to maintain orbit
            shuriken.transform.position = playerTransform.position + offset;

            // Rotate the shuriken visually (optional)
            shuriken.transform.Rotate(Vector3.forward, orbitSpeed * Time.deltaTime);
        }
    }
}

