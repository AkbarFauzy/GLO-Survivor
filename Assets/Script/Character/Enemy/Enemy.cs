using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic.UI;
using UnityEngine;


namespace Survivor.Character.Enemies {
    public class Enemy : Observer
    {
        public string EnemyName;

        public EnemyData enemyData;
        public float Health { get; private set; }
        public float Speed { get; private set; }
        public float Damage { get; private set; }
        public bool IsFacingRight { get; private set; }
        public bool IsJumping { get; private set; }
        public float LastOnGroundTime { get; private set; }

        protected Transform _player;
        protected Rigidbody2D _rb;

        public float maxFallSpeed = -10f;

        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

        [SerializeField] private LayerMask _groundLayer;

        public GameObject damagePopupPrefab;
        private int popupPoolSize = 5;
        private Queue<GameObject> popupPool;

        private bool isBlinking = false;
        private float blinkTimer = 0f;
        private Color originalColor;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            IsFacingRight = true;

            InitializePopupPool();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        private void OnEnable()
        {
            Health = enemyData.Health;
            Speed = enemyData.Speed;
            Damage = enemyData.Damage;
        }

        protected virtual void Update()
        {
            if (isBlinking)
            {
                HandleBlinking();
            }

            LastOnGroundTime -= Time.deltaTime;

            if (!IsJumping)
            {
                //Ground Check
                if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
                {
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                    LastOnGroundTime = 0.5f;
                }
            }

            if (IsJumping && _rb.velocity.y < 0)
            {
                IsJumping = false;
            }

        }

        protected virtual void FixedUpdate()
        {
            if (_player == null) return;

            MoveTowardPlayer();
            LimitFallSpeed();
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;

            BlinkWhite();
            ShowDamagePopup(damage);
            if (Health <= 0)
            {
                OnDied();
            }
        }
        private void InitializePopupPool()
        {
            popupPool = new Queue<GameObject>();
            for (int i = 0; i < popupPoolSize; i++)
            {
                GameObject popup = Instantiate(damagePopupPrefab, gameObject.transform);
                popup.SetActive(false);
                popupPool.Enqueue(popup);
            }
        }

        private void ShowDamagePopup(float damage)
        {
            if (popupPool.Count > 0)
            {
                GameObject popup = popupPool.Dequeue();
                popup.transform.position = transform.position + Vector3.up;
                popup.SetActive(true);

                // Set the damage value
                PopUpDamage popupScript = popup.GetComponent<PopUpDamage>();
                if (popupScript != null)
                {
                    popupScript.Setup(damage, ReturnPopupToPool);
                }
            }
        }

        private void ReturnPopupToPool(GameObject popup)
        {
            popup.SetActive(false);
            popupPool.Enqueue(popup);
        }

        private void HandleBlinking()
        {
            blinkTimer -= Time.deltaTime;

            if (blinkTimer <= 0)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = originalColor;
                }

                isBlinking = false;
            }
        }

        private void BlinkWhite()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null && !isBlinking)
            {
                // Set the color to white and start the blinking timer
                spriteRenderer.color = Color.white;
                isBlinking = true;
                blinkTimer = 0.1f; // Duration of the blink in seconds
            }
        }

        protected virtual void MoveTowardPlayer()
        {
            Vector2 direction = (_player.position - transform.position).normalized;
            if ((direction.x > 0 && !IsFacingRight) || (direction.x < 0 && IsFacingRight))
            {
                Turn();
            }
            _rb.velocity = new Vector2(direction.x * Speed, _rb.velocity.y);
        }

        void LimitFallSpeed()
        {
            // Clamp the falling speed to prevent unrealistic behavior
            if (_rb.velocity.y < maxFallSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, maxFallSpeed);
            }
        }

        protected virtual void OnDied() {
            Debug.Log("Enemy Died");
            NotifyEvents<Enemy>(Events.EnemyDied, this);
            NotifyEvents(Events.SpawnExpOrb, (Vector2)transform.position);
        }

        public void SetPlayer(Transform player)
        {
            _player = player;
        }

        protected void Jump()
        {
            IsJumping = true;
            LastOnGroundTime = 0;

            float verticalForce = enemyData.JumpForce;

            if (_rb.velocity.y < 0)
                verticalForce -= _rb.velocity.y;

            if (_player != null)
            {
                Vector2 directionToPlayer = (_player.position - transform.position).normalized;
                Vector2 jumpForce = new Vector2(directionToPlayer.x * Speed, verticalForce);
                _rb.AddForce(jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                _rb.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            }
        }

        protected bool CanJump()
        {
            return LastOnGroundTime > 0 && !IsJumping;
        }

        protected void Turn()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        }
    }
}

