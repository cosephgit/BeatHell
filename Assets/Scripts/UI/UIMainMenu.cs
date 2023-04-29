using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// handles the main menu system

public class UIMainMenu : MonoBehaviour
{
    public void ButtonPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonOptions()
    {

    }

    public void ButtonQuit()
    {
        #if UNITY_EDITOR
        Debug.Log("QUIT!");
        #else
        Application.Quit();
        #endif
    }
}
