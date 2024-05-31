using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;//общее число противников в волне
        public float spawnInterval;
        public int spawnCount;// число уже появившихся противников
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; //список всех волн
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInterval;
    bool isWaveActive = false;

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount==0 && !isWaveActive)
            StartCoroutine(BeginNextWave());
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer=0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive=true;
        yield return new WaitForSeconds(waveInterval);

        if(currentWaveCount < waves.Count-1)
        {
            isWaveActive=false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota=0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            currentWaveQuota += enemyGroup.enemyCount;
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f,10f), player.transform.position.y + Random.Range(-10f,10f));
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    if(enemiesAlive>=maxEnemiesAllowed)
                    {
                        maxEnemiesReached=true;
                        return;
                    }
                }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
        if(enemiesAlive<maxEnemiesAllowed)
            maxEnemiesReached = false;
    }
}
