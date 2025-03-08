using TwoBitMachines.FlareEngine;
using Survivor.Character.Enemies;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class Weapon : Observer, IWeapon
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private bool onFixedUpdate;

        protected IWeaponBehavior _behavior;
        private ProjectilePool _pool;

        public ProjectilePool Pool => _pool;

        public WeaponData WeaponData => weaponData; 
        public int Level { get; private set; }
        public bool IsPoolExist => _pool != null;
        public bool IsMaxLevel => weaponData.Levels.Length == Level;

        private void Start()
        {
            Level = 1;

            if (weaponData != null)
            {
                if (weaponData.ProjectilePrefab != null)
                {
                    GameObject projectilePrefab = Instantiate(weaponData.ProjectilePrefab);
                    GetComponent<Firearm>().defaultProjectile.projectile = projectilePrefab.GetComponent<TwoBitMachines.FlareEngine.Projectile>().projectile;
                }

                InitializeBehavior();
            }
        }

        public virtual void InitializeBehavior()
        {
            // Initialize weapon behavior
        }

        private void Update()
        {
            if (onFixedUpdate && Time.fixedDeltaTime == Time.deltaTime)
            {
                _behavior?.Fire();
            }
            else if (!onFixedUpdate)
            {
                _behavior?.Fire();
            }
        }

        public void LevelUp()
        {
            if (Level < weaponData.Levels.Length)
            {
                Level++;
                _behavior?.LevelUp();
            }
        }

        public float GetDamage() => weaponData.Levels[Level - 1].damage;
        public float GetFireRate() => weaponData.Levels[Level - 1].fireRate;
        public float GetRange() => weaponData.Levels[Level - 1].range;
        public string GetNextLevelDescription() => weaponData.Levels[Level].description;
    }
}
