using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AntClass
{
    Worker,
    Soldier,
    Breeder,
    Scout
}

[RequireComponent(typeof(NavMeshAgent), typeof(CheckpointTracker))]
public class AntAgent : MonoBehaviour
{
    public static readonly List<AntAgent> s_instances = new List<AntAgent>();

    public bool m_selected;

    [HideInInspector] public NavMeshAgent m_agent;

    private CheckpointTracker m_checkpointTracker;
    private MeshRenderer m_renderer;
    private bool m_touchedGround;
    private Vector3? m_destination;

    private void Start()
    {
        s_instances.Add(this);

        m_agent = GetComponent<NavMeshAgent>();
        m_renderer = GetComponent<MeshRenderer>();
        m_checkpointTracker = GetComponent<CheckpointTracker>();

        SetColor();
    }

    private void OnDestroy()
    {
        s_instances.Remove(this);
    }

    private void Update()
    {
        if (m_destination.HasValue && Vector3.Distance(transform.position, (Vector3) m_destination) <= 0.1f)
            m_agent.ResetPath();
    }

    public void StartRace()
    {
        
    }

    private void OnCollisionEnter(Collision _)
    {
        if (m_touchedGround)
            return;

        m_touchedGround = true;
    }

    public IEnumerator SetDestination(Vector3 dest)
    {
        while (!m_touchedGround)
            yield return new WaitForFixedUpdate();

        m_agent.enabled = true;
        m_agent.ResetPath(); // in case an old path was set
        m_agent.SetDestination(dest);

        m_destination = dest;
    }

    public void SetSelected(bool value)
    {
        m_selected = value;
    }

    private void SetColor()
    {
        var color = Random.ColorHSV(0, 1f, 1f, 1f, 0.5f, 0.75f);

        m_renderer.materials[0].color = color;
        m_renderer.materials[1].color = color * (2f / 3f);
    }
}