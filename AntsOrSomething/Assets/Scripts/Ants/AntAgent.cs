using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class AntAgent : MonoBehaviour
{
    public static readonly List<AntAgent> s_instances = new List<AntAgent>();

    public bool m_selected;

    [HideInInspector] public NavMeshAgent m_agent;

    private MeshRenderer m_renderer;
    private bool m_touchedGround;
    private Vector3? m_destination = null;
    private AntClass m_class;

    private void Start()
    {
        s_instances.Add(this);

        m_agent = GetComponent<NavMeshAgent>();
        m_renderer = GetComponent<MeshRenderer>();

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

    private void OnCollisionEnter(Collision _)
    {
        if (m_touchedGround)
            return;

        m_touchedGround = true;
    }

    public void AssignClass(Dictionary<AntClass, float> ratios, uint index, uint count)
    {
        // ex: 0.2907795, 0.4834647, 0.8584071, 1

        var range = index / (double) count;

        foreach (var ratio in ratios.Where(ratio => range < ratio.Value))
        {
            m_class = ratio.Key;
            // StartCoroutine(SetColor());

            return;
        }

        throw new IndexOutOfRangeException("Ratio error");
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