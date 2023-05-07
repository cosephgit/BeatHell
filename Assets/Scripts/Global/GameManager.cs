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
    [SerializeField]private int[] difficultyBPM = new int[3] { 125, 150, 190 };
    [field: SerializeField]public int stageFinal { get; private set; } = 9;
    public float volume { get; private set; }
    public bool epilepsy { get; private set; }
    public int[] stage { get; private set; } = new int[Global.SAVESLOTS];
    public int[] difficulty { get; private set; } = new int[Global.SAVESLOTS];
    public int[] score { get; private set; } = new int[Global.SAVESLOTS];
    public int slotActive { get; private set; } = 0;
    public int[] scoresHigh { get; private set; } = new int[Global.SCORESLOTS];
    public int scoreFinal { get; private set; } = 0; // only used on completion of the final stage

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
        for (int i = 0; i < Global.SAVESLOTS; i++)
        {
            stage[i] = PlayerPrefs.GetInt(SlotKeySyntax(Global.SAVESTAGE, i), 0);
            difficulty[i] = PlayerPrefs.GetInt(SlotKeySyntax(Global.SAVEDIFFICULTY, i), 0);
            score[i] = PlayerPrefs.GetInt(SlotKeySyntax(Global.SAVESCORE, i), 0);
        }

        for (int i = 0; i < Global.SCORESLOTS; i++)
        {
            scoresHigh[i] = PlayerPrefs.GetInt(SlotKeySyntax(Global.SAVEHISCORE, i), 0);
        }
    }

    // this method standardises the string format used for save slots for each piece of information
    // pass the key (the specific type of information to save), the slot (which save slot 0-2)
    private string SlotKeySyntax(string key, int slot)
    {
        return (key + slot.ToString());
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

    public void FinalStageComplete(int scoreGain)
    {
        scoreFinal = score[slotActive] + scoreGain;
    }
    public void ProgressStage(int scoreGain)
    {
        if (stage[slotActive] < stageFinal)
        {
            stage[slotActive]++;
        }
        PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVESTAGE, slotActive), stage[slotActive]);
        score[slotActive] += scoreGain;
        PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVESCORE, slotActive), score[slotActive]);
    }
    public void ResetStage(int slot = -1)
    {
        int slotClear = slot;
        if (slot == -1) slotClear = slotActive;

        stage[slotClear] = 0;
        PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVESTAGE, slotClear), 0);
        difficulty[slotClear] = 0;
        PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVEDIFFICULTY, slotClear), 0);
        score[slotClear] = 0;
        PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVESCORE, slotClear), 0);
    }

    // gets the currently active stage
    public int GetStage()
    {
        return stage[slotActive];
    }

    // gets the currently active stage
    public int GetDiff()
    {
        return difficulty[slotActive];
    }

    public int GetBPM()
    {
        return difficultyBPM[difficulty[slotActive]];
    }

    public void SetActiveSlot(int slot)
    {
        slotActive = slot;
    }
    public void SetSlotDifficulty(int slot, int diff)
    {
        difficulty[slot] = diff;
        PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVEDIFFICULTY, slot), diff);
    }

    // saves the end game score when the game is finished, and returns the index of the score position (-1 means no new high score)
    public int NewFinalScore(int scoreNew)
    {
        int scoreIndex = 0;

        for (int i = 0; i < Global.SCORESLOTS; i++)
        {
            if (scoreNew <= scoresHigh[i]) scoreIndex = i + 1;
        }

        if (scoreIndex == Global.SCORESLOTS) scoreIndex = -1;
        else
        {
            // demote all the scores from this slot down by one slot
            for (int i = Global.SCORESLOTS - 1; i > scoreIndex; i--)
            {
                scoresHigh[i] = scoresHigh[i - 1];
                PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVEHISCORE, i), scoresHigh[i]);
            }
            // set the new score
            scoresHigh[scoreIndex] = scoreNew;
            PlayerPrefs.SetInt(SlotKeySyntax(Global.SAVEHISCORE, scoreIndex), scoreNew);
        }

        return scoreIndex;
    }

    public float GetDifficultyScalar()
    {
        return (Global.DIFFBASE + (Global.DIFFMULT * (float)stage[slotActive]));
    }
}
