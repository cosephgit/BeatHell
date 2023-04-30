using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns a wave of enemies of the specified type, with the specified delay between them, and the specified change in launch direction

public class EnemyPawnSpawner : MusicManager
{
    [Header("Spawn settings")]
    [SerializeField]private int enemySpawnStartBar = 1; // the number of bars before this spawner starts spawning
    [SerializeField]private int enemySpawnDestroyBar = 4; // how many bars before this spawner self-destructs
    [SerializeField]private int enemyBeatFracDelay = 4; // how many beat fractions between spawns
    [SerializeField]private int enemySpawnMax = 4;
    [SerializeField]private float rotationIncrement = 0f; // the degrees of rotation after each spawn
    private int beatFracCount = 0;
    private int enemySpawns = 0;
    private int barCount = 0;

    protected override void Start()
    {
        base.Start();
        playing = false; // only start playing when this spawner goes active
    }

    protected override void MusicBeatFraction(int count)
    {
        base.MusicBeatFraction(count);
        if (playing)
        {
            beatFracCount++;
            if (beatFracCount == enemyBeatFracDelay)
            {
                Instantiate(PrefabProvider.instance.enemyPrefab, transform.position, transform.rotation);
                enemySpawns++;
                if (enemySpawns < enemySpawnMax)
                {
                    transform.Rotate(0, 0, rotationIncrement);
                    beatFracCount = 0;
                    // TODO TEMP TESTING FEATURE
                    UIManager.instance.waveMarker.UpdateWave(enemySpawns);
                }
            }
        }
    }

    protected override void MusicBar()
    {
        base.MusicBar();
        barCount++;
        if (!playing)
        {
            if (barCount >= enemySpawnStartBar) playing = true;
        }
        if (barCount >= enemySpawnDestroyBar) Destroy(gameObject, 1f); // give enough time for the last beat to finish playing
    }
}
