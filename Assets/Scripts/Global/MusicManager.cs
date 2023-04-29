using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages playing music, registering it's events to the BeatManager during Start

// TODO work out how to store the music data, which instruments, how to write the music into the game, etc

public class MusicManager : MonoBehaviour
{
    // these are the instrument sounds to be played
    [SerializeField]private AudioSource instClap;
    [SerializeField]private AudioSource instHiHat;
    [SerializeField]private AudioSource instKick;
    [SerializeField]private AudioSource instSnare;

    private void Start()
    {
        BeatManager.onBeatFrac += MusicBeatFraction;
        BeatManager.onBeat += MusicBeat;
        BeatManager.onBar += MusicBar;
    }

    // make sure to register the events
    private void OnDestroy()
    {
        BeatManager.onBeatFrac -= MusicBeatFraction;
        BeatManager.onBeat -= MusicBeat;
        BeatManager.onBar -= MusicBar;
    }

    private void MusicBeatFraction(int count)
    {

    }

    private void MusicBeat(int count)
    {
        instKick.Play();
    }

    private void MusicBar()
    {
        instSnare.Play();
    }
}
