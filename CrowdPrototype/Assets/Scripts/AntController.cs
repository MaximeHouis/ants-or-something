using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AntController : MonoBehaviour
{
    public static readonly List<AntController> s_instances = new List<AntController>();

    [HideInInspector] public NavMeshAgent m_agent;

    private bool m_touchedGround = false;
    private Vector3? m_destination = null;
    private AntClass m_class;

    public AntClass Class
    {
        get => m_class;
        set
        {
            m_class = value;

            var color = value.Color();
            GetComponent<MeshRenderer>().materials[0].color = color;
            GetComponent<MeshRenderer>().materials[1].color += color;
        }
    }

    private void Start()
    {
        s_instances.Add(this);

        m_agent = GetComponent<NavMeshAgent>();
    }

    private void OnDestroy()
    {
        s_instances.Remove(this);
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
            yield return new WaitForSeconds(0.5f);

        m_agent.enabled = true;
        m_agent.ResetPath(); // in case an old path was set
        m_agent.SetDestination(dest);

        m_destination = dest;
    }
}