using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnableType {
    Enemy,
    Merchant,
    Chest
}

public class ShipSpawner : MonoBehaviour
{
    public GameObject merchantShipPrefab;
    public GameObject enemyShipPrefab;
    public GameObject chestPrefab;

    public Transform player;

    public int maxMerchants = 7;
    public int maxEnemies = 3;
    public int maxChests = 12;

    int merchantCount = 0;
    int enemyCount = 0;
    int chestCount = 0;

    public float merchantRespawnTime;
    public float enemyRespawnTime;
    public float chestRespawnTime;
    float merchantTimer = 0f;
    float enemyTimer = 0f;
    float chestTimer = 0f;

    void Start() {
        for (int i = 0; i < maxMerchants; i++) {
            SpawnMerchant();
        }
        for (int i = 0; i < maxEnemies; i++) {
            SpawnEnemy();
        }
        for (int i = 0; i < maxChests; i++) {
            SpawnChest();
        }
    }

    void Update() {
        if (merchantCount < maxMerchants) {
            merchantTimer -= Time.deltaTime;
            if (merchantTimer < 0f) {
                SpawnMerchant();
            }
        }

        if (enemyCount < maxEnemies) {
            enemyTimer -= Time.deltaTime;
            if (enemyTimer < 0f) {
                SpawnEnemy();
            }
        }

        if (chestCount < maxChests) {
            chestTimer -= Time.deltaTime;
            if (chestTimer < 0f) {
                SpawnChest();
            }
        }
    }

    public void ChestSpawnedIndependently() {
        chestCount++;
    }

    void SpawnMerchant() {
        //Debug.Log("Spawned merchant");
        SpawnShip(merchantShipPrefab, 5f);
        merchantTimer = merchantRespawnTime;
        merchantCount++;
    }

    void SpawnEnemy() {
        //Debug.Log("Spawned enemy");
        SpawnShip(enemyShipPrefab, 5f);
        enemyTimer = enemyRespawnTime;
        enemyCount++;
    }

    void SpawnChest() {
        //Debug.Log("Spawned chest");
        SpawnShip(chestPrefab, 0f);
        chestTimer = chestRespawnTime;
        chestCount++;
    }

    public void ReportDead(SpawnableType dead) {
        if (dead == SpawnableType.Merchant) {
            merchantCount--;
        }
        else if (dead == SpawnableType.Enemy) {
            enemyCount--;
        }
        else if (dead == SpawnableType.Chest) {
            chestCount--;
        }
    }

    void SpawnShip(GameObject prefab, float yPos) {
        Vector3 potentialSpawn = Vector3.up * 5f;

        do {
            potentialSpawn = new Vector3(Random.Range(-360f, 360f), yPos, Random.Range(-360f, 360f));
        } while (Vector3.Distance(potentialSpawn, player.position) < 50f);

        Instantiate(prefab, potentialSpawn, Quaternion.identity);
    }
}
