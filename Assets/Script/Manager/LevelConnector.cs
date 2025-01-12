using Survivor.Character.Player;
using Survivor.Mechanic.Weapons;
using Survivor.Mechanic.UI;
using Survivor.Manager.UI;
using UnityEngine;

namespace Survivor.Connector {
    public class LevelConnector : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerWeaponSystem _playerWeaponSystem;
        [SerializeField] private PowerUpSelector _selector;


        private void Start()
        {
            _player.AddObserver(HUDManager.Instance);
            _player.AddObserver(_selector);
            _selector.AddObserver(_playerWeaponSystem);
        }
    }
}

