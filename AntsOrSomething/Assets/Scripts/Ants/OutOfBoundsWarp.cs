using UnityEngine;

public class OutOfBoundsWarp : MonoBehaviour
{
    public Transform m_respawnPoint;
    public float m_yWarpLimit = -42f;

    private void FixedUpdate()
    {
        if (transform.position.y <= m_yWarpLimit)
            transform.position = m_respawnPoint.position;
    }
}