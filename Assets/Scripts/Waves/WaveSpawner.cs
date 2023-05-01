using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this wave spawner spawns the individual waves in a stage
// it uses random generation to spawn the waves, but from a ready-made fixed seed

public class WaveSpawner : MonoBehaviour
{
    // TEMP just for concept testing, an array of ready-made spawners
    [SerializeField]private EnemyPawnSpawner[] spawners;
    [SerializeField]private int waveCount = 4; // number of waves in the stage
    [SerializeField]private int waveBarPause = 8; // number of bars that play between each wave
    [SerializeField]private int stageBarStart = 1; // number of bars before the first spawn
    [SerializeField]private int stageSeed = 0; // the original random seed to be used for this stage
    private Random.State stageSeedState;
    private int barCounter = 0;

    private void Awake()
    {
        Random.InitState(stageSeed);
        stageSeedState = Random.state;
        barCounter = waveBarPause - stageBarStart;
    }

    private void Start()
    {
        BeatManager.onBar += StageBar;
    }

    private void StageBar()
    {
        barCounter++;
        if (barCounter >= waveBarPause)
        {
            barCounter = 0;
            // to ensure that no other random calls interfere with level generation, recall and save the random seed it with each usage
            Random.state = stageSeedState;
            Instantiate(spawners[Random.Range(0, spawners.Length)]);
            stageSeedState = Random.state;
        }
    }

    private void OnDestroy()
    {
        BeatManager.onBar -= StageBar;
    }
}
