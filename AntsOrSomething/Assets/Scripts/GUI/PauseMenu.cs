using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameUI.Instance.TogglePause();
    }

    public void Restart()
    {
        Resume();
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Resume();
        SceneLoader.Instance.LoadScene(0);
    }
}