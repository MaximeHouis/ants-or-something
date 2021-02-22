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
    public Color m_firstColor;
    public Color m_lastColor;

    private AntPlayer m_antPlayer;
    private CheckpointTracker m_playerTracker;

    private bool m_paused;
    private GameObject m_pauseMenu;

    private void Start()
    {
        Instance = this;

        m_antPlayer = AntPlayer.Instance;
        m_playerTracker = m_antPlayer.GetComponent<CheckpointTracker>();

        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Menu"))
            TogglePause();

        var reference = !m_playerTracker.Finished
            ? CheckpointSystem.Instance.Elapsed
            : m_playerTracker.CurrentTime;
        var lapCount = CheckpointSystem.Instance.LapCount;
        var pos = CheckpointTracker.Instances.IndexOf(m_playerTracker) + 1;
        var total = CheckpointSystem.Instance.EntityCount;

        m_textTimer.text = "Time: " + reference.ToString("c");
        m_textLap.text = "Lap: " + $"{Math.Min(m_playerTracker.Lap + 1, lapCount)}/{lapCount}";
        m_textPosition.text = $"Position:\n{pos}/{total}";
        m_textPosition.color = Color.Lerp(m_firstColor, m_lastColor, pos / (float) total);
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
        Cursor.visible = m_paused;
    }
}