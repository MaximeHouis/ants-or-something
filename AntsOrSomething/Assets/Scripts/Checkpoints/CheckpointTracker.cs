using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    public static readonly List<CheckpointTracker> Instances = new List<CheckpointTracker>();

    private IAntRacer m_racer;
    private AntAgent m_agent;
    private AntPlayer m_player;

    [NonSerialized] public uint CheckpointIndex;
    [NonSerialized] public int Lap;
    [NonSerialized] public float Distance;

    [NonSerialized] public bool Finished;
    [NonSerialized] public TimeSpan CurrentTime;

    private void Awake()
    {
        Instances.Add(this);

        m_agent = GetComponent<AntAgent>();
        m_player = GetComponent<AntPlayer>();

        if (m_agent && m_player)
            throw new MissingComponentException("AntAgent and AntPlayer are mutually exclusive");

        m_racer = m_agent ? (IAntRacer) m_agent : m_player;

        m_racer.BeginRace();
    }

    private void OnDestroy()
    {
        Instances.Remove(this);
    }

    private void LateUpdate()
    {
        Distance = Vector3.Distance(transform.position, Next().transform.position);
    }

    public void SetIndex(uint i)
    {
        if (CheckpointIndex == i || Finished)
            return;

        CurrentTime = CheckpointSystem.Instance.Elapsed;
        CheckpointIndex = i;

        if (i == 0)
        {
            Lap += 1;

            var count = CheckpointSystem.Instance.LapCount;
            Finished = Lap == count;

            StartCoroutine(Finished ? m_racer.Finished() : m_racer.NewLap(Lap + 1, count));
        }

        StartCoroutine(m_racer.NewCheckpoint(i));
    }

    private Checkpoint Next()
    {
        var checkpoints = CheckpointSystem.Instance.Checkpoints;

        for (var i = 0; i < checkpoints.Count; i++)
        {
            if (i + 1 >= checkpoints.Count)
                return checkpoints[0];

            if (checkpoints[i].Index == CheckpointIndex)
                return checkpoints[i + 1];
        }

        throw new IndexOutOfRangeException();
    }
}