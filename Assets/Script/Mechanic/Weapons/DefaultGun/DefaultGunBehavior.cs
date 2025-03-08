using System.Collections;
using System.Collections.Generic;
using TwoBitMachines.FlareEngine;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class DefaultGunBehavior : IWeaponBehavior
    {
        private DefaultGun weapon;
        private Firearm firearm;

        [SerializeField] private Transform firePoint;

        public void Fire()
        {

        }

        public void Initialize<T>(T weapon) where T : Weapon
        {
            this.weapon = weapon as DefaultGun;
            if (this.weapon == null)
            {
                Debug.LogError($"Weapon of type {typeof(T)} is not supported for this behavior.");
                return;
            }

            firearm = this.weapon.GetComponent<Firearm>();
            if (firearm == null)
            {
                Debug.LogError("Firearm component is missing on the weapon.");
            }
        }

        public void LevelUp()
        {
            if (firearm == null) return;

            firearm.defaultProjectile.projectile.damage = weapon.GetDamage();
            firearm.defaultProjectile.projectile.fireRate = weapon.GetFireRate();
        }
    }
}
