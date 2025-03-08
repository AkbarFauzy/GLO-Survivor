using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class GrenadesBehaviour : IWeaponBehavior
    {
        private Grenades weapon;
        private int burstCount = 1;

        private float baseThrowForce = 1f;
        private float arcHeight = 2f;
        private float randomAngleRange = 15f;
        private float cooldownInterval = 3f;

        private float burstInterval = 0.2f;
        private float burstTimer = 0f;
        private float cooldownTimer = 0f;
        private int grenadesThrown = 0;
        private bool isBursting = false;
        private bool isCooldown = false;

        public void Initialize<T>(T weapon) where T : Weapon
        {
            if (weapon is Grenades grenades)
            {
                this.weapon = grenades;
            }
            else
            {
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
            }
        }

        public void Fire()
        {
            if (isCooldown)
            {
                HandleCooldown();
                return;
            }

            if (!isBursting)
            {
                StartBurst();
            }

            burstTimer += Time.deltaTime;

            if (burstTimer >= burstInterval && grenadesThrown < burstCount)
            {
                burstTimer = 0f;
                ThrowNextGrenade();
            }
        }

        public void LevelUp()
        {
            burstCount = Mathf.Min(3, weapon.Level); // Cap burst count at 3 grenades
            Debug.Log($"Grenade Weapon leveled up to {weapon.Level}: Now throws {burstCount} grenade(s) per burst.");
        }

        private void HandleCooldown()
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownInterval)
            {
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }

        private void StartBurst()
        {
            isBursting = true;
            grenadesThrown = 0;
            burstTimer = 0f;
        }

        private void ThrowNextGrenade()
        {
            GameObject grenade = weapon.Pool.GetProjectile();

            if (grenade != null)
            {
                Vector2 randomDirection = GetRandomThrowDirection();
                ThrowGrenade(grenade, randomDirection);
                grenadesThrown++;
            }

            if (grenadesThrown >= burstCount)
            {
                isBursting = false;
                isCooldown = true;
            }
        }

        private void ThrowGrenade(GameObject grenade, Vector3 direction)
        {
            grenade.SetActive(true);
            grenade.GetComponent<ExplodedProjectile>().Init(weapon.GetRange(), weapon.GetDamage());
            grenade.transform.position = weapon.transform.position;
            Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = direction * weapon.GetRange();
            }
        }

        private Vector2 GetRandomThrowDirection()
        {
            float horizontalDirection = Random.Range(0, 2) == 0 ? -1f : 1f;
            float randomAngle = Random.Range(-randomAngleRange, randomAngleRange);
            Vector2 baseDirection = new Vector2(horizontalDirection, arcHeight).normalized;

            return Quaternion.Euler(0, 0, randomAngle) * baseDirection * baseThrowForce;
        }
    }
}

