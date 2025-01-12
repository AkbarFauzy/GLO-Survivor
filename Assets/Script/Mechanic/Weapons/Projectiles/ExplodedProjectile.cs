using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class ExplodedProjectile : Projectile
    {
        [Header("Explosion Settings")]
        private float explosionRadius; // Radius of explosion
        private float explosionDamage; // Damage dealt by explosion
        [SerializeField] private float knockbackForce = 5f; // Force applied to enemies during explosion
        [SerializeField] private GameObject explosionEffectPrefab; // Visual effect for explosion
        private float explosionDelay = 3f; // Time before explosion
        protected float explosionEffectDuration = 2f; // Time explosion effect stays active

        [Header("Audio Settings")]
        [SerializeField] private AudioClip throwSound; // Sound for grenade throw
        [SerializeField] private AudioClip explosionSound; // Sound for explosion
        [SerializeField] private AudioSource audioSource; // Audio source for playing sounds

        private float timer; // Tracks time since activation
        private bool hasExploded = false; // Prevent multiple explosions
        protected GameObject instantiatedExplosionEffect;
        protected float effectTimer;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            if (explosionEffectPrefab != null)
            {
                instantiatedExplosionEffect = Instantiate(explosionEffectPrefab);
                instantiatedExplosionEffect.SetActive(false);
            }
        }

        public virtual void Init(float explosionRadius, float explosionDamage)
        {
            // Initialize grenade state for reuse
            this.explosionRadius = explosionRadius;
            this.explosionDamage = explosionDamage;

            timer = 0f;
            hasExploded = false;
            gameObject.SetActive(true);

            // Play throw sound
            PlaySound(throwSound);
        }

        protected override void Update()
        {
            // Update the timer
            timer += Time.deltaTime;

            // Check for explosion
            if (timer >= explosionDelay && !hasExploded)
            {
                Explode();
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            // Handle collision logic here
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Explode();
            }
        }

        protected virtual void Explode()
        {
            if (hasExploded) return;

            Debug.Log("Exploded");
            hasExploded = true;

            // Activate explosion effect
            if (instantiatedExplosionEffect != null)
            {
                instantiatedExplosionEffect.transform.position = transform.position;
                instantiatedExplosionEffect.SetActive(true);
                effectTimer = 0f;
            }

            // Play explosion sound
            PlaySound(explosionSound);

            // Apply damage and knockback to nearby enemies
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Deal damage
                    var enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(explosionDamage);
                    }

                    // Apply knockback
                    Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
                        enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }

            gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            // Visualize the explosion radius in the editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        protected virtual void OnDisable()
        {
            // Reset explosion state for pooling reuse
            rb.velocity = Vector2.zero; // Stop any remaining movement
            timer = 0f; // Reset the timer
            hasExploded = false;
        }

        private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}

