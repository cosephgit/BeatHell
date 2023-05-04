using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages playing music, registering it's events to the BeatManager during Start
// singleton - as well as playing the standard music track, this plays extra sounds sent by enemy spawners and other events

// TODO work out how to store the music data, which instruments, how to write the music into the game, etc

public class MusicManager : MonoBehaviour
{
    // these are the instrument sounds to be played
    [Header("Music settings")]
    [SerializeField]private AudioSource player; // the sound player for this music player
    [SerializeField]private AudioSource playerLoop;
    [SerializeField]private float pulseStrength = 1f;
    [Tooltip("Index 0 should be null")]
    [SerializeField]private AudioClip[] instruments; // an array of instruments which this music player has
    [Tooltip("Index 0 is always no instrument")]
    [SerializeField]private int[] sheetMusic; // an array of instrument indices, in order of playing indexed by beat fraction
    private int beatFracCount;
    protected bool playing = true; // start playing by default

    // register beat events
    protected virtual void Start()
    {
        beatFracCount = 0;
        BeatManager.onBeatFrac += MusicBeatFraction;
        BeatManager.onBeat += MusicBeat;
        BeatManager.onBar += MusicBar;
    }

    // make sure to deregister the events
    private void OnDestroy()
    {
        BeatManager.onBeatFrac -= MusicBeatFraction;
        BeatManager.onBeat -= MusicBeat;
        BeatManager.onBar -= MusicBar;
    }

    protected virtual void MusicBeatFraction(int count)
    {
        if (playing)
        {
            if (sheetMusic[beatFracCount] > 0)
            {
                BackgroundPulse.instance.AddBeat(pulseStrength);
                player.PlayOneShot(instruments[sheetMusic[beatFracCount]]);
            }
            beatFracCount++;
            // loop
            if (beatFracCount >= sheetMusic.Length) beatFracCount = 0;
        }
    }

    protected virtual void MusicBeat(int count)
    {
    }

    protected virtual void MusicBar()
    {
    }
}
