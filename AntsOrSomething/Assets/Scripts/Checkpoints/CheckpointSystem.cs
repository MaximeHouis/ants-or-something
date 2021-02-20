using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance;

    public List<Checkpoint> Checkpoints { get; } = new List<Checkpoint>();
    [NonSerialized] public readonly Stopwatch Chrono = new Stopwatch();

    [Header("Core")]
    public uint m_lapCount = 3;

    [Header("Finish Line")]
    public GameObject m_particles;

    private void Awake()
    {
        Instance = this;

        UpdateIndicesAndNames();
    }

    private IEnumerator Start()
    {
        foreach (var antAgent in AntAgent.s_instances)
        {
            StartCoroutine(antAgent.Countdown());
        }
        
        yield return AntPlayer.Instance.Countdown();
        
        Chrono.Start();
        FireParticles(Checkpoints[0].transform.position);
        StartCoroutine(AntPlayer.Instance.BeginRace());
        foreach (var antAgent in AntAgent.s_instances)
        {
            StartCoroutine(antAgent.BeginRace());
        }
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