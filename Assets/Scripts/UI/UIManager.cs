using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]private UIBar healthBar;
    [field: SerializeField]public UIMagazine magazine { get; private set; }
    [field: SerializeField]public UIWaveMarker waveMarker { get; private set; }
    [field: SerializeField]public UIScoreBox scoreBox { get; private set; }
    [field: SerializeField]public UIInGameMenu inGameMenu { get; private set; }
    [field: SerializeField]public UIWaveMarker stageBox { get; private set; }

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

    public void UpdateHealth(float healthPart)
    {
        healthBar.SetTarget(healthPart);
    }
}
