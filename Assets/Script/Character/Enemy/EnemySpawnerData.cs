using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Character.Enemies {
    [CreateAssetMenu(fileName = "Enemy Spawn Data")]
    public class EnemySpawnerData : ScriptableObject
    {
        [Header("Spawn Settings")]
        public GameObject EnemyPrefab;
        public int PoolSize;
        public int MaxSpawns;
        public float SpawnInterval;
        public float SpawnDuration;
        public float SpawnAt;

        [Header("Trigger Settings")]
        public bool useTimerTrigger = true;    // Trigger based on time
        public bool spawnAfterBossKilled = false; // Trigger when boss is killed
/*        public float spawnDuration = 10f;*/
    }
}

