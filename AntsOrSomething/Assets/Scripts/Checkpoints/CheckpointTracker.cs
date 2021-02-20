using System;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    private IAntRacer m_racer;
    private AntAgent m_agent;
    private AntPlayer m_player;
    private uint m_index;
    private int m_lap;

    [NonSerialized] public bool Finished;
    [NonSerialized] public TimeSpan CurrentTime;
    
    private void Awake()
    {
        m_agent = GetComponent<AntAgent>();
        m_player = GetComponent<AntPlayer>();

        if (m_agent && m_player)
            throw new MissingComponentException("AntAgent and AntPlayer are mutually exclusive");

        m_racer = m_agent ? (IAntRacer) m_agent : m_player;
        
        m_racer.BeginRace();
    }

    public void SetIndex(uint i)
    {
        if (m_index == i || Finished)
            return;

        CurrentTime = CheckpointSystem.Instance.Chrono.Elapsed;
        m_index = i;

        if (i == 0)
        {
            m_lap += 1;
            
            var count = CheckpointSystem.Instance.m_lapCount;
            Finished = m_lap == count;

            StartCoroutine(Finished ? m_racer.Finished() : m_racer.NewLap(m_lap + 1, count));
        }

        StartCoroutine(m_racer.NewCheckpoint(i));
    }
}