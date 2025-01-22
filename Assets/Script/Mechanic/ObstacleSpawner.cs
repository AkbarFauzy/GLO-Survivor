using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic {
    public class ObstacleSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnData
        {
            public float spawnTime;                   // Time (in seconds) when the obstacle spawns
            public bool useFixedPosition;             // Whether to use a fixed position
            public Vector3 fixedPosition;             // The fixed position for the obstacle
            public GameObject obstaclePrefab;         // Obstacle prefab for this spawn
            public GameObject warningPrefab;          // Warning indicator prefab for this spawn
            public float warningDuration = 1f;        // Duration of the warning before spawning
        }

        public Transform player;                      // Reference to the player's transform
        public float spawnDistance = 10f;             // Distance from the player for random spawning
        public Vector2 spawnRangeX = new Vector2(-5f, 5f); // Horizontal spawn range for random spawn
        public Vector2 spawnRangeY = new Vector2(-5f, 5f); // Vertical spawn range for random spawn

        public List<SpawnData> stageSpawnData;        // Predefined spawn data for the stage

        private void Start()
        {
            StartCoroutine(SpawnObstacles());
        }

        private IEnumerator SpawnObstacles()
        {
            foreach (var spawnData in stageSpawnData)
            {
                // Wait until the spawn time
                yield return new WaitForSeconds(spawnData.spawnTime);

                // Determine warning start and end positions
                Vector3 startPosition;
                Vector3 endPosition;

                if (spawnData.useFixedPosition)
                {
                    // For fixed position, assume a fixed direction for the warning
                    startPosition = spawnData.fixedPosition + Vector3.left * spawnDistance / 2;
                    endPosition = spawnData.fixedPosition + Vector3.right * spawnDistance / 2;
                }
                else
                {
                    // For random position, calculate based on player's position and spawn range
                    Vector3 randomOffset = new Vector3(
                        Random.Range(spawnRangeX.x, spawnRangeX.y),
                        Random.Range(spawnRangeY.x, spawnRangeY.y),
                        0f
                    ).normalized * spawnDistance;

                    Vector3 warningCenter = player.position + randomOffset;

                    // Assume the warning spans horizontally (you can modify this for other orientations)
                    startPosition = warningCenter + Vector3.left * spawnDistance / 2;
                    endPosition = warningCenter + Vector3.right * spawnDistance / 2;
                }

                // Show the warning indicator
                if (spawnData.warningPrefab != null)
                {
                    GameObject warning = Instantiate(spawnData.warningPrefab, (startPosition + endPosition) / 2, Quaternion.identity);
                    warning.GetComponent<Animator>().speed = spawnData.warningDuration;

                    // Scale the warning to match the distance
                    Vector3 warningScale = warning.transform.localScale;
                    warningScale.x = Vector3.Distance(startPosition, endPosition);
                    warning.transform.localScale = warningScale;

                    Destroy(warning, spawnData.warningDuration);
                }

                // Wait for the warning duration
                yield return new WaitForSeconds(spawnData.warningDuration);

                // Spawn the obstacle at one endpoint
                if (spawnData.obstaclePrefab != null)
                {
                    GameObject obstacle = Instantiate(spawnData.obstaclePrefab, startPosition, Quaternion.identity);

                    // Move the obstacle to the other endpoint
                    ObstacleMover mover = obstacle.AddComponent<ObstacleMover>();
                    mover.Initialize(startPosition, endPosition, spawnData.warningDuration);
                }
            }
        }
    }
}
