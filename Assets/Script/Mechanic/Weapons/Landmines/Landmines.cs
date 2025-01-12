using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class Landmines : Weapon, IWeapon
    {
        public override void InitializeBehavior()
        {
            _behavior = new LandminesBehavior();
            _behavior.Initialize(this);
        }
    }
}

