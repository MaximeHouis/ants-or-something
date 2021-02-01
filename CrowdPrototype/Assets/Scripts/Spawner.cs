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
    [CanBeNull] public List<GameObject> m_checkpoints;

    [Min(0)] public int m_count = 25;
    [Min(0)] public float m_range = 2.5f;
    [Min(0)] public float m_intervalMilliseconds = 25.0f;

    private void Start()
    {
        Spawn();
    }

    [ContextMenu("Call Spawn()")]
    public void Spawn()
    {
        if (Application.isPlaying)
            StartCoroutine(DoSpawn());
        else
            Debug.LogError("Cannot spawn entities as the game is not running.");
    }

    private IEnumerator DoSpawn()
    {
        for (var i = 0; i < m_count; i++)
        {
            var pos = transform.position;
            var offset = Utils.RandomPointInCircle(m_range);

            pos.x += offset.x;
            pos.z += offset.y;

            var entity = Instantiate(m_entity, pos, Quaternion.identity, transform);
            entity.name = "Ant #" + (i + 1);

            if (m_objective && m_entity.GetComponent<AntController>())
            {
                m_entity.GetComponent<AntController>().m_objective = m_objective;
            }

            yield return new WaitForSeconds(m_intervalMilliseconds / 1000.0f);
        }
    }
}