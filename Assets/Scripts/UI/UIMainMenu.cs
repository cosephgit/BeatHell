using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// handles the main menu system

public class UIMainMenu : MonoBehaviour
{
    [SerializeField]private GameObject screenMain;
    [SerializeField]private GameObject screenPlay;
    [SerializeField]private GameObject screenOptions;
    [SerializeField]private GameObject screenInstructions;
    [SerializeField]private GameObject screenQuit;
    [SerializeField]private Slider sliderVolume;
    [SerializeField]private Toggle toggleEpilepsy;
    [SerializeField]private UIScoreBoard scoreBoard;
    private bool mainMenu;

    void Awake()
    {
        mainMenu = (screenPlay != null);
        ButtonReturn();
    }

    void Start()
    {
        sliderVolume.value = GameManager.instance.volume;
        toggleEpilepsy.isOn = GameManager.instance.epilepsy;
        scoreBoard.ShowScores(-1);
    }

    public void ButtonPlay(System.Int32 slot)
    {
        GameManager.instance.SetActiveSlot(slot);
        SceneManager.LoadScene(1);
    }

    public void ButtonPlaySelect()
    {
        if (mainMenu) screenPlay.SetActive(true);
        screenMain.SetActive(false);
        screenOptions.SetActive(false);
        screenInstructions.SetActive(false);
        screenQuit.SetActive(false);
    }

    public void ButtonOptions()
    {
        if (mainMenu) screenPlay.SetActive(false);
        screenMain.SetActive(false);
        screenOptions.SetActive(true);
        screenInstructions.SetActive(false);
        screenQuit.SetActive(false);
    }

    public void ButtonInstructions()
    {
        if (mainMenu) screenPlay.SetActive(false);
        screenMain.SetActive(false);
        screenOptions.SetActive(false);
        screenInstructions.SetActive(true);
        screenQuit.SetActive(false);
    }

    public void ButtonReturn()
    {
        if (mainMenu) screenPlay.SetActive(false);
        screenMain.SetActive(true);
        screenOptions.SetActive(false);
        screenInstructions.SetActive(false);
        screenQuit.SetActive(false);
    }

    public void ButtonQuitMenu()
    {
        if (mainMenu) screenPlay.SetActive(false);
        screenMain.SetActive(false);
        screenOptions.SetActive(false);
        screenInstructions.SetActive(false);
        screenQuit.SetActive(true);
    }

    public void ButtonQuit()
    {
        #if UNITY_EDITOR
        Debug.Log("QUIT!");
        #else
        Application.Quit();
        #endif
    }

    public void SliderMusic(System.Single vol)
    {
        GameManager.instance.SetVolume(vol);
    }

    public void ToggleEpilepsy(System.Boolean isOn)
    {
        GameManager.instance.SetEpilepsy(isOn);
    }
}
