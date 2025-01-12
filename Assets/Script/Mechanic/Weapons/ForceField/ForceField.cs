using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class ForceField : Weapon, IWeapon
    {
        public override void InitializeBehavior()
        {
            _behavior = new ForceFieldBehavior();
            _behavior.Initialize(this);
        }
    }

}
