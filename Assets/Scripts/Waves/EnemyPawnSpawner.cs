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
    [SerializeField]private float rotationMax = 90f; // the degrees of rotation maximum
    [SerializeField]private BaseStrategy spawnStrategy; // the prefab strategy that the spawns should start with
    [SerializeField]private int spawnStrength;
    [SerializeField]private bool menuMusicLoop = false; // allows looping the music on the main menu
    private int beatFracCountSpawn = 0; // independent counter for spawning enemies
    private int enemySpawns = 0;
    private int barCount = 0;
    private float rotationStep; // the step
    private float weaponKey;

    protected override void Start()
    {
        base.Start();
    }

    public void StartWave(int enemyIndex, int spawnCount, float weaponKeyValue)
    {
        enemySpawnMax = spawnCount;
        spawnStrength = enemyIndex;
        rotationStep = rotationMax / (float)(spawnCount - 1);
        playing = false; // only start playing when this spawner goes active
        weaponKey = weaponKeyValue;
    }

    protected override void MusicBeatFraction(int count)
    {
        base.MusicBeatFraction(count);
        if (playing)
        {
            beatFracCountSpawn++;
            if (beatFracCountSpawn == enemyBeatFracDelay)
            {
                GameObject enemySpawn = Instantiate(PrefabProvider.instance.enemyPrefab[spawnStrength], transform.position, transform.rotation);
                EnemyMovement enemySpawnMove = enemySpawn.GetComponent<EnemyMovement>();
                if (enemySpawnMove)
                {
                    enemySpawnMove.SetStrategy(spawnStrategy);
                }
                EnemyShooting enemySpawnShoot = enemySpawn.GetComponent<EnemyShooting>();
                if (enemySpawnShoot)
                {
                    enemySpawnShoot.SetWeapon(weaponKey);
                }

                enemySpawns++;
                if (enemySpawns < enemySpawnMax)
                {
                    transform.Rotate(0, 0, rotationStep);
                    beatFracCountSpawn = 0;
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
        if (barCount >= enemySpawnDestroyBar && !menuMusicLoop) Destroy(gameObject, 1f); // give enough time for the last beat to finish playing
    }
}
