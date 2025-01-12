/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class ProjectileBehavior : IWeaponBehavior
    {
        private Weapon weapon;
        private ProjectilePool pool;
        private float fireTimer = 0f;

        public void Initialize(Weapon weapon)
        {
            this.weapon = weapon;

            if (weapon.WeaponData.hasProjectile)
            {
                // Initialize a pooling system for projectiles
                GameObject poolObject = new GameObject($"{weapon.WeaponData.weaponName}_Pool");
                pool = poolObject.AddComponent<ProjectilePool>();
                pool.Initialize(weapon.WeaponData.projectilePrefab, 10); // Pool size of 10
            }
        }

        public void Fire()
        {
            if (pool == null) return;

            float fireRate = weapon.GetFireRate(); // Fire rate determines shots per second
            float interval = 1f / fireRate;
            fireTimer += Time.deltaTime;

            if (fireTimer >= interval)
            {
                fireTimer -= interval;
                GameObject projectile = pool.GetProjectile();
                if (projectile != null)
                {
                    projectile.transform.position = weapon.transform.position;
                    projectile.SetActive(true);

                    // Apply an initial velocity or behavior
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 direction = (fireDirection - weapon.transform.position).normalized;
                        rb.velocity = direction * 10f; // Example velocity
                    }
                }
            }
        }

        public void LevelUp(int newLevel)
        {
            Debug.Log($"Weapon leveled up to {newLevel}: Adjusting ProjectileBehavior.");
        }

    }


}
*/