using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class Weapon : Observer, IWeapon
    {
        public WeaponData WeaponData;
        public int Level { get; private set; }
        protected IWeaponBehavior _behavior;
        [SerializeField] protected LayerMask enemyLayer;

        [SerializeField] private bool onFixedUpdate;
        public ProjectilePool Pool { get; private set; }

        public bool IsPoolExist { get => Pool != null; }

        public bool IsMaxLevel { get => WeaponData.levels.Length == Level; }

        public virtual void InitializeBehavior() {
            Debug.LogError("There is no Behavior Implemented");  
        }

        private void Start()
        {
            Level = 1;

            // Initialize the weapon behavior based on the weapon data
            if (WeaponData != null)
            {
                if (WeaponData.hasProjectile)
                {
                    // Initialize a pooling system for projectiles
                    GameObject poolObject = new GameObject($"{WeaponData.weaponName}_Pool");
                    Pool = poolObject.AddComponent<ProjectilePool>();
                    Pool.Initialize(this, WeaponData.projectilePrefab, 10); // Pool size of 10
                    Subscribe<Enemy>(Events.EnemyHit, OnEnemyHit);
                }

                InitializeBehavior();
            }
        }

        private void Update()
        {
            if (!onFixedUpdate) return;

            _behavior.Fire();
        }

        private void FixedUpdate()
        {
            if (onFixedUpdate) return;

            _behavior.Fire();
        }

        public void LevelUp()
        {
            if (Level < WeaponData.levels.Length)
            {
                Level++;
                if (_behavior != null) { 
                    _behavior.LevelUp(Level);
                }
            }
        }

        public float GetDamage()
        {
            return WeaponData.levels[Level - 1].damage;
        }
        public float GetFireRate()
        {
            return WeaponData.levels[Level - 1].fireRate;
        }
        public float GetRange()
        {
            return WeaponData.levels[Level-1].range;
        }

        public string GetNextLevelDescription()
        {
            return WeaponData.levels[Level].description;
        }

        private void OnEnemyHit(Enemy enemy) {
            Debug.Log($"{WeaponData.name} Hit {enemy} for {GetDamage()} damage");
            enemy.TakeDamage(GetDamage());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, GetRange());
        }

    }
}

