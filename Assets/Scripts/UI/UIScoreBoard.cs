using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// show the high scores, highlighting the newly entered score if applicable

public class UIScoreBoard : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI scores;

    // show the scores
    // scoreNew is the index of which score is the most recent score
    public void ShowScores(int scoreNew)
    {
        string scoreString = "";

        for (int i = 0; i < Global.SCORESLOTS; i++)
        {
            if (i == scoreNew) scoreString += "<color=#FF00FF><b>";
            scoreString += (i + 1) + ": ";
            scoreString += GameManager.instance.scoresHigh[i];
            if (i == scoreNew) scoreString += "</b></color>";

            if (i < Global.SCORESLOTS - 1)
            {
                scoreString += "\n";
            }
        }

        scores.text = scoreString;
    }
}
