using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class TeslaWeapon : Weapon, IWeapon
    {
        [Header("Effects")]
        public GameObject LightningtEffects;

        public override void InitializeBehavior()
        {
            _behavior = new TeslaBehavior();
            _behavior.Initialize(this);
        }


/*        private void OnDrawGizmosSelected()
        {
            // Visualize chain range in the editor
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chainRange);
        }*/
    }
}

