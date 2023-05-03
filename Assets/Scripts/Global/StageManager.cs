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
        MenuClose();
        UIManager.instance.stageBox.UpdateWave(GameManager.instance.stage + 1);
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
            UIManager.instance.stageBox.UpdateWave(-2); // flag for stage failed
            playerAlive = false;
            MenuClose();
            GameManager.instance.ResetStage();
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
            GameManager.instance.ProgressStage();
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
        SceneManager.LoadScene(0); // back to main menu for now
    }

    private IEnumerator PlayerWinRoutine()
    {
        while (stageEndBars < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(1); // reload scene, with new scene stored in GameManager
    }

    public void BarEnd()
    {
        if (!playerAlive || stageEnd)
        {
            stageEndBars++;
        }
    }
}
