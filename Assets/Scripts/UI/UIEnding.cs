using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// manages the UI for the ending scene (basically: show score, scoreboard, and take continue button input)

public class UIEnding : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI difficultyText;
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private TextMeshProUGUI challengeText;
    [SerializeField]private UIScoreBoard scoreBoard;


    private void Awake()
    {
        
    }

    private void Start()
    {
        int newScore = GameManager.instance.score[GameManager.instance.slotActive];
        int newHighScore = GameManager.instance.NewFinalScore(newScore);
        int difficulty = GameManager.instance.difficulty[GameManager.instance.slotActive];

        scoreText.text = newScore.ToString();

        difficultyText.text = Global.ScoreName(difficulty);

        if (difficulty < 2)
        {
            challengeText.text = "BUT CAN YOU BEAT THE BEAT WHEN IT IS " + Global.ScoreName(difficulty + 1) + "?";
        }

        scoreBoard.ShowScores(newHighScore);
    }

    public void ButtonContinue()
    {
        SceneManager.LoadScene(0); // back to main menu
    }
}
