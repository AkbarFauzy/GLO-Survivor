using Survivor.Character.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Loot
{
    public class LootManager : Observer
    {
        [SerializeField] private GameObject lootPrefab;
        [SerializeField] private int poolSize = 10;
        private Queue<GameObject> lootPool;

        private void Awake()
        {
            InitializeLootPool();
            Subscribe<Vector2>(Events.EnemyDied, SpawnLoot);
        }

        private void InitializeLootPool()
        {
            lootPool = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject loot = Instantiate(lootPrefab);
                loot.SetActive(false);
                lootPool.Enqueue(loot);
            }
        }

        private void SpawnLoot(Vector2 position)
        {
            if (lootPool.Count > 0)
            {
                GameObject loot = lootPool.Dequeue();
                loot.transform.position = position;
                loot.SetActive(true);
            }
        }

        public void ReturnLootToPool(GameObject loot)
        {
            loot.SetActive(false);
            lootPool.Enqueue(loot);
        }
    }
}
