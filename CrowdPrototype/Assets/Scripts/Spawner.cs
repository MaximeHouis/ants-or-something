using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    public GameObject m_entity;

    [Min(0)]
    public int m_count = 25;

    [Min(0), InspectorName("Duration in seconds")]
    public float m_duration = 2.0f;

    private float Interval => m_count != 0 ? m_duration / m_count : 0f;
    private BoxCollider m_collider;

    private void Start()
    {
        m_collider = GetComponent<BoxCollider>();

        Spawn();
    }

    private void OnValidate()
    {
        m_collider = GetComponent<BoxCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + m_collider.center, m_collider.size);
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
            var pos = Utils.RandomPointInBox(m_collider);
            var entity = Instantiate(m_entity, pos, Quaternion.identity, transform);

            entity.name = "Ant #" + (i + 1);

            if (Interval != 0)
                yield return new WaitForSeconds(Interval);
        }
    }
}