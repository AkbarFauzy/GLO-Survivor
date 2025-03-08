using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic.Weapons;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Survivor.Mechanic.UI {
    public class WeaponSlotUI : MonoBehaviour
    {
        [SerializeField] private List<Image> _slotIcon;
        [SerializeField] private List<TextMeshPro> _slotLvl;

        public void UpdateUI(int index, Weapon weapon)
        {
            _slotIcon[index].sprite = weapon.WeaponData.Icon;

            if (weapon.IsMaxLevel) {
                _slotLvl[index].text = $"Lvl.Max";
            }
            else { 
                _slotLvl[index].text = $"Lvl.{weapon.Level}"; 
            }
        }

    }

}
