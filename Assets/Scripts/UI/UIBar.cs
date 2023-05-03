using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UIBar manages sliders in the UI, takes new values for them and quickly and smoothly transitions to the new value

public class UIBar : MonoBehaviour
{
    [SerializeField]private Slider slider;
    [SerializeField]private float sliderChangeRate = 1f;
    [SerializeField]private Color colorDown = Color.red;
    [SerializeField]private Color colorUp = Color.cyan;
    [SerializeField]private Image colorFlash;
    [SerializeField]private float colorFlashDecay = 2f; // how quickly the color flash fades away
    private float target;
    private float colorFlashAmount;

    void Awake()
    {
        target = 0;
        slider.value = 0;
        colorFlashAmount = 0f;
    }

    void Update()
    {
        if (slider.value < target)
        {
            slider.value = Mathf.Min(slider.value + (Time.deltaTime * sliderChangeRate), target);
        }
        else if (slider.value > target)
        {
            slider.value = Mathf.Max(slider.value - (Time.deltaTime * sliderChangeRate), target);
        }

        if (colorFlashAmount < 0)
        {
            colorFlashAmount = Mathf.Min(0, colorFlashAmount + Time.deltaTime * colorFlashDecay);
            colorFlash.color = Color.Lerp(Color.clear, colorDown, -colorFlashAmount);
        }
        else if (colorFlashAmount > 0)
        {
            colorFlashAmount = Mathf.Max(0, colorFlashAmount - Time.deltaTime * colorFlashDecay);
            colorFlash.color = Color.Lerp(Color.clear, colorUp, colorFlashAmount);
        }
    }

    public void SetTarget(float targetNew)
    {
        target = Mathf.Clamp(targetNew, 0f, 1f);
        if (Mathf.Approximately(target, slider.value))
        { }
        else if (target < slider.value)
        {
            colorFlashAmount = -1f;
        }
        else if (target > slider.value)
        {
            colorFlashAmount = 1f;
        }
    }    
}
