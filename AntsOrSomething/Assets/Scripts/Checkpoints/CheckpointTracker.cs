using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTracker : MonoBehaviour
{
    // private static readonly List<Checkpoint> m_checkpointsOriginal = new List<Checkpoint>();
    //
    // private readonly Dictionary<Checkpoint, bool> m_checkpoints = new Dictionary<Checkpoint, bool>();

    // private void Start()
    // {
    //     if (m_checkpointsOriginal.Count == 0)
    //     {
    //         var roots = SceneManager.GetActiveScene().GetRootGameObjects();
    //
    //         foreach (var root in roots)
    //         {
    //             var checkpoints = root.GetComponentsInChildren<Checkpoint>();
    //
    //             foreach (var checkpoint in checkpoints)
    //             {
    //                 m_checkpointsOriginal.Add(checkpoint);
    //             }
    //         }
    //         
    //         Debug.Log("Registered " + m_checkpointsOriginal.Count + " checkpoints");
    //     }
    //
    //     foreach (var checkpoint in m_checkpointsOriginal)
    //     {
    //         m_checkpoints[checkpoint] = false;
    //     }
    // }

    private AntAgent m_agent;
    private AntPlayer m_player;
    private uint m_index;

    private void Awake()
    {
        m_agent = GetComponent<AntAgent>();
        m_player = GetComponent<AntPlayer>();

        if (m_agent && m_player)
            throw new MissingComponentException("AntAgent and AntPlayer are mutually exclusive");
    }

    public void SetIndex(uint i)
    {
        if (m_player && m_index == i)
            return;
        
        m_index = i;

        if (i == 0)
        {
            // TODO: Particle effect?
            NewLap();
        }

        NewCheckpoint();
    }

    private void NewLap()
    {
        if (m_player)
        {
            StartCoroutine(m_player.NewLap());
        }
    }

    private void NewCheckpoint()
    {
        if (m_agent)
        {
            m_agent.NextCheckpoint();
        }
    }
}