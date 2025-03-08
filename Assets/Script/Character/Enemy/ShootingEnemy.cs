using TwoBitMachines.FlareEngine;
using TwoBitMachines.FlareEngine.AI;
using UnityEngine;

namespace Survivor.Character.Enemies {

    public class ShootingEnemy : Enemy
    {
        public GameObject projectilePrefab;
        private GameObject _projectileInstance;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _projectileInstance = Instantiate(projectilePrefab);
            ProjectileBase projectileBase = _projectileInstance.GetComponent<ProjectileBase>();
            projectileBase.damage = enemyData.Damage;
            GetComponent<ShootProjectileAtTarget>().projectile = projectileBase;
        }

        /*        protected override void Update()
                {
                    if (_player == null) return;

                    float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

                    if (distanceToPlayer <= detectionRadius)
                    {
                        if (distanceToPlayer > shootingRadius)
                        {
                         *//*   MoveTowardPlayer();*//*
                        }
                        else
                        {
                            HandleShooting();
                        }
                    }
                }

        *//*        protected override void MoveTowardPlayer()
                {
                    if (_player == null) return;

                    Vector2 direction = (_player.position - transform.position).normalized;
                    // Move only horizontally
                    _rb.velocity = new Vector2(direction.x * Speed, _rb.velocity.y);
                }*//*

                private void HandleShooting()
                {
                    // Check if the projectile is inactive and ready to be reused
                    if (_projectileInstance == null || !_projectileInstance.activeInHierarchy)
                    {
                        _rb.velocity = new Vector2(0, _rb.velocity.y);
                        ShootProjectile();
                    }
                }

                private void ShootProjectile()
                {
                    if (_projectileInstance == null)
                    {
                        // Instantiate the projectile for the first time
                        _projectileInstance = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
                        _projectileInstance.GetComponent<EnemyProjectile>().AddObserver(this);
                    }
                    else
                    {
                        // Reset projectile position and reactivate it
                        _projectileInstance.transform.position = shootingPoint.position;
                        _projectileInstance.transform.rotation = Quaternion.identity;
                        _projectileInstance.SetActive(true);
                    }

                    // Set projectile velocity
                    if (_player != null)
                    {
                        Vector2 direction = (_player.position - shootingPoint.position).normalized;

                        Rigidbody2D projectileRb = _projectileInstance.GetComponent<Rigidbody2D>();
                        if (projectileRb != null)
                        {
                            projectileRb.velocity = direction * projectileSpeed;
                        }
                    }
                }

                private void OnPlayerTakeDamage() {
                    if (_player != null)
                    {
                        _player.GetComponent<Player.Player>().TakeDamage(Damage);
                    }
                }

                private void OnDrawGizmosSelected()
                {
                    // Visualize detection and shooting radii in the Unity editor
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position, detectionRadius);

                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(transform.position, shootingRadius);
                }*/
    }
}
