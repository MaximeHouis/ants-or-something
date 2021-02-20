﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameUI.Instance.TogglePause();
    }

    public void MainMenu()
    {
        Resume();
        SceneManager.LoadScene(0);
    }
}