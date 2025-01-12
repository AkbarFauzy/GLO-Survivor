using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class Shuriken : Weapon, IWeapon
    {
        public override void InitializeBehavior()
        {
            _behavior = new ShurikenBehavior();
            _behavior.Initialize(this);
        }
    }

}
