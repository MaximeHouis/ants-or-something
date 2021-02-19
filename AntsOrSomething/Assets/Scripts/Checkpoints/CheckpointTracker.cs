using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTracker : MonoBehaviour
{
    private static readonly List<Checkpoint> m_checkpointsOriginal = new List<Checkpoint>();

    private readonly Dictionary<Checkpoint, bool> m_checkpoints = new Dictionary<Checkpoint, bool>();

    private void Start()
    {
        if (m_checkpointsOriginal.Count == 0)
        {
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var root in roots)
            {
                var checkpoints = root.GetComponentsInChildren<Checkpoint>();

                foreach (var checkpoint in checkpoints)
                {
                    m_checkpointsOriginal.Add(checkpoint);
                }
            }
            
            Debug.Log("Added " + m_checkpointsOriginal.Count + " checkpoints");
        }

        foreach (var checkpoint in m_checkpointsOriginal)
        {
            m_checkpoints[checkpoint] = false;
        }
    }
}