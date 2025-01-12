using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using Survivor.Mechanic;
using UnityEngine.UI;
using UnityEngine;


namespace Survivor.Character.Player {
    [RequireComponent(typeof(Animator))]
    public class Player : Observer
    {
        #region VARIABLE
        public int Level;
        private float _currentExperience;
        private float _maxExp;
        private int _baseExp = 10;
        
        private Animator _anim;

        private float _currentHealth;
        private float _maxHealth = 100f;
        [SerializeField] private Slider _hpBar;

        [SerializeField] private float _magnetRadius = 10f;
        [SerializeField] private CircleCollider2D _magnetTrigger;

        [SerializeField] private float invincibilityDuration = 2.0f; // Time during which damage is ignored
        [SerializeField] private float blinkInterval = 0.1f;         // Time between blinks
        private float invincibilityTimer = 0f;                       // Tracks time remaining for invincibility
        private float blinkTimer = 0f;                               // Tracks time for blinking
        private bool isInvincible = false;                           // Flag to check if player is invincible
        private SpriteRenderer spriteRenderer;
        #endregion

        public float Experience { get => _currentExperience; }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found! Attach a SpriteRenderer to this GameObject.");
            }
        }

        void Start()
        {
            _hpBar.maxValue = _maxHealth;
            _currentHealth = _maxHealth;
            _hpBar.value = _currentHealth;

            _anim = GetComponent<Animator>();
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

            if (isInvincible)
            {
                // Manage invincibility duration
                invincibilityTimer -= Time.deltaTime;
                if (invincibilityTimer <= 0)
                {
                    isInvincible = false;
                    spriteRenderer.enabled = true; // Ensure visibility is restored
                    return;
                }

                // Manage blinking effect
                blinkTimer -= Time.deltaTime;
                if (blinkTimer <= 0)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
                    blinkTimer = blinkInterval; // Reset blink timer
                }
            }

        }

        public void TakeDamage(float damage)
        {
            if (!isInvincible)
            {
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;
                blinkTimer = 0f; // Start blinking immediately
                _currentHealth -= damage;
                _hpBar.value = _currentHealth;
                Debug.Log(_currentHealth);
                if (_currentHealth <= 0)
                {
                    NotifyEvents(Events.GameOver);
                }
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
           
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                TakeDamage(collision.gameObject.GetComponent<Enemy>().Damage);
            }
        }

    }
}

