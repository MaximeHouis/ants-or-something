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

    private IAntRacer m_racer;
    private AntAgent m_agent;
    private AntPlayer m_player;
    private uint m_index;
    private int m_lap;

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
        if (m_index == i)
            return;

        m_index = i;

        if (i == 0)
        {
            m_lap += 1;
            
            var count = CheckpointSystem.Instance.m_lapCount;
            var finished = m_lap == count;

            // TODO: Particle effect?
            StartCoroutine(finished ? m_racer.Finished() : m_racer.NewLap(m_lap + 1, count));
        }

        StartCoroutine(m_racer.NewCheckpoint(i));
    }
}