using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject cactus;
    public int enemiesLeft = 1;

    public float spawnRange = 9;
    private int wave = 1;
    private int enemiesToSpawn = 2;
    void Start()
    {
        
    }

    void Update()
    {
        enemiesLeft = FindObjectsOfType<Enemy>().Length;
        if (enemiesLeft == 0)
        {
            wave++;
            enemiesToSpawn += wave;
            SpawnWave(enemiesToSpawn);

        }
    }

    void SpawnWave(int enemies)
    {
        for (int i = 0; i < enemies; i++) 
        {
            Instantiate(cactus, GenerateSpawnPosition(), Quaternion.identity);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosx = Random.Range(-spawnRange, spawnRange);
        float spawnPosz = Random.Range(-spawnRange - 80, spawnRange  - 80);
        Vector3 returnPos = new Vector3(spawnPosx, 0, spawnPosz);
        return returnPos;
    }
}
