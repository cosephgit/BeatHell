using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// shows the wave indicator
// fades to transparency again after it is updated
// ALSO used as stage indicator

public class UIWaveMarker : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI waveTitle;
    [SerializeField]private TextMeshProUGUI waveNumber;
    [SerializeField]private float transFadeRate = 0.5f;
    [SerializeField]private Color colorBase = Color.white;
    private float opacity;
    private string stage = "";

    void Awake()
    {
        opacity = 0;
        waveTitle.color = Color.clear;
        waveNumber.color = Color.clear;
    }

    public void SetStage(int stageNew)
    {
        stage = (stageNew + 1) + " - ";
    }

    // updates the stage/wave indicator with the new stage/wave number
    public void UpdateWave(int wave, bool final = false)
    {
        // -2 and -1 are flags for stage states
        if (wave == -2)
        {
            waveNumber.text = "FAILED";
        }
        else if (wave == -1)
        {
            waveNumber.text = "COMPLETE";
        }
        else
        {
            string display;
            if (stage == "") // then this is a stage marker
                display = wave.ToString();
            else // then this is a wave marker
                display = stage + wave.ToString();

            if (final) display += " [FINAL]"; // final wave/stage indicator

            waveNumber.text = display;
        }
        opacity = 1f;
    }

    // update the current transparency each frame
    private void Update()
    {
        if (opacity > 0)
        {
            Color colorFrame = colorBase;
            colorFrame.a = opacity;
            opacity -= Time.deltaTime * transFadeRate;
            if (opacity <= 0)
            {
                opacity = 0;
            }
            waveTitle.color = colorFrame;
            waveNumber.color = colorFrame;
        }
    }
}
