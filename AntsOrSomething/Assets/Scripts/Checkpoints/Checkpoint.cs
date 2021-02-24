using UnityEngine;
using UnityEngine.Serialization;

#pragma warning disable CS0660
#pragma warning disable CS0661

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public Checkpoint Next;
    [HideInInspector] public BoxCollider Box;

    [FormerlySerializedAs("m_index")] public uint Index;

    private void Awake()
    {
        Box = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var tracker = other.GetComponent<CheckpointTracker>();

        if (tracker)
            tracker.SetIndex(Index);
    }

    private void OnDrawGizmos()
    {
        var col = GetComponent<BoxCollider>();

        if (!col)
        {
            Debug.LogError("Failed to get BoxCollider");
            return;
        }

        Gizmos.color = Color.green * new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
    }

    public static bool operator ==(Checkpoint a, Checkpoint b)
    {
        if (a is null && b is null)
            return true;
        if (a is null || b is null)
            return false;
        return a.Index == b.Index;
    }

    public static bool operator !=(Checkpoint a, Checkpoint b)
    {
        return !(a == b);
    }

    public static bool operator <(Checkpoint a, Checkpoint b)
    {
        return a.Index < b.Index;
    }

    public static bool operator >(Checkpoint a, Checkpoint b)
    {
        return a.Index > b.Index;
    }

    public static bool operator <=(Checkpoint a, Checkpoint b)
    {
        return a < b || a == b;
    }

    public static bool operator >=(Checkpoint a, Checkpoint b)
    {
        return a > b || a == b;
    }
}