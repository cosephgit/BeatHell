using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    private int score;
    private bool playerAlive = true;
    private bool stageEnd = false;
    private int stageEndBars = 0;
    public bool menuOpen { get; private set; }

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

    void Start()
    {
        int stage = GameManager.instance.stage[GameManager.instance.slotActive];
        MenuClose();
        UIManager.instance.waveMarker.SetStage(stage);
        UIManager.instance.stageBox.UpdateWave(stage + 1, (stage == GameManager.instance.stageFinal));
    }

    public void AddScore(int bonus)
    {
        float scoreModified = bonus * (Global.SCOREBASE + (GameManager.instance.GetDiff() * Global.SCOREDIFF));
        scoreModified *= GameManager.instance.GetDifficultyScalar();
        score += Mathf.CeilToInt(scoreModified);
        UIManager.instance.scoreBox.UpdateScore(score);
    }

    // called when player dies
    public void PlayerDeath()
    {
        if (playerAlive && !stageEnd)
        {
            UIManager.instance.stageBox.UpdateWave(-2); // flag for stage failed
            playerAlive = false;
            MenuClose();
            StartCoroutine(PlayerDeathRoutine());
        }
    }

    // called when player dies
    public void StageComplete()
    {
        if (playerAlive && !stageEnd)
        {
            UIManager.instance.stageBox.UpdateWave(-1); // flag for stage complete
            stageEnd = true;
            MenuClose();
            StartCoroutine(PlayerWinRoutine());
        }
    }

    private void Update()
    {
        if (playerAlive && !stageEnd)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (menuOpen)
                {
                    MenuClose();
                }
                else
                {
                    MenuOpen();
                }
            }
        }
    }

    // open the in-game menu
    // if there is no UIManager.instance, then we're on the main menu and this is irrelevant
    public void MenuOpen()
    {
        if (UIManager.instance)
        {
            Time.timeScale = 0f;
            menuOpen = true;
            UIManager.instance.inGameMenu.gameObject.SetActive(true);
            UIManager.instance.inGameMenu.Open();
        }
    }

    public void MenuClose()
    {
        if (UIManager.instance)
        {
            Time.timeScale = 1f;
            menuOpen = false;
            UIManager.instance.inGameMenu.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayerDeathRoutine()
    {
        while (stageEndBars < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 0;
        UIManager.instance.stageEnd.gameObject.SetActive(true);
        UIManager.instance.stageEnd.SetStageEnd(false, GameManager.instance.GetStage(), 0);
    }

    private IEnumerator PlayerWinRoutine()
    {
        while (stageEndBars < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 0;
        UIManager.instance.stageEnd.gameObject.SetActive(true);
        UIManager.instance.stageEnd.SetStageEnd(true, GameManager.instance.GetStage(), score);
    }

    public void BarEnd()
    {
        if (!playerAlive || stageEnd)
        {
            stageEndBars++;
        }
    }
}
