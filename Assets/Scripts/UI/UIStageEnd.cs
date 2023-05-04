using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// menu element for the end of a stage (complete or failed)

public class UIStageEnd : MonoBehaviour
{
    [SerializeField]private Button buttonPlay;
    [SerializeField]private TextMeshProUGUI playText;
    [SerializeField]private TextMeshProUGUI titleText;
    [SerializeField]private TextMeshProUGUI scoreText;
    private bool save;
    private int score;

    public void SetStageEnd(bool success, int stage, int scoreNew)
    {
        save = success;
        if (success)
        {
            titleText.text = "STAGE " + (stage + 1) + " COMPLETE";
            scoreText.enabled = true;
            scoreText.text = "SCORE " + scoreNew;
            buttonPlay.interactable = true;
            playText.color = Color.white;
            score = scoreNew;
        }
        else
        {
            titleText.text = "STAGE " + (stage + 1) + " FAILED";
            scoreText.enabled = false;
            buttonPlay.interactable = false;
            playText.color = Color.gray;
            score = 0;
        }
    }

    public void ButtonNext()
    {
        Time.timeScale = 1f;
        GameManager.instance.ProgressStage(score);
        SceneManager.LoadScene(1); // reload scene, with new stage number stored in GameManager
    }

    public void ButtonRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1); // just reload the scene without saving
    }

    public void ButtonQuit()
    {
        Time.timeScale = 1f;
        if (save)
            GameManager.instance.ProgressStage(score);
        SceneManager.LoadScene(0); // return to menu with progress saved (if successful)
    }
}
