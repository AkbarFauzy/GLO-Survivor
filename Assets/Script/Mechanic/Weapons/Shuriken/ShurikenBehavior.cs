using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class ShurikenBehavior : IWeaponBehavior
    {
        private Shuriken weapon;

        private const float ORBIT_RADIUS = 3f; // Distance from the player
        private const float ORBIT_SPEED = 180f; // Speed of rotation in degrees per second
        private const int MAX_SHURIKEN_COUNT = 5; // Maximum number of shurikens
        private int shurikenCount = 1; // Number of shurikens orbiting
        private List<GameObject> activeShurikens = new List<GameObject>(); // Track active shurikens
        private Transform playerTransform; // Reference to the player

        public void Initialize<T>(T weapon) where T : Weapon
        {
            if (weapon is Shuriken shurikenScript)
            {
                this.weapon = shurikenScript;
                playerTransform = weapon.transform;
                AddShurikens(shurikenCount);
            }
        }

        public void Fire()
        {
            for (int i = 0; i < activeShurikens.Count; i++)
            {
                RotateShuriken(activeShurikens[i], i);
            }
        }

        public void LevelUp()
        {
            shurikenCount = Mathf.Min(MAX_SHURIKEN_COUNT, weapon.Level + 1); // Max 5 shurikens
            Debug.Log($"Shuriken Weapon leveled up to {weapon.Level}: Now has {shurikenCount} shurikens.");

            AddShurikens(shurikenCount);
        }

        private void AddShurikens(int count)
        {
            while (activeShurikens.Count < count)
            {
                GameObject shuriken = weapon.Pool.GetProjectile();
                if (shuriken != null)
                {
                    activeShurikens.Add(shuriken);
                    PositionShuriken(shuriken, activeShurikens.Count - 1);
                }
                else
                {
                    Debug.LogError("Failed to get shuriken from pool.");
                    break;
                }
            }
        }

        private void PositionShuriken(GameObject shuriken, int index)
        {
            float angle = (360f / shurikenCount) * index;
            Vector3 offset = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * ORBIT_RADIUS,
                Mathf.Sin(angle * Mathf.Deg2Rad) * ORBIT_RADIUS,
                0f
            );
            shuriken.transform.position = playerTransform.position + offset;
            shuriken.SetActive(true);
        }

        private void RotateShuriken(GameObject shuriken, int index)
        {
            float angle = (360f / shurikenCount) * index + Time.time * ORBIT_SPEED;
            Vector3 offset = new Vector3(
                  Mathf.Cos(angle * Mathf.Deg2Rad) * ORBIT_RADIUS,
                  Mathf.Sin(angle * Mathf.Deg2Rad) * ORBIT_RADIUS,
                  0f
              );

            shuriken.transform.position = playerTransform.position + offset;
            shuriken.transform.Rotate(Vector3.forward, ORBIT_SPEED * Time.deltaTime);
        }
    }
}

