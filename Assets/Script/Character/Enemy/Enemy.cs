using System.Collections;
using System.Collections.Generic;
using TwoBitMachines.FlareEngine.AI;
using TwoBitMachines.FlareEngine.AI.BlackboardData;
using Survivor.Mechanic.UI;
using UnityEngine;
using TwoBitMachines.FlareEngine;

namespace Survivor.Character.Enemies {
    public class Enemy : Observer
    {
        public string EnemyName;

        public EnemyData enemyData;
        public float Health { get; private set; }
        public float Speed { get; private set; }
        public float Damage { get; private set; }

        protected Rigidbody2D _rb;

        public GameObject damagePopupPrefab;
        private int popupPoolSize = 5;
        private Queue<GameObject> popupPool;

        private bool isBlinking = false;
        private float blinkTimer = 0f;
        private Color originalColor;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();

            InitializePopupPool();
            InitializeSpriteRenderer();
            InitializeEnemyData();
        }

        private void OnEnable()
        {
            InitializeComponents();
        }

        protected virtual void Update()
        {
            if (isBlinking)
            {
                HandleBlinking();
            }
        }

        public void TakeDamage(float damage)
        {
            ShowDamagePopup(damage);
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

        public virtual void OnDied()
        {
            Debug.Log("Enemy Died");
            NotifyEvents<Enemy>(Events.EnemyDied, this);
            NotifyEvents(Events.EnemyDied, (Vector2)transform.position);
            NotifyEvents(Events.SpawnExpOrb, (Vector2)transform.position);
        }

        private void InitializeSpriteRenderer()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        private void InitializeEnemyData()
        {
            if (enemyData != null)
            {
                Health = enemyData.Health;
                Speed = enemyData.Speed;
                Damage = enemyData.Damage;
            }
        }

        private void InitializeComponents()
        {
            if (TryGetComponent(out TargetPathfinding targetPathfinding))
            {
                targetPathfinding.followSpeed = enemyData.Speed;
            }

            if (TryGetComponent(out TargetPathfindingBasic targetPathfindingBasic))
            {
                targetPathfindingBasic.followSpeed = enemyData.Speed;
            }

            if (TryGetComponent(out Health healthScript))
            {
                healthScript.SetValue(enemyData.Health);
            }

            if (TryGetComponent(out AIFSM ai))
            {
                ai.damage.damage = enemyData.Damage;
            }
        }
    }
}

