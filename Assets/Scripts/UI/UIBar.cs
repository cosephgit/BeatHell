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
    private float target;

    void Awake()
    {
        target = 0;
        slider.value = 0;
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
    }

    public void SetTarget(float targetNew)
    {
        target = Mathf.Clamp(targetNew, 0f, 1f);
    }    
}
