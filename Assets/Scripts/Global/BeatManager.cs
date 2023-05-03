using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the beat manager
// the heart and soul of the game
// this manages the timing of EVERYTHING
// the timing of music, events, fire rates, spawns, and everything else are based on the count in this class
// singleton

public class BeatManager : MonoBehaviour
{
    public static BeatManager instance;
    // the timing events for the game
    public delegate void BeatFrac(int count);
    public static event BeatFrac onBeatFrac;
    public delegate void Beat(int count);
    public static event Beat onBeat;
    public delegate void Bar();
    public static event Bar onBar;
    [SerializeField]private int startBPM = 90;
    [SerializeField]private int beatsPerBar = 4; // number of beats per bar
    [SerializeField]private int beatFracPerBeat = 2; // number of fractions that each beat is split into
    public static int beatFracFrames { get; private set; } = 10; // the number of fixed update frames per fraction of a beat
    private int frameCount = 0;
    private int beatFraction = 0;
    private int beat = 0;
    public static float bpm { get; private set; }

    private void Awake()
    {
        if (instance)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            instance = this;

            StartCoroutine(LateFixedUpdate());

            #if UNITY_EDITOR
            //onBeatFrac += BeatFractionHappens;
            //onBeat += BeatHappens;
            //onBar += BarHappens;
            #endif

            BPMToBeatFrames(startBPM);
            Debug.Log("actual bpm is " + bpm);
        }
    }

    // we want to make ABSOLUTELY SURE that the count always happens at the same point in the frame, in this case it will always happen after everything else
    private IEnumerator LateFixedUpdate()
    {
        yield return new WaitForSeconds(0.5f); // take a little time before starting the beat
        while (true)
        {
            yield return new WaitForFixedUpdate();
            frameCount++;
            if (frameCount == beatFracFrames)
            {
                frameCount = 0;
                beatFraction++;

                // we've reached the end of a beat fraction
                if (onBeatFrac != null)
                    onBeatFrac(beatFraction); // call events

                if (beatFraction == beatFracPerBeat)
                {
                    beatFraction = 0;
                    beat++;

                    // beat ends
                    if (onBeat != null)
                        onBeat(beat); // call events

                    if (beat == beatsPerBar)
                    {
                        // bar ends
                        if (onBar != null)
                            onBar(); // call events

                        beat = 0;
                    }
                }
            }
        }
    }

    // work out the frames per beat from the BPM, then validate the BPM value
    // this rounds the BPM to the nearest BPM that is acceptable (i.e. that has a suitable corresponding integer beatFrames value)
    private void BPMToBeatFrames(int newBPM)
    {
        int framesPerMinute = Mathf.CeilToInt(60f / Time.fixedDeltaTime);
        beatFracFrames = Mathf.CeilToInt((float)framesPerMinute / (float)newBPM / (float)beatFracPerBeat); // work out how many frames occcur between each beat fraction
        bpm = (float)framesPerMinute / (float)beatFracFrames / (float)beatFracPerBeat;
        // e.g. 90 bpm, 0.02f frame time
        // framesPerMinute = 3000
        // beatFractions = 2
        // beatFracFrames = 3000 / 90 / 2 = 16.66666 rounded to 17
        // bpm = 3000 / 17 / 2 = 88.235...
    }

    #if UNITY_EDITOR
    // TESTING FUNCTIONS
    // count is the number of beat fractions so far in this beat
    void BeatFractionHappens(int count)
    {
        Debug.Log("half-beat " + count);
    }
    // count is the number of beats so far in this bar
    void BeatHappens(int count)
    {
        Debug.Log("beat " + count);
    }
    void BarHappens()
    {
        Debug.Log("bar");
    }
    #endif
}
