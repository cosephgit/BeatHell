using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// DontDestroyOnLoad singleton
// all overarching features should be handled through this class e.g. save/load and settings

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]private AudioMixer audioMixer;
    public float volume { get; private set; }
    public bool epilepsy { get; private set; }
    public int stage { get; private set; }

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
            DontDestroyOnLoad(gameObject);
        }

        volume = PlayerPrefs.GetFloat(Global.SAVEVOLUME, 1f);
        audioMixer.SetFloat("_volume", VolToDecibels(volume));
        epilepsy = (PlayerPrefs.GetInt(Global.SAVEEPILEPSY, 0) == 1);
        stage = PlayerPrefs.GetInt(Global.SAVEWAVE, 0);
    }

    // updates the volume both in the save settings and in the audio mixer
    public void SetVolume(float vol)
    {
        volume = vol;
        PlayerPrefs.SetFloat(Global.SAVEVOLUME, vol);
        audioMixer.SetFloat("_volume", VolToDecibels(volume));
    }

    public void SetEpilepsy(bool isOn)
    {
        epilepsy = isOn;
        PlayerPrefs.SetInt(Global.SAVEEPILEPSY, isOn ? 1 : 0);
    }

    public static float VolToDecibels(float vol)
    {
        float decibels;
        if (vol < 0.01f)
        {
            // can't do log 0
            decibels = -80f;
        }
        else
        {
            decibels = Mathf.Log(vol, 2f); // so each halving of volume is -1
            decibels *= 10f; // -10 decibels is approximately half volume
        }
        return decibels;
    }

    public void ProgressStage()
    {
        stage++;
        PlayerPrefs.SetInt(Global.SAVEWAVE, stage);
    }
    public void ResetStage()
    {
        stage = 0;
        PlayerPrefs.SetInt(Global.SAVEWAVE, stage);
    }
}
