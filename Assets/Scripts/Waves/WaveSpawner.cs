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
    [SerializeField]private int waveBarPause = 4; // number of bars that play between each wave
    [SerializeField]private int stageBarStart = 1; // number of bars before the first spawn
    [SerializeField]private int stageBarTarget = 48; // the approximate target number of bars to play for this stage
    [SerializeField]private float intensityHighBias = 1.5f;
    // to comment this effectively I would need to attach an annotated spreadsheet, so don't mess with it
    // short story is it is the ideal adjusted intensity deficit in a stage, and since you don't understand what that means: don't mess with it
    private const float BASELINE = 29.94f;
    // this is much simpler - target wave size of 8 with a weak enemy at 1st stage max intensity (with a bit of buffer so it's still possible with imperfect timing)
    private const float WAVESTRENGTHBASE = 6f;
    private float[] barIntensity;

    /*
    How random numbers work in this script.
    Each stage has a stageSeed
    Before spawning each wave, the random seed is set with stageSeed + waveCountSpawned
    The reason for this is to allow Random.Range calls outside of this script to be made (for cosmetic reasons) but for THIS script to have level generation
    that ALWAYS has the same (randomly determined) result for any given stageSeedBase.

    An alternative approach for this is to pregenerate all the wave indices at the start of the stage during Start
    */
    [SerializeField]private int stageSeed = 0; // the original random seed to be used for this stage
    [SerializeField]private int waveMin = 6;
    [SerializeField]private int waveMax = 12;
    private int barCounter = 0;
    private int waveCountSpawned = 0;

    private int barCurrent = 0;
    private float intensityAccumulator = 0f;

    // creates a stage intensity graph
    private void IntensityGenerator()
    {
        // new stage intensity graph calculation
        Vector2[] intensitySteps; // each intensity step has two values: the intensity (float 0...1) and the bar (int 0...)

        Random.InitState(stageSeed);
        int waveCount = Random.Range(3, 6);
        float intensityLast = 0f;
        float waveBarAverage = (float)stageBarTarget / (float)waveCount; // the average number of bars per wave
        int stageBarsToAssign = stageBarTarget; // the number of bars of the stage which are not yet assigned to a wave
        intensitySteps = new Vector2[waveCount * 2]; // each intensity peak needs an initial value and a final to get the right intensity flow
        for (int i = waveCount - 1; i >= 0; i--)
        {
            float intensityThis;
            if (intensityLast > 0f)
            {
                float intensityDrop = ((float)Random.Range(7, 11)) * 0.1f; //intensity is multiplied by between 0.6 and 1.0 with each wave (preceding wave in play order)
                //Debug.Log("wave " + i + " intensityDrop = " + intensityDrop);
                intensityThis = intensityLast * intensityDrop;
            }
            else
                intensityThis = 1f; // the last wave (first in the loop) is always max intensity
            intensitySteps[i * 2] = new Vector2(0.2f, 0); // always drop to 0.2f intensity after each peak
            intensitySteps[(i * 2) + 1] = new Vector2(intensityThis, 0);
            //Debug.Log("wave " + i + " intensity = " + intensitySteps[(i * 2) + 1].x);
            intensityLast = intensityThis;
        }
        //Debug.Log("stageBarsToAssign left " + stageBarsToAssign);
        // all peak intensities have been set, now we need to set the timing to work correctly
        for (int i = 0; i < waveCount; i++)
        {
            int stepBars;
            if (i == 0)
                intensitySteps[i * 2].y = 0;
            else
                intensitySteps[i * 2].y = intensitySteps[i * 2 - 1].y + 1;

            if (i == waveCount - 1) // last intensity peak, assign all remaining bars
                stepBars = stageBarsToAssign;
            else
                stepBars = Mathf.CeilToInt((Random.Range(0.8f, 1.2f) * stageBarsToAssign / (waveCount - i)));

            intensitySteps[(i * 2) + 1].y = intensitySteps[i * 2].y + stepBars;
            stageBarsToAssign -= stepBars;
            //Debug.Log("intensity for wave " + i + " is " + intensitySteps[(i * 2) + 1].x + " starting on bar " + intensitySteps[i * 2].y + " ending on bar " + intensitySteps[(i * 2) + 1].y);
            //Debug.Log("stageBarsToAssign left " + stageBarsToAssign);
        }

        // now we've assigned the intensity and timing of the intensity peaks
        // but because these have been placed with random intensity drops and spacing, the overall stage difficulty might be right off
        // so it needs to be rebalanced
        int barLast = (int)intensitySteps[intensitySteps.Length - 1].y + 1;
        float[] intensityDeficitWork = new float[barLast]; // the values for each bar of how much intensity there ISN'T
        float intensityDeficit = 0f;
        int intensityStepCurrent = 0;
        for (int i = 0; i < barLast; i++)
        {
            if (intensityStepCurrent < intensitySteps.Length - 1)
            {
                // check if the current bar i is part of the next intensity step
                if (i >= intensitySteps[intensityStepCurrent + 1].y)
                {
                    // the bar is in the next step
                    intensityStepCurrent++;
                }
            }
            // the intensity for this bar
            float barIntensity = intensitySteps[intensityStepCurrent].x; // TODO finish this, just testing
            float barsExtra = i - intensitySteps[intensityStepCurrent].y; // the number of extra bars complete since the start of the intensity step
            if (barsExtra > 0)
            {
                float lerp = barsExtra / (intensitySteps[intensityStepCurrent + 1].y - intensitySteps[intensityStepCurrent].y);
                barIntensity = Mathf.Lerp(intensitySteps[intensityStepCurrent].x, intensitySteps[intensityStepCurrent + 1].x, lerp);
                //Debug.Log("lerping: " + lerp);
            }
            // need to lerp between the start value of this step and the start value of the next step
            //Debug.Log("intensityStepCurrent = " + intensityStepCurrent + " barIntensity = " + barIntensity);

            // this calculation takes the magnitude of the REDUCED intensity (the amount less than 0) and applies a power to it
            // the point of this is that it values low intensity even lower - it's very easy to manage a constant low intensity compared to spikes of high intensity
            intensityDeficitWork[i] = 1f - Mathf.Pow(barIntensity, intensityHighBias);
            intensityDeficit += intensityDeficitWork[i];
        }
        if (intensityDeficit > BASELINE)
        {
            //Debug.Log("total intensity deficit " + intensityDeficit + " is ABOVE BASELINE");
        }
        else if (intensityDeficit < BASELINE)
        {
            //Debug.Log("total intensity deficit " + intensityDeficit + " is BELOW BASELINE");
        }
        barIntensity = new float[barLast];
        for (int i = 0; i < barLast; i++)
        {
            float fook = intensityDeficitWork[i] * BASELINE / intensityDeficit;
            //intensityDeficitWork[i] *= BASELINE / intensityDeficit;
            intensityDeficitWork[i] = Mathf.Pow(intensityDeficitWork[i], (intensityDeficit / BASELINE));
            //Debug.Log("adjusted deficit " + i + " is " + intensityDeficitWork[i] + " old adjusted deficit: " + fook);
            barIntensity[i] = Mathf.Pow((1f - intensityDeficitWork[i]), 1f / intensityHighBias);
            if (barIntensity[i] < 0f) Debug.LogError("ALERT barIntensity[" + i + "] < 0f: " + barIntensity[i]);
            //else Debug.Log("barIntensity[" + i + "]: " + barIntensity[i]);
        }
        /*for (int i = 0; i < intensityDeficitWork.Length; i++)
        {
            intensityDeficitWork[i] *= BASELINE / intensityDeficit;
            if (intensityDeficitWork[i] >= 1f) Debug.LogError("ALERT intensityDeficitWork[" + i + "] >= 1f: " + intensityDeficitWork[i]);
            intensitySteps[i].x = (1f - intensityDeficitWork[i]);
        }*/
    }

    private void Awake()
    {
        barCounter = waveBarPause - stageBarStart;
        IntensityGenerator();
    }

    private void Start()
    {
        BeatManager.onBar += StageBar;
    }

    private void StageBar()
    {
        // at each bar, add the intensity of the current bar to the intensity accumulator
        if (barCurrent == barIntensity.Length)
        {
            StageManager.instance.StageComplete();
        }
        else if (barCurrent < barIntensity.Length) // check we're on a valid bar
        {
            float intensityThis = barIntensity[barCurrent];
            intensityAccumulator += intensityThis;

            // then if the intensityAccumulator is high enough and enough bars have passed, spawn a wave 
            if (barCounter >= waveBarPause && (intensityAccumulator > 0))
            {
                Random.InitState(stageSeed + barCurrent);
                // once the minimum number of bars between waves has passed, try to spawn a wave
                int enemyIndex = 0;
                int enemyIndexMax = -1;
                for (int i = 0; i < PrefabProvider.instance.enemyStrength.Length; i++)
                {
                    // get the strongest enemy index that can be spawned at the current intensity
                    if (PrefabProvider.instance.enemyStrength[i] < barIntensity[barCurrent])
                    {
                        enemyIndexMax = i;
                    }
                }
                // if multiple possible enemies are possible at the current intensity, randomise amongst them
                if (enemyIndexMax >= 0)
                    enemyIndex = Random.Range(0, enemyIndexMax + 1);

                // calculate the total wave strength for the current bar, and work out how many of the chosen enemies to spawn
                float waveStrength = WAVESTRENGTHBASE * barIntensity[barCurrent];
                int waveSpawnCount = Mathf.Clamp(Mathf.FloorToInt(waveStrength / PrefabProvider.instance.enemyStrength[enemyIndex]), waveMin, waveMax);

                // place the spawner and update the UI
                EnemyPawnSpawner waveSpawned = Instantiate(spawners[Random.Range(0, spawners.Length)]);

                if (waveSpawned)
                {
                    waveSpawned.StartWave(enemyIndex, waveSpawnCount);
                    Debug.Log("wave spawn triggered with enemy index " + enemyIndex + " and count " + waveSpawnCount);

                    // subtract the strength of the spawned wave from the intensity accumulated
                    intensityAccumulator -= (PrefabProvider.instance.enemyStrength[enemyIndex] * waveSpawnCount);

                    // update the UI with the spawn and increment the number of waves spawned
                    UIManager.instance.waveMarker.UpdateWave(waveCountSpawned + 1);
                    waveCountSpawned++;

                    barCounter = 0;
                }
            }
            else
            {
                barCounter++;
            }
        }
        barCurrent++;
    }

    private void OnDestroy()
    {
        BeatManager.onBar -= StageBar;
    }
}
