using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    public static bool Active = true;

    [Header("Orbit settings")]
    public Transform m_center;

    public float m_radius = 20.0f;
    public float m_height = 20.0f;
    public float m_speed = 1.0f;
    public float m_offset;

    [Header("After Key Press Transition")]
    public Transform m_destination;

    public float m_durationSeconds = 1f;

    private bool m_inTransition;
    private Vector3 m_beforePos;
    private Quaternion m_beforeRot;

    private void Awake()
    {
        Active = true;
    }

    private void OnDestroy()
    {
        Active = false;
    }

    private void Update()
    {
        if (Active && !m_inTransition && Input.anyKeyDown)
        {
            m_inTransition = true;
            m_offset = 0;
            m_beforePos = transform.position;
            m_beforeRot = transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        if (!Active || !m_center)
            return;

        if (m_inTransition)
        {
            m_offset += Time.fixedDeltaTime;
            var interpolation = m_offset / m_durationSeconds;

            transform.position = Vector3.Lerp(m_beforePos, m_destination.position, interpolation);
            transform.rotation = Quaternion.Lerp(m_beforeRot, m_destination.rotation, interpolation);

            if (m_offset >= m_durationSeconds)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            var pos = m_center.transform.position;

            pos.y += m_height;
            pos.x += Mathf.Cos(m_offset) * m_radius;
            pos.z += Mathf.Sin(m_offset) * m_radius;

            transform.position = pos;
            transform.LookAt(m_center);

#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                m_offset += Time.fixedDeltaTime * m_speed;
        }
    }

    private void OnValidate()
    {
        FixedUpdate();
    }

    // private void OnDrawGizmosSelected()
    // {
    //     if (!m_center)
    //         return;
    //     
    //     var pos = m_center.position;
    //
    //     pos.y += m_height;
    //     Gizmos.DrawWireSphere();
    // }
}