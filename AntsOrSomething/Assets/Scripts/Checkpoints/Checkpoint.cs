using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public Checkpoint Next;

    public uint m_index;

    private void OnTriggerEnter(Collider other)
    {
        var tracker = GetComponent<CheckpointTracker>();

        if (!tracker)
            return;
    }

    private void OnDrawGizmos()
    {
        var col = GetComponent<Collider>();

        if (!col)
        {
            Debug.LogError("Failed to get collider");
            return;
        }

        Gizmos.color = Color.green * new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
    }

    public static bool operator <(Checkpoint a, Checkpoint b)
    {
        return a.m_index < b.m_index;
    }

    public static bool operator >(Checkpoint a, Checkpoint b)
    {
        return a.m_index > b.m_index;
    }
}