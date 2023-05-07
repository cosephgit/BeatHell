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
    [SerializeField]private Button buttonQuit;
    [SerializeField]private TextMeshProUGUI playText;
    [SerializeField]private TextMeshProUGUI titleText;
    [SerializeField]private GameObject scoreTitle;
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private GameObject scoreTotalTitle;
    [SerializeField]private TextMeshProUGUI scoreTotalText;
    private bool save;
    private int score;
    private int stage;

    public void SetStageEnd(bool success, int stageNew, int scoreNew)
    {
        save = success;
        if (success)
        {
            float scoreTotal = GameManager.instance.score[GameManager.instance.slotActive] + scoreNew;
            stage = stageNew;
            titleText.text = "STAGE " + (stage + 1) + " COMPLETE";
            scoreTitle.SetActive(true);
            scoreText.enabled = true;
            scoreText.text = scoreNew.ToString();
            scoreTotalTitle.SetActive(true);
            scoreTotalText.enabled = true;
            scoreTotalText.text = scoreTotal.ToString();
            buttonPlay.interactable = true;
            playText.color = Color.white;
            score = scoreNew;
            if (stage >= GameManager.instance.stageFinal)
                buttonQuit.interactable = false; // can't quit out if you finished the game!
        }
        else
        {
            titleText.text = "STAGE " + (stageNew + 1) + " FAILED";
            scoreTitle.SetActive(false);
            scoreText.enabled = false;
            scoreTotalTitle.SetActive(false);
            scoreTotalText.enabled = false;
            buttonPlay.interactable = false;
            playText.color = Color.gray;
            score = 0;
        }
    }

    public void ButtonNext()
    {
        Time.timeScale = 1f;
        if (stage >= GameManager.instance.stageFinal)
        {
            GameManager.instance.FinalStageComplete(score);
            SceneManager.LoadScene(2); // go to the ending!
        }
        else
        {
            GameManager.instance.ProgressStage(score);
            SceneManager.LoadScene(1); // reload scene, with new stage number stored in GameManager
        }
    }

    public void ButtonRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1); // just reload the scene without saving
    }

    public void ButtonQuit()
    {
        if (stage < GameManager.instance.stageFinal)
        {
            Time.timeScale = 1f;
            if (save)
                GameManager.instance.ProgressStage(score);
            SceneManager.LoadScene(0); // return to menu with progress saved (if successful)
        }
        else ButtonNext();
    }
}
