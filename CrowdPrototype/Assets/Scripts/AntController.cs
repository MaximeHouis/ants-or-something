using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AntController : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent m_agent;
    [CanBeNull] public GameObject m_objective;
    public List<GameObject> m_checkpoints;

    private bool m_touchedGround = false;
    
    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (m_touchedGround)
            return;

        m_touchedGround = true;

        if (!m_objective)
            return;

        // Wait for a bit before setting the objective
        StartCoroutine(SetDestination(m_objective.transform.position));
    }

    private IEnumerator SetDestination(Vector3 dest)
    {
        yield return new WaitForSeconds(1);
        m_agent.enabled = true;
        m_agent.SetDestination(dest);
    }
}