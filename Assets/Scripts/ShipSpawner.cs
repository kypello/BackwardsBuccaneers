using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject merchantShipPrefab;
    public GameObject enemyShipPrefab;

    public Transform player;

    public int maxMerchants = 7;
    public int maxEnemies = 3;

    int merchantCount = 0;
    int enemyCount = 0;

    public float merchantRespawnTime;
    public float enemyRespawnTime;
    float merchantTimer = 0f;
    float enemyTimer = 0f;

    void Start() {
        for (int i = 0; i < maxMerchants; i++) {
            SpawnMerchant();
        }
        for (int i = 0; i < maxEnemies; i++) {
            SpawnEnemy();
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
    }

    void SpawnMerchant() {
        Debug.Log("Spawned merchant");
        SpawnShip(merchantShipPrefab);
        merchantTimer = merchantRespawnTime;
        merchantCount++;
    }

    void SpawnEnemy() {
        Debug.Log("Spawned enemy");
        SpawnShip(enemyShipPrefab);
        enemyTimer = enemyRespawnTime;
        enemyCount++;
    }

    public void ReportDeadMerchant() {
        merchantCount--;
    }

    public void ReportDeadEnemy() {
        enemyCount--;
    }

    void SpawnShip(GameObject prefab) {
        Vector3 potentialSpawn = Vector3.up * 5f;

        do {
            potentialSpawn = new Vector3(Random.Range(-360f, 360f), 5f, Random.Range(-360f, 360f));
        } while (Vector3.Distance(potentialSpawn, player.position) < 50f);

        GameObject newShip = Instantiate(prefab, potentialSpawn, Quaternion.identity);
        newShip.GetComponent<ShipControl>().spawner = this;
    }
}
