using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// manages the background pulse which is strengthened by music managers, and fades gradually over time
// kind of UI (just a visual, and may affect UI text) kind of not (not on a canvas object)

public class BackgroundPulse : MonoBehaviour
{
    public static BackgroundPulse instance;
    [SerializeField]private SpriteRenderer backgroundSprite;
    [SerializeField]private TextMeshProUGUI optionalText;
    [SerializeField]private float bgStrengthScale = 0.2f; // background opacity is strength times this
    [SerializeField]private float bgStrengthDecayMin = 2f;
    [SerializeField]private float bgStrengthDecayScale = 10f;
    [SerializeField]private Color bgColorBase = Color.red;
    [SerializeField]private Color bgColorText = Color.yellow;
    [SerializeField]private Color bgColorTextPulse = Color.green;
    private float bgStrength = 0f;

    private void Awake()
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
    }

    private void SetBGColor()
    {
        Color bgColor = bgColorBase;

        bgColor.a = bgStrength * bgStrengthScale;

        if (GameManager.instance.epilepsy) bgColor.a *= 0.3f; // reduced flash brightness in epilepsy-safe mode

        backgroundSprite.color = bgColor;

        if (optionalText)
        {
            optionalText.color = Color.Lerp(bgColorText, bgColorTextPulse, bgStrength);
        }
    }

    private void Update()
    {
        if (bgStrength > 0)
        {
            bgStrength = Mathf.Max(0f, Mathf.Min(bgStrength - Time.deltaTime * bgStrengthDecayMin, bgStrength - bgStrength * Time.deltaTime * bgStrengthDecayScale));

            SetBGColor();
        }
    }

    public void AddBeat(float strength)
    {
        bgStrength += strength;
        SetBGColor();
    }
}
