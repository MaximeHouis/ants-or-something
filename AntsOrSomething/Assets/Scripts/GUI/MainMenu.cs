using System;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2
}

public class MainMenu : MonoBehaviour
{
    public static Difficulty LocalDifficulty = Difficulty.Easy;

    public Dropdown m_difficultyDropdown;
    public Text m_versionText;

    private void Awake()
    {
        m_difficultyDropdown.value = (int) LocalDifficulty;
        m_difficultyDropdown.onValueChanged.AddListener(DifficultyChange);
        m_versionText.text = Application.version;
    }

    public void LoadMap(int index)
    {
        SceneLoader.Instance.LoadScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private static void DifficultyChange(int value)
    {
        switch (value)
        {
            case 0:
                LocalDifficulty = Difficulty.Easy;
                break;
            case 1:
                LocalDifficulty = Difficulty.Medium;
                break;
            case 2:
                LocalDifficulty = Difficulty.Hard;
                break;
            default:
                throw new NotImplementedException($"Difficulty {value} not implemented");
        }
    }
}