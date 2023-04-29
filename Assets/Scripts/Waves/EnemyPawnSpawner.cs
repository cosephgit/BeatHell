using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns a wave of enemies of the specified type, with the specified delay between them, and the specified change in launch direction

public class EnemyPawnSpawner : MonoBehaviour
{
    [SerializeField]private GameObject enemyPrefab;
    [SerializeField]private int enemyBeatFracDelay = 4; // how many beat fractions between spawns
    [SerializeField]private int enemySpawnMax = 4;
    [SerializeField]private float rotationIncrement = 0f; // the degrees of rotation after each spawn
    private int beatFracCount = 0;
    private int enemySpawns = 0;

    void Start()
    {
        BeatManager.onBeatFrac += BeatFractionSpawn;
    }

    private void OnDestroy()
    {
        BeatManager.onBeatFrac -= BeatFractionSpawn;
    }

    private void BeatFractionSpawn(int count)
    {
        beatFracCount++;
        if (beatFracCount == enemyBeatFracDelay)
        {
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            enemySpawns++;
            if (enemySpawns < enemySpawnMax)
            {
                transform.Rotate(0, 0, rotationIncrement);
                beatFracCount = 0;
                // TODO TEMP TESTING FEATURE
                UIManager.instance.waveMarker.UpdateWave(enemySpawns);
            }
            else
                Destroy(gameObject);
        }
    }
}
