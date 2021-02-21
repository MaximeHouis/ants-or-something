using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance;

    public List<Checkpoint> Checkpoints { get; } = new List<Checkpoint>();

    public int EntityCount => CheckpointTracker.Instances.Count;

    public TimeSpan Elapsed => new TimeSpan((long) (m_elapsedTime * 1e7f)); // 10'000'000 ticks in 1 second

    [FormerlySerializedAs("m_lapCount")] [Header("Core")]
    public uint LapCount = 3;

    [Header("Finish Line")]
    public GameObject m_particles;

    private float m_elapsedTime;
    private bool m_started;

    private void Awake()
    {
        Instance = this;

        UpdateIndicesAndNames();
    }

    private IEnumerator Start()
    {
        InvokeRepeating(nameof(UpdatePositions), 0f, 0.2f);

        foreach (var antAgent in AntAgent.s_instances)
        {
            StartCoroutine(antAgent.Countdown());
        }

        yield return AntPlayer.Instance.Countdown();

        m_started = true;
        FireParticles(Checkpoints[0].transform.position);
        StartCoroutine(AntPlayer.Instance.BeginRace());
        foreach (var antAgent in AntAgent.s_instances)
        {
            StartCoroutine(antAgent.BeginRace());
        }
    }

    private void Update()
    {
        if (m_started)
            m_elapsedTime += Time.deltaTime;
    }

    private void UpdatePositions()
    {
        CheckpointTracker.Instances.Sort((a, b) =>
        {
            if (a.Finished && b.Finished)
                return a.CurrentTime.CompareTo(b.CurrentTime);
            if (a.Finished)
                return -1;
            if (b.Finished)
                return 1;

            var compare = b.Lap.CompareTo(a.Lap);
            if (compare != 0)
                return compare;

            compare = b.CheckpointIndex.CompareTo(a.CheckpointIndex);
            return compare != 0 ? compare : a.Distance.CompareTo(b.Distance);
        });
    }

    private void FireParticles(Vector3 position)
    {
        if (!m_particles)
            return;

        var system = Instantiate(m_particles, position, Quaternion.identity);

        Destroy(system, 7.5f);
    }

    [ContextMenu("UpdateIndicesAndNames()")]
    public void UpdateIndicesAndNames()
    {
        Checkpoints.Clear();

        foreach (var checkpoint in GetComponentsInChildren<Checkpoint>())
        {
            Checkpoints.Add(checkpoint);
        }

        if (Checkpoints.Count < 2)
            throw new ArgumentOutOfRangeException($"At least 2 checkpoints are needed");

        Checkpoints.Sort((a, b) => a.Index > b.Index ? 1 : -1);

        var size = Checkpoints.Count;
        for (var i = 0; i < size; i++)
        {
            var index = i + 1 < size ? (uint) i : uint.MaxValue;

            Checkpoints[i].Index = index;
            Checkpoints[i].name = $"Checkpoint {index}";
            Checkpoints[i].Next = i + 1 < size ? Checkpoints[i + 1] : Checkpoints[0];
        }
    }

    private void OnDrawGizmosSelected()
    {
        var size = Checkpoints.Count;
        Gizmos.color = Color.green * new Color(0.75f, 0.75f, 0.75f, 1f);

        for (var i = 0; i < size; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawLine(Checkpoints[size - 1].transform.position,
                    Checkpoints[0].transform.position);
                continue;
            }

            Gizmos.DrawLine(Checkpoints[i - 1].transform.position, Checkpoints[i].transform.position);
        }
    }
}