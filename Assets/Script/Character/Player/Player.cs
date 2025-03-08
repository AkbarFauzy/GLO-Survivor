using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using Survivor.Mechanic;
using UnityEngine.UI;
using UnityEngine;


namespace Survivor.Character.Player {
    public class Player : Observer
    {
        #region VARIABLE
        public int Level;
        private float _currentExperience;
        private float _maxExp;
        private int _baseExp = 10;

        private float _maxHealth = 100f;

        [SerializeField] private float _magnetRadius = 10f;
        [SerializeField] private CircleCollider2D _magnetTrigger;
        #endregion

        public float Experience { get => _currentExperience; }

        void Start()
        {
            SetMagnetRadius(_magnetRadius);
            AddObserver(this);

            Subscribe<ExpOrb>(Events.ExpOrbPickedUp , OnGainingExperience);
            
            _maxExp = _baseExp;
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentExperience >= _maxExp) {
                LevelUp();
            }
        }

        public void SetMagnetRadius(float rad)
        {
            _magnetTrigger.radius = rad;
        }

        private void LevelUp() {
            Debug.Log("Level up");
            Level += 1;
            _maxExp = GetXPForLevel();
            NotifyEvents<Player>(Events.OnPlayerLevelUp, this);
        }

        public void OnGainingExperience(ExpOrb exp) {
            _currentExperience += exp.Value;
            NotifyEvents<Player>(Events.PlayerGainingExperience, this);
        }

        private int GetXPForLevel()
        {
            return _baseExp * (Level * (Level + 1)) / 2;
        }

        public float GetCurrentLevelMaxEXP()
        {
            return _maxExp;
        }

        public void Died() {
            NotifyEvents(Events.GameOver);
        }
    }
}

