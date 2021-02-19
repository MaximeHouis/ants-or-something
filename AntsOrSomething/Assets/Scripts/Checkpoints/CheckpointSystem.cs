using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CheckpointSystem : MonoBehaviour
{
    private readonly List<Checkpoint> m_checkpoints = new List<Checkpoint>();

    private void Awake()
    {
        foreach (var checkpoint in GetComponentsInChildren<Checkpoint>())
        {
            m_checkpoints.Add(checkpoint);
        }

        if (m_checkpoints.Count < 2)
            throw new ArgumentOutOfRangeException($"At least 2 checkpoints are needed");

        m_checkpoints.Sort((a, b) => a.m_index > b.m_index ? 1 : -1);

        var size = m_checkpoints.Count;
        for (var i = 0; i < size; i++)
        {
            m_checkpoints[i].Next = i + 1 < size ? m_checkpoints[i + 1] : m_checkpoints[0];
        }
    }

    private void OnDrawGizmosSelected()
    {
        var size = m_checkpoints.Count;
        Gizmos.color = Color.green * new Color(0.75f, 0.75f, 0.75f, 1f);

        for (var i = 0; i < size; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawLine(m_checkpoints[size - 1].transform.position,
                    m_checkpoints[0].transform.position);
                continue;
            }

            Gizmos.DrawLine(m_checkpoints[i - 1].transform.position, m_checkpoints[i].transform.position);
        }
    }
}