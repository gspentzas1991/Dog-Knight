using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject powerupPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    [SerializeField] private int bossWaveFrequency = 10;

    //The horizontal range something can spawn
    private float spawnRange = 8;

    /// <summary>
    /// Spawns monsters in the level
    /// </summary>
    /// <param name="numberOfEnemies">The number of monsters to be spawned</param>
    public void SpawnEnemies(int numberOfEnemies)
    {
        for (int i = 1; i<=numberOfEnemies; i++)
        {
            if (i % bossWaveFrequency == 0)
            {
                Instantiate(bossPrefab, RandomSpawnPosition(), bossPrefab.transform.rotation);
            }
            else
            {
                var randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(randomEnemy, RandomSpawnPosition(), randomEnemy.transform.rotation);
            }
        }
    }

    /// <summary>
    /// Spawns powerups in the level
    /// </summary>
    /// <param name="numberOfPowerups">The number of powerups to be spawned</param>
    public void SpawnPowerups(int numberOfPowerups)
    {
        for (int i = 0; i < numberOfPowerups; i++)
        {
            Instantiate(powerupPrefab, RandomSpawnPosition(), powerupPrefab.transform.rotation);
        }
    }

    /// <summary>
    /// Retuns a random Vector3 that is within the spawn range
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomSpawnPosition()
    {
        return new Vector3(Random.Range(-spawnRange, spawnRange), 4.5f, -0.5f);
    }

}
