using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public Text TimerText;
    public Text CountdownText;
    public GameObject m_pauseMenuPrefab;

    private AntPlayer m_antPlayer;
    private CheckpointTracker m_playerTracker;

    private bool m_paused;
    private GameObject m_pauseMenu;

    private void Start()
    {
        Instance = this;

        m_antPlayer = AntPlayer.Instance;
        m_playerTracker = m_antPlayer.GetComponent<CheckpointTracker>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Menu"))
            TogglePause();

        var reference = !m_playerTracker.Finished
            ? CheckpointSystem.Instance.Elapsed
            : m_playerTracker.CurrentTime;

        TimerText.text = "Time: " + reference.ToString("c");
    }

    public void TogglePause()
    {
        if (m_paused)
        {
            Destroy(m_pauseMenu);
        }
        else
        {
            m_pauseMenu = Instantiate(m_pauseMenuPrefab);
        }

        m_paused = !m_paused;
        Time.timeScale = m_paused ? 0f : 1f;
    }
}