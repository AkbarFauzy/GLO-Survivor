using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class GrenadesBehaviour : IWeaponBehavior
    {
        private Grenades weapon;
        private int burstCount = 1;

        [Header("Throw Settings")]
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
            else {
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
            }
        }

        public void Fire()
        {
            // Handle cooldown between bursts
            if (isCooldown)
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= cooldownInterval)
                {
                    isCooldown = false; // Cooldown finished, allow a new burst
                    cooldownTimer = 0f;
                }
                return;
            }

            // If not currently bursting, start a new burst
            if (!isBursting)
            {
                isBursting = true;
                grenadesThrown = 0;
                burstTimer = 0f; // Reset the burst timer
            }

            // Increment the burst timer
            burstTimer += Time.deltaTime;

            // Check if it's time to throw the next grenade in the burst
            if (burstTimer >= burstInterval && grenadesThrown < burstCount)
            {
                burstTimer = 0f; // Reset timer for the next grenade
                GameObject grenade = weapon.Pool.GetProjectile();

                if (grenade != null)
                {
                    Vector2 randomDirection = GetRandomThrowDirection();
                    ThrowGrenade(grenade, randomDirection);
                    grenadesThrown++;
                }

                // End the burst if all grenades have been thrown
                if (grenadesThrown >= burstCount)
                {
                    isBursting = false;
                    isCooldown = true; // Start cooldown after burst ends
                }
            }
        }

        public void LevelUp(int newLevel)
        {
            burstCount = Mathf.Min(3, newLevel); // Cap burst count at 3 grenades
            Debug.Log($"Grenade Weapon leveled up to {newLevel}: Now throws {burstCount} grenade(s) per burst.");
        }

        private void ThrowGrenade(GameObject grenade, Vector3 direction)
        {
            grenade.SetActive(true);
            grenade.GetComponent<ExplodedProjectile>().Init(weapon.GetRange(), weapon.GetDamage());
            grenade.transform.position = weapon.transform.position;
            Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = direction * weapon.GetRange(); // Use range as throw force
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

