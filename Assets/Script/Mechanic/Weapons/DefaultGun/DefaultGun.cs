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
    }
}
