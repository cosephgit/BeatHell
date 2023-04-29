using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// displays the players score
// appears only the first time score is gained

public class UIScoreBox : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI scoreTitle;
    [SerializeField]private TextMeshProUGUI scoreNumber;
    [SerializeField]private Color colorBase = Color.white;
    [SerializeField]private Color colorPop = Color.yellow;
    [SerializeField]private float colorPopFadeRate = 1f;
    private float colorPopAmount;

    void Awake()
    {
        scoreTitle.color = Color.clear;
        scoreNumber.color = Color.clear;
        colorPopAmount = 0f;
    }

    public void UpdateScore(int score)
    {
        scoreNumber.text = score.ToString();
        colorPopAmount = 1f;
    }

    // update the current transparency each frame
    private void Update()
    {
        if (colorPopAmount > 0)
        {
            colorPopAmount -= Time.deltaTime * colorPopFadeRate;
            if (colorPopAmount <= 0)
            {
                colorPopAmount = 0;
            }

            Color colorFrame = Color.Lerp(colorBase, colorPop, colorPopAmount);

            scoreTitle.color = colorFrame;
            scoreNumber.color = colorFrame;
        }
    }
}
