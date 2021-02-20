using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public Text TimerText;
    public Text CountdownText;

    private AntPlayer m_antPlayer;
    private CheckpointTracker m_playerTracker;

    private void Start()
    {
        Instance = this;

        m_antPlayer = AntPlayer.Instance;
        m_playerTracker = m_antPlayer.GetComponent<CheckpointTracker>();
    }

    public void Update()
    {
        var reference = !m_playerTracker.Finished
            ? CheckpointSystem.Instance.Elapsed
            : m_playerTracker.CurrentTime;

        TimerText.text = "Time: " + reference.ToString("c");
    }
}