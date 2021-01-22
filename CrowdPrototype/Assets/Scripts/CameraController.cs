using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    public Vector3 m_center = new Vector3(0, 0, 0);
    public float m_radius = 20;
    public float m_height = 20;
    public float m_speed = 2.5f;

    private float m_offset = 0;
    private bool m_moved = false;

    private void Start()
    {
    }

    private void Awake()
    {
        m_moved = true;
    }

    private void OnValidate()
    {
        m_moved = true;
    }

    private void Update()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaY = Input.GetAxis("Vertical");

        if (deltaX != 0.0f)
        {
            m_moved = true;
            m_offset += deltaX * m_speed * Time.deltaTime;
        }

        if (deltaY != 0.0f)
        {
            //m_moved = true;
            //m_radius += deltaY * m_speed * 5.0f * Time.deltaTime;
            //m_radius = Mathf.Clamp(m_radius, 5, 50);
        }
    }

    private void LateUpdate()
    {
        if (m_moved)
        {
            m_moved = false;
            transform.position = new Vector3(
                Mathf.Cos(m_offset) * m_radius,
                m_height,
                Mathf.Sin(m_offset) * m_radius);
            transform.LookAt(m_center);
        }
    }
}
