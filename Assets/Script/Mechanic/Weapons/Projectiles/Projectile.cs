using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class Projectile : Subject
    {
        [SerializeField] protected bool IsDestroyable;

        private float _lifetime = 5f;         // Time before the projectile is deactivated
        private float _lifeTimer = 0f;
        
        private ProjectilePool _pool; 

        private void OnEnable()
        {
            _lifeTimer = 0f;
        }

        protected virtual void Update()
        {
            if (IsDestroyable) {
                _lifeTimer += Time.deltaTime;
                if (_lifeTimer >= _lifetime)
                {
                    ReturnToPool();
                }
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            // Handle collision logic here
            if (collision.gameObject.CompareTag("Enemy"))
            {
                NotifyEvents<Enemy>(Events.EnemyHit, collision.GetComponent<Enemy>());
                if (IsDestroyable)
                {
                    ReturnToPool();
                }
            }
        }

        protected void ReturnToPool()
        {
            if (_pool != null)
            {
                _pool.ReturnProjectile(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetPool(ProjectilePool pool) {
            _pool = pool;  
        }
    }
}

