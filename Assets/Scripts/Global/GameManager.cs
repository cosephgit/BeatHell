using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int score;
    private bool playerAlive = true;
    private bool stageEnd = false;
    private int stageEndBars = 0;

    void Awake()
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
            instance = this;

        score = 0;
        playerAlive = true;
        stageEndBars = 0;
        stageEnd = false;
        BeatManager.onBar += BarEnd;
    }

    public void AddScore(int bonus)
    {
        score += bonus;
        UIManager.instance.scoreBox.UpdateScore(score);
    }

    // called when player dies
    public void PlayerDeath()
    {
        if (playerAlive && !stageEnd)
        {
            UIManager.instance.waveMarker.UpdateWave(-2); // flag for stage failed
            playerAlive = false;
            StartCoroutine(PlayerDeathRoutine());
        }
    }

    // called when player dies
    public void StageComplete()
    {
        if (playerAlive && !stageEnd)
        {
            UIManager.instance.waveMarker.UpdateWave(-1); // flag for stage complete
            stageEnd = true;
            StartCoroutine(PlayerWinRoutine());
        }
    }

    private IEnumerator PlayerDeathRoutine()
    {
        while (stageEndBars < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(0); // back to main menu for now
    }

    private IEnumerator PlayerWinRoutine()
    {
        while (stageEndBars < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(0); // back to main menu for now
    }

    public void BarEnd()
    {
        if (!playerAlive || stageEnd)
        {
            stageEndBars++;
        }
    }
}
