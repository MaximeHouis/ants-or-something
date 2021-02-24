using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingObject : MonoBehaviour
{
    public Vector3 m_offsetPosition = Vector3.zero;
    public Vector3 m_offsetRotation = Vector3.zero;
    public Vector3 m_offsetScale = Vector3.zero;
    public float m_duration = 1f;
    public float m_offset;
    public bool m_loop = true;

    private Vector3 m_fromPosition;
    private Vector3 m_fromRotation;
    private Vector3 m_fromScale;
    private Vector3 m_toPosition;
    private Vector3 m_toRotation;
    private Vector3 m_toScale;

    private Rigidbody m_rigidbody;

    private void Start()
    {
        RefreshData();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;
        m_offset += delta;

        var overflow = m_offset >= m_duration;

        if (overflow)
        {
            m_offset -= m_duration;

            if (m_loop)
            {
                Utils.Swap(ref m_fromPosition, ref m_toPosition);
                Utils.Swap(ref m_fromRotation, ref m_toRotation);
                Utils.Swap(ref m_fromScale, ref m_toScale);
            }
            else
            {
                RefreshData();
            }
        }

        var offset = m_offset / m_duration;

        m_rigidbody.MovePosition(Vector3.Lerp(m_fromPosition, m_toPosition, offset));
        m_rigidbody.MoveRotation(Quaternion.Euler(Vector3.Lerp(m_fromRotation, m_toRotation, offset)));
        transform.localScale = Vector3.Lerp(m_fromScale, m_toScale, offset);
    }

    private void RefreshData()
    {
        var transf = transform;

        m_fromPosition = transf.position;
        m_fromRotation = transf.eulerAngles;
        m_fromScale = transf.localScale;

        m_toPosition = m_fromPosition + m_offsetPosition;
        m_toRotation = m_fromRotation + m_offsetRotation;
        m_toScale = m_fromScale + m_offsetScale;
    }
}