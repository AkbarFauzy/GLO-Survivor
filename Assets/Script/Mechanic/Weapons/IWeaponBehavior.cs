using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public interface IWeaponBehavior
    {
        void Initialize<T>(T weapon) where T : Weapon; // Setup for the weapon
        void Fire();
        void LevelUp(int newLevel); // Adjust behavior for new level

    }
}
