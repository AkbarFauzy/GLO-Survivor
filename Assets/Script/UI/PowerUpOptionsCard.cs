using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic.Weapons;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Survivor.Mechanic.UI
{
    public class PowerUpOptionsCard : Subject
    {
        private GameObject _currentWeapon;

        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI ItemNameTxt;
        [SerializeField] private TextMeshProUGUI ItemDescriptionTxt;

        public void SetupWeapon(GameObject weapon) {
            _currentWeapon = weapon;
            Weapon weaponScript = _currentWeapon.GetComponent<Weapon>();
            Icon.sprite = weaponScript.WeaponData.icon;
            ItemNameTxt.text = weaponScript.WeaponData.weaponName +" Lvl." + (weaponScript.Level + 1);
            ItemDescriptionTxt.text = weaponScript.GetNextLevelDescription();
        }

        public void OnClick() {
            if (_currentWeapon != null)
            {
                NotifyEvents<GameObject>(Events.PlayerGetWeapon, _currentWeapon);
                _currentWeapon = null;
            }
        }
    }
}
