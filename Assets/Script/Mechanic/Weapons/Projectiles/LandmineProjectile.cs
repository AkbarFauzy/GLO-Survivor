using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Survivor.Mechanic.Weapons.Projectiles {
    public class LandmineProjectile : ExplodedProjectile
    {
        [Header("Landmine Settings")]
        [SerializeField] private float armDelay = 0.5f; // Time before the landmine arms itself
        private bool isArmed = false; // Indicates whether the landmine is ready to explode

        public Action<GameObject> OnExplode;

        private void OnEnable()
        {
            // Reset landmine state
            isArmed = false;

            // Start the arming process
            Invoke(nameof(ArmMine), armDelay);
        }

        private void ArmMine()
        {
            isArmed = true;
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            // Only trigger explosion if the mine is armed and an enemy enters the trigger
            if (isArmed && collider.CompareTag("Enemy"))
            {
                Explode();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            // Manage explosion effect deactivation
            if (instantiatedExplosionEffect != null && instantiatedExplosionEffect.activeSelf)
            {
                // Notify explosion event
                OnExplode?.Invoke(gameObject);
            }
            CancelInvoke(); // Cancel any pending arming invocation
            OnExplode = null;
        }

        public override void Init(float explosionRadius, float explosionDamage)
        {
            base.Init(explosionRadius, explosionDamage);
            isArmed = false; // Reset arming state
        }
    }
}

