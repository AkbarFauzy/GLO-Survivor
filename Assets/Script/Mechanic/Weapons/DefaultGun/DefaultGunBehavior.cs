using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class DefaultGunBehavior : IWeaponBehavior
    {
        private DefaultGun weapon;
        private float fireTimer = 0f;

        [SerializeField] private Transform firePoint;

        public void Initialize<T>(T weapon) where T : Weapon
        {
            if (weapon is DefaultGun defaultGun)
            {
                this.weapon = defaultGun;
            }
            else {
                // Handle the case where the weapon is not a TeslaWeapon
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
            }
        }

        public void Fire()
        {
            Transform nearestEnemy = FindNearestEnemy();
            if (!weapon.IsPoolExist || nearestEnemy == null) return;

            float fireRate = weapon.GetFireRate(); // Fire rate determines shots per second
            float interval = 1f / fireRate;
            fireTimer += Time.deltaTime;

            if (fireTimer >= interval)
            {
                fireTimer -= interval;
                GameObject projectile = weapon.Pool.GetProjectile();
                if (projectile != null)
                {
                    projectile.transform.position = weapon.transform.position;
                    projectile.SetActive(true);

                    // Apply an initial velocity or behavior
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 direction = (nearestEnemy.position - weapon.transform.position).normalized;
                        rb.velocity = direction * 20f;
                    }
                }
            }
        }
        public void LevelUp(int newLevel)
        {
            Debug.Log($"Weapon leveled up to {newLevel}: Adjusting ProjectileBehavior.");
        }

        private Transform FindNearestEnemy()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(weapon.transform.position, weapon.GetRange(), LayerMask.GetMask("Enemy"));
            Transform nearestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var hit in hits)
            {
                float distance = Vector2.Distance(weapon.transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = hit.transform;
                }
            }

            return nearestEnemy;
        }
    }
}
