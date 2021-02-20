using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Text m_versionText;

    private void Awake()
    {
        m_versionText.text = Application.version;
    }

    public void LoadMap(int index)
    {
        SceneManager.LoadScene(index);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
