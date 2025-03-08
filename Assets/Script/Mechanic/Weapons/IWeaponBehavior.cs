using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public interface IWeaponBehavior
    {
        void Initialize<T>(T weapon) where T : Weapon;
        void Fire();
        void LevelUp(); 

    }
}
