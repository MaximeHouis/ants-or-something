using UnityEngine;

public class RandomScale : MonoBehaviour
{
    [Header("Scale Value")]
    [Min(0)] public float m_Minimum = 0.2f;

    [Min(0)] public float m_Maximum = 1.0f;

    private void Start()
    {
        transform.localScale = Random.Range(m_Minimum, m_Maximum) * Vector3.one;
    }

#if UNITY_EDITOR
    [Header("Debug")]
    public bool m_showGizmos;

    private void OnDrawGizmos()
    {
        if (!m_showGizmos)
            return;

        var color1 = Color.white;
        var color2 = Color.cyan;
        var color3 = Color.grey;

        var meshFilter = GetComponent<MeshFilter>();
        var position = transform.position;

        if (meshFilter)
        {
            var rotation = transform.rotation;
            var mesh = meshFilter.sharedMesh;

            Gizmos.color = color1;
            Gizmos.DrawWireMesh(mesh, position, rotation, Vector3.one * m_Minimum);

            // Gizmos.color = color2;
            // Gizmos.DrawWireMesh(mesh, position, rotation, Vector3.one);

            Gizmos.color = color3;
            Gizmos.DrawWireMesh(mesh, position, rotation, Vector3.one * m_Maximum);
        }
        else
        {
            Gizmos.color = color1;
            Gizmos.DrawWireCube(position, Vector3.one * m_Minimum);

            Gizmos.color = color2;
            Gizmos.DrawWireCube(position, Vector3.one);

            Gizmos.color = color3;
            Gizmos.DrawWireCube(position, Vector3.one * m_Maximum);
        }
    }
#endif
}