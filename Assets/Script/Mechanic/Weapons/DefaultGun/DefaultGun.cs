using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class DefaultGun : Weapon, IWeapon
    {
        public override void InitializeBehavior()
        {
           _behavior = new DefaultGunBehavior();
           _behavior.Initialize(this);
        }

        private void FixedUpdate()
        {
            if (_behavior == null) return;
            
            _behavior.Fire();
        }

        private void OnDrawGizmosSelected()
        {
            if (WeaponData != null && WeaponData.levels.Length > 0)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, WeaponData.levels[Level - 1].range);
            }
        }

    }
}
