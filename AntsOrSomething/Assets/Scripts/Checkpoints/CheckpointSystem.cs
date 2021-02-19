using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance;

    public List<Checkpoint> Checkpoints { get; } = new List<Checkpoint>();

    private void Awake()
    {
        Instance = this;

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
            Checkpoints[i].Index = (uint) i;
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