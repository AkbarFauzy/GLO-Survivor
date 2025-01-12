using System.Collections;
using System.Collections.Generic;
using Survivor.Mechanic;
using UnityEngine;

namespace Survivor.Character.Enemies {

    public class EnemySpawner : Observer
    {
        public Transform Player;
        public EnemySpawnerData spawnerSettings;

        public ExpOrbPool OrbPool;

        public Vector2 SpawnPoint { get; private set; } = new Vector2();
        private List<GameObject> _enemyPool = new List<GameObject>();
        private bool isSpawning = false;
        private int spawnCount = 0;

        [Header("Spawn Points")]
        public Transform[] spawnPoints;

        private void Start()
        {
            if (spawnerSettings == null)
            {
                Debug.LogError("SpawnerSettings is not assigned!");
                return;
            }

            Init();

            if (spawnerSettings.useTimerTrigger)
            {
                StartCoroutine(StartTimerSpawn());
            }

            Subscribe<Enemy>(Events.EnemyDied, ReturnEnemyToPool);
        }

        public void Init()
        {
            for (int i = 0; i < spawnerSettings.PoolSize; i++)
            {
                GameObject newEnemyObject = Instantiate(spawnerSettings.EnemyPrefab);
                Enemy newEnemy = newEnemyObject.GetComponent<Enemy>();
                newEnemy.AddObserver(this);
                newEnemy.AddObserver(OrbPool);
                newEnemy.SetPlayer(Player);
                newEnemyObject.SetActive(false);
                _enemyPool.Add(newEnemyObject);
            }
        }
        private void ReturnEnemyToPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            spawnCount--;
        }

        private GameObject GetEnemyFromPool()
        {
            foreach (GameObject enemy in _enemyPool)
            {
                if (!enemy.activeInHierarchy)
                {
                    return enemy;
                }
            }
            return null;
        }

        private IEnumerator StartTimerSpawn()
        {
            yield return new WaitForSeconds(spawnerSettings.SpawnAt);
            if (!isSpawning)
            {
                isSpawning = true;
                StartCoroutine(SpawnWithTimer());
            }
        }

        private IEnumerator SpawnWithTimer()
        {
            float timer = 0f;

            while (spawnerSettings.SpawnDuration < 0 || timer < spawnerSettings.SpawnDuration)
            {
                if (spawnerSettings.MaxSpawns < 0 || spawnCount < spawnerSettings.MaxSpawns)
                {
                    SpawnEnemy();
                    spawnCount++;
                }

                timer += spawnerSettings.SpawnInterval;
                yield return new WaitForSeconds(spawnerSettings.SpawnInterval);
            }

            isSpawning = false;
        }

        private void SpawnEnemy()
        {
            if (spawnPoints.Length == 0)
            {
                Debug.LogWarning("No spawn points assigned.");
                return;
            }

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = GetEnemyFromPool();
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
            enemy.SetActive(true);
        }

    }
}
