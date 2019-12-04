using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float spawnRate = 0.5f;
    [SerializeField] float spawnRandFactor = 0.3f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int numOfEnemies = 5;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }

    public float GetSpawnRate() { return spawnRate; }

    public float GetSpawnRandFactor() { return spawnRandFactor; }

    public float GetMoveSpeed() { return moveSpeed; }

    public int GetNumOfEnemies() { return numOfEnemies; }

    // Get waypoints from pathPrefab
    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();

        foreach (Transform waypoint in pathPrefab.transform)
        {
            waveWaypoints.Add(waypoint);
        }

        return waveWaypoints;
    }
}
