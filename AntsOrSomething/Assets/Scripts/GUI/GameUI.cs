using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public Text m_textTimer;
    public Text m_textLap;
    public Text m_textPosition;
    public Text m_textCountdown;
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
        var lapCount = CheckpointSystem.Instance.LapCount;

        m_textTimer.text = "Time: " + reference.ToString("c");
        m_textLap.text = "Lap:      " + $"{Math.Min(m_playerTracker.Lap + 1, lapCount)}/{lapCount}";
        m_textPosition.text = "Position: " +
                              $"{CheckpointTracker.Instances.IndexOf(m_playerTracker) + 1}/" +
                              $"{CheckpointSystem.Instance.EntityCount}";
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