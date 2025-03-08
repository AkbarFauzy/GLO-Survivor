using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class ForceFieldBehavior : IWeaponBehavior
    {
        private ForceField weapon;
        private float pulseForce;
        private float nextPulseTime;
        private Transform weaponTransform;

        public void Initialize<T>(T weapon) where T : Weapon
        {
            if (weapon is ForceField forceField)
            {
                this.weapon = forceField;
                this.weaponTransform = weapon.gameObject.GetComponentInChildren<Transform>();
                nextPulseTime = Time.time + weapon.GetFireRate();
            }
            else
            {
                // Handle the case where the weapon is not a ForceField
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
            }
        }

        public void Fire()
        {
            if (Time.time >= nextPulseTime)
            {
                EmitPulse();
                nextPulseTime = Time.time + weapon.GetFireRate();
            }
        }

        public void LevelUp()
        {
            if (weaponTransform != null)
            {
                weaponTransform.localScale = new Vector3(weapon.GetRange(), weapon.GetRange(), 1f);
            }
            Debug.Log($"Weapon leveled up to {weapon.Level}: Adjusting ProjectileBehavior. " +
                $"Current Damage: {weapon.GetDamage()}" +
                $"Current Fire Rate: {weapon.GetFireRate()}" +
                $"Current Range: {weapon.GetRange()}");
        }

        private void EmitPulse()
        {
            // Get all objects in the pulse radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(weapon.transform.position, weapon.GetRange());

            foreach (Collider2D collider in colliders)
            {
                // Check if it's an enemy
                if (collider.CompareTag("Enemy"))
                {
                    // Apply damage
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Debug.Log("Enemy Take Damage From Pulse");
                        enemy.TakeDamage(weapon.GetDamage());
                    }

                    // Apply force
                    Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 pushDirection = (collider.transform.position - weapon.transform.position).normalized;
                        rb.AddForce(pushDirection * pulseForce, ForceMode2D.Impulse);
                    }
                }
            }

            // Optional: Visualize the pulse effect
            Debug.Log("Pulse emitted!");
        }

        public void SetPulseForce(float force)
        {
            pulseForce = force;
        }
    }
}
