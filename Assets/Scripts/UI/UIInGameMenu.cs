using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGameMenu : UIMainMenu
{
    public void Open()
    {
        ButtonReturn();
    }

    public void ButtonResume()
    {
        StageManager.instance.MenuClose();
    }

    public void ButtonQuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
