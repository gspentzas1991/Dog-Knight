using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gravityMultiplier;
    public SpawnManager spawnManager;
    public int waveNumber = 0;
    void Start()
    {
        Physics.gravity *= gravityMultiplier;
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            AdvanceWave();
        }
    }

    private void AdvanceWave()
    {
        waveNumber++;
        spawnManager.SpawnEnemies(1);
        spawnManager.SpawnPowerups(1);
    }
}
