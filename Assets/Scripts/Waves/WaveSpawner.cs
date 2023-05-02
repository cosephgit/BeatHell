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
    /*
    How random numbers work in this script.
    Each stage has a stageSeed
    Before spawning each wave, the random seed is set with stageSeed + waveCountSpawned
    The reason for this is to allow Random.Range calls outside of this script to be made (for cosmetic reasons) but for THIS script to have level generation
    that ALWAYS has the same (randomly determined) result for any given stageSeedBase.

    An alternative approach for this is to pregenerate all the wave indices at the start of the stage during Start
    */
    [SerializeField]private int stageSeed = 0; // the original random seed to be used for this stage
    private int barCounter = 0;
    private int waveCountSpawned = 0;

    private void Awake()
    {
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
            if (waveCountSpawned < waveCount)
            {
                // to ensure that no other random calls interfere with level generation, recall and save the random seed it with each usage
                Random.InitState(stageSeed + waveCountSpawned);
                Instantiate(spawners[Random.Range(0, spawners.Length)]);
                UIManager.instance.waveMarker.UpdateWave(waveCountSpawned + 1);
            }
            else if (waveCountSpawned == waveCount)
            {
                GameManager.instance.StageComplete();
            }
            waveCountSpawned++;
        }
    }

    private void OnDestroy()
    {
        BeatManager.onBar -= StageBar;
    }
}
