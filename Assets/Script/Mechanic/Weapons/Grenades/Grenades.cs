using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class Grenades : Weapon, IWeapon
    {
        public override void InitializeBehavior()
        {
            _behavior = new GrenadesBehaviour();
            _behavior.Initialize(this);
        }

    }
}


