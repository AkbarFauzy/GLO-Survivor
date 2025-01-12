using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public float spawnTime;             // Time (in seconds) when the obstacle spawns
        public bool useFixedPosition;       // Whether to use a fixed position
        public Vector3 fixedPosition;       // The fixed position for the obstacle
    }

    public GameObject obstaclePrefab;      // The obstacle prefab
    public GameObject warningPrefab;       // The warning indicator prefab
    public Transform player;               // Reference to the player's transform
    public float warningDuration = 1f;     // Duration of the warning before spawning
    public float spawnDistance = 10f;      // Distance from the player for random spawning
    public Vector2 spawnRangeX = new Vector2(-5f, 5f); // Horizontal spawn range for random spawn
    public Vector2 spawnRangeY = new Vector2(-5f, 5f); // Vertical spawn range for random spawn

    public List<SpawnData> stageSpawnData; // Predefined spawn data for the stage

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

            // Determine spawn position
            Vector3 spawnPosition;
            if (spawnData.useFixedPosition)
            {
                spawnPosition = spawnData.fixedPosition; // Use the predefined fixed position
            }
            else
            {
                // Randomize the spawn position within the range
                spawnPosition = player.position + new Vector3(
                    Random.Range(spawnRangeX.x, spawnRangeX.y),
                    Random.Range(spawnRangeY.x, spawnRangeY.y),
                    0f
                ).normalized * spawnDistance;
            }

            // Show the warning indicator
            GameObject warning = Instantiate(warningPrefab, spawnPosition, Quaternion.identity);
            Destroy(warning, warningDuration);

            // Spawn the obstacle after the warning duration
            yield return new WaitForSeconds(warningDuration);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }
}