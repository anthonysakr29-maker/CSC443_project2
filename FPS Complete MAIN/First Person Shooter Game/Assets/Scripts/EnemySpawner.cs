using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemySpawnType
{
    public string enemyName;
    public EnemyHealth prefab;
    public int unlockWave = 1;
    public float spawnWeight = 1f;
    public int prewarmCount = 10;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] private EnemySpawnType[] enemyTypes;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Settings")]
    [SerializeField] private int startingEnemies = 5;
    [SerializeField] private int enemiesAddedPerWave = 2;
    [SerializeField] private float startingSpawnInterval = 1.5f;
    [SerializeField] private float minSpawnInterval = 0.4f;
    [SerializeField] private float spawnIntervalDecrease = 0.15f;
    [SerializeField] private float timeBetweenWaves = 5f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesLeftText;

    [SerializeField] private ShopManager shopManager;

    private ObjectPool<EnemyHealth>[] pools;
    private Dictionary<EnemyHealth, ObjectPool<EnemyHealth>> activeEnemyPools = new Dictionary<EnemyHealth, ObjectPool<EnemyHealth>>();

    private int currentWave = 0;
    private int enemiesToSpawn;
    private int enemiesAlive;
    private bool waveActive;

    private bool waitingForShop;

    private void Start()
    {
        pools = new ObjectPool<EnemyHealth>[enemyTypes.Length];

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            pools[i] = new ObjectPool<EnemyHealth>(
                enemyTypes[i].prefab,
                transform,
                enemyTypes[i].prewarmCount
            );
        }

        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            currentWave++;

            enemiesToSpawn = startingEnemies + ((currentWave - 1) * enemiesAddedPerWave);
            enemiesAlive = enemiesToSpawn;
            waveActive = true;

            UpdateUI();

            float spawnInterval = Mathf.Max(
                minSpawnInterval,
                startingSpawnInterval - ((currentWave - 1) * spawnIntervalDecrease)
            );

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            while (enemiesAlive > 0)
            {
                yield return null;
            }

            waveActive = false;
            UpdateUI();

            waitingForShop = true;

            if (shopManager != null)
            {
                shopManager.OpenShop();
            }

            while (waitingForShop)
            {
                yield return null;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int enemyIndex = PickEnemyTypeIndex();
        if (enemyIndex == -1) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        EnemyHealth enemy = pools[enemyIndex].Get(point.position, point.rotation);
        activeEnemyPools[enemy] = pools[enemyIndex];

        enemy.OnDied += HandleEnemyDied;
    }

    private int PickEnemyTypeIndex()
    {
        float totalWeight = 0f;

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            if (currentWave >= enemyTypes[i].unlockWave)
            {
                totalWeight += enemyTypes[i].spawnWeight;
            }
        }

        if (totalWeight <= 0f) return -1;

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            if (currentWave < enemyTypes[i].unlockWave) continue;

            currentWeight += enemyTypes[i].spawnWeight;

            if (randomValue <= currentWeight)
            {
                return i;
            }
        }

        return 0;
    }

    private void HandleEnemyDied(EnemyHealth enemy)
    {
        enemy.OnDied -= HandleEnemyDied;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddReward(enemy.ScoreReward, enemy.MoneyReward);
        }

        enemiesAlive--;
        UpdateUI();

        if (activeEnemyPools.TryGetValue(enemy, out ObjectPool<EnemyHealth> pool))
        {
            activeEnemyPools.Remove(enemy);
            pool.Return(enemy);
        }
    }

    private void UpdateUI()
    {
        if (waveText != null)
            waveText.text = "Wave: " + currentWave;

        if (enemiesLeftText != null)
            enemiesLeftText.text = waveActive ? "Enemies: " + enemiesAlive : "Next wave soon...";
    }

    public void ContinueToNextWave()
    {
        waitingForShop = false;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}