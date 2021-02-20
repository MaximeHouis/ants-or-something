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
public class AntAgent : MonoBehaviour, IAntRacer
{
    public static readonly List<AntAgent> s_instances = new List<AntAgent>();

    [HideInInspector] public NavMeshAgent m_agent;

    private CheckpointTracker m_checkpointTracker;
    private Checkpoint m_currentCheckpoint;
    private MeshRenderer m_renderer;
    private bool m_touchedGround;

    [Tooltip("Changing this has no effect, debug only")]
    public Vector3 m_destination;

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

    private IEnumerator GoToNextCheckpoint()
    {
        var next = m_currentCheckpoint ? m_currentCheckpoint.Next : CheckpointSystem.Instance.Checkpoints[1];

        yield return SetDestination(Utils.RandomPointInBox(next.Box));
        m_currentCheckpoint = next;
    }

    private void OnCollisionEnter()
    {
        if (m_touchedGround)
            return;

        m_touchedGround = true;
    }

    public IEnumerator BeginRace()
    {
        yield return GoToNextCheckpoint();
    }

    public IEnumerator Countdown()
    {
        yield break;
    }

    public IEnumerator NewCheckpoint(uint index)
    {
        yield return GoToNextCheckpoint();
    }

    public IEnumerator NewLap(int index, uint count)
    {
        yield break;
    }

    public IEnumerator Finished()
    {
        yield return new WaitForSeconds(0.5f);
        m_agent.enabled = false;
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

    private void SetColor()
    {
        var color = Random.ColorHSV(0, 1f, 1f, 1f, 0.5f, 0.75f);

        m_renderer.materials[0].color = color;
        m_renderer.materials[1].color = color * (2f / 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, m_destination);
    }
}