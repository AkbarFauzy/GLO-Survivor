using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class TeslaBehavior : IWeaponBehavior
    {
        private TeslaWeapon weapon;
        private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
        private float chainRange;
        private int maxChains;
        private float chainDelay;

        private GameObject lightningEffectPrefab;

        public void Initialize<T>(T weapon) where T : Weapon
        {
            // Assign the generic weapon to the TeslaWeapon field, if possible
            if (weapon is TeslaWeapon teslaWeapon)
            {
                this.weapon = teslaWeapon;

                // Initialize properties from TeslaWeapon data
                chainRange = teslaWeapon.GetRange();
                maxChains = 5; 
                chainDelay = 0.2f;

                // Assign the lightning effect prefab
                lightningEffectPrefab = teslaWeapon.LightningtEffects; // Specific to TeslaWeapon
            }
            else
            {
                // Handle the case where the weapon is not a TeslaWeapon
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
            }
        }

        public void Fire()
        {
            Debug.Log("TeslaWeapon Fire");
            // Start the chain effect
            Collider2D[] colliders = Physics2D.OverlapCircleAll(weapon.transform.position, chainRange);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Trigger the chain starting from the first enemy hit
                    GameObject initialTarget = collider.gameObject;
                    weapon.StartCoroutine(ChainEffect(initialTarget, maxChains));
                    break;
                }
            }

        }

        public void LevelUp(int newLevel)
        {
            Debug.Log($"Weapon leveled up to {newLevel}: Adjusting ChainedElectricBehavior. " +
                      $"Current Damage: {weapon.GetDamage()}" +
                      $"Current Fire Rate: {weapon.GetFireRate()}" +
                      $"Current Range: {weapon.GetRange()}");
        }

        private IEnumerator ChainEffect(GameObject currentTarget, int remainingChains)
        {
            if (remainingChains <= 0 || currentTarget == null) yield break;

            // Apply damage to the current target
            ApplyDamage(currentTarget);

            // Find nearby enemies to chain to
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(currentTarget.transform.position, chainRange);
            GameObject nextTarget = null;

            foreach (var collider in nearbyColliders)
            {
                if (collider.CompareTag("Enemy") && !hitTargets.Contains(collider.gameObject))
                {
                    nextTarget = collider.gameObject;
                    break;
                }
            }

            if (nextTarget != null)
            {
                // Mark the target as hit
                hitTargets.Add(nextTarget);

                // Create the visual effect for the chain
                CreateLightningEffect(currentTarget.transform.position, nextTarget.transform.position);

                // Wait for the chain delay
                yield return new WaitForSeconds(chainDelay);

                // Chain to the next target
                weapon.StartCoroutine(ChainEffect(nextTarget, remainingChains - 1));
            }
        }

        private void ApplyDamage(GameObject target)
        {
            // Apply damage to the enemy
            var enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log($"Chained Electric Weapon hits {enemy.name} for {weapon.GetDamage()} damage.");
                enemy.TakeDamage(weapon.GetDamage());
            }
        }

        private void CreateLightningEffect(Vector3 start, Vector3 end)
        {
            if (lightningEffectPrefab != null)
            {
                GameObject effect = Object.Instantiate(lightningEffectPrefab, start, Quaternion.identity);
                var lineRenderer = effect.GetComponent<LineRenderer>();

                if (lineRenderer != null)
                {
                    lineRenderer.SetPosition(0, start);
                    lineRenderer.SetPosition(1, end);

                    // Optionally add randomness to the lightning appearance
                    AddLightningEffectRandomness(lineRenderer, start, end);
                }

                Object.Destroy(effect, 0.5f); // Clean up the effect after 0.5 seconds
            }
        }

        private void AddLightningEffectRandomness(LineRenderer lineRenderer, Vector3 start, Vector3 end)
        {
            int segmentCount = 5; // Number of segments in the lightning bolt
            lineRenderer.positionCount = segmentCount;

            for (int i = 1; i < segmentCount - 1; i++)
            {
                float t = i / (float)(segmentCount - 1);
                Vector3 position = Vector3.Lerp(start, end, t);

                // Add randomness to the position
                position += new Vector3(
                    Random.Range(-0.1f, 0.1f),
                    Random.Range(-0.1f, 0.1f),
                    Random.Range(-0.1f, 0.1f)
                );

                lineRenderer.SetPosition(i, position);
            }
        }
    }
}
