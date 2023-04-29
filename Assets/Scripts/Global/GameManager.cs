using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int score;
    private bool playerAlive = true;
    private int playerDeadBars = 0;

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
        playerDeadBars = 0;
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
        if (playerAlive)
        {
            playerAlive = false;
            StartCoroutine(PlayerDeathRoutine());
        }
    }

    private IEnumerator PlayerDeathRoutine()
    {
        while (playerDeadBars < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(0); // back to main menu for now
    }

    public void BarEnd()
    {
        if (!playerAlive)
        {
            playerDeadBars++;
        }
    }
}
