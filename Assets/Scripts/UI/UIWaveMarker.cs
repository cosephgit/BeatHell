using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// shows the wave indicator
// fades to transparency again after it is updated

public class UIWaveMarker : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI waveTitle;
    [SerializeField]private TextMeshProUGUI waveNumber;
    [SerializeField]private float transFadeRate = 0.5f;
    [SerializeField]private Color colorBase = Color.white;
    private float opacity;

    void Awake()
    {
        opacity = 0;
        waveTitle.color = Color.clear;
        waveNumber.color = Color.clear;
    }

    public void UpdateWave(int wave)
    {
        if (wave == -2)
        {
            waveTitle.text = "STAGE";
            waveNumber.text = "FAILED";
        }
        else if (wave == -1)
        {
            waveTitle.text = "STAGE";
            waveNumber.text = "COMPLETE";
        }
        else
        {
            waveTitle.text = "WAVE";
            waveNumber.text = wave.ToString();
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
