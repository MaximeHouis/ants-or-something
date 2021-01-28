using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    public GameObject m_entity;
    [CanBeNull] public GameObject m_objective;
    
    [Min(0)] public int m_count = 25;
    [Min(0)] public float m_range = 2.5f;

    private GameObject m_group;
    private BoxCollider m_spawnBox;

    private void Start()
    {
        m_group = new GameObject("Ant Group");
        m_spawnBox = GetComponent<BoxCollider>();

        StartCoroutine(DoSpawn());
    }

    private IEnumerator DoSpawn()
    {
        for (var i = 0; i < m_count; i++)
        {
            var pos = transform.position;
            pos.x += Random.Range(-m_range, m_range);
            pos.z += Random.Range(-m_range, m_range);

            var entity = Instantiate(m_entity, pos, Quaternion.identity, m_group.transform);
            entity.name = "Ant #" + (i + 1);

            if (m_objective && m_entity.GetComponent<AntController>())
            {
                m_entity.GetComponent<AntController>().m_objective = m_objective;
            }

            yield return new WaitForSeconds(0.025f);
        }
    }
}