using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var tracker = GetComponent<CheckpointTracker>();

        if (!tracker)
            return;
        
    }

    private void OnDrawGizmos()
    {
        var collider = GetComponent<Collider>();

        if (!collider)
        {
            Debug.LogError("Failed to get collider");
            return;
        }

        Gizmos.color = Color.green * new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
    }
}
