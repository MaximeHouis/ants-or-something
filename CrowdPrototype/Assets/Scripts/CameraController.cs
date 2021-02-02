using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 m_centerAnchor = Vector3.zero;
    public float m_sensitivity = 100.0f;
    public GameObject m_targetIndicator;

    private GameObject m_destinationIndicator;
    private Camera m_camera;
    private bool m_mouseGrabbed;

    private bool MouseGrabbed
    {
        set
        {
            if (m_mouseGrabbed == value)
                return;

            m_mouseGrabbed = value;

            Cursor.visible = !m_mouseGrabbed;
            Cursor.lockState = m_mouseGrabbed ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    private void Start()
    {
        m_camera = Camera.main;

        if (!m_camera)
            throw new NullReferenceException("No main camera");

        m_targetIndicator = Instantiate(m_targetIndicator, Vector3.zero, Quaternion.identity);
        m_targetIndicator.name = "Target";

        m_destinationIndicator = Instantiate(m_targetIndicator, Vector3.zero, Quaternion.identity);
        m_destinationIndicator.name = "Destination";
    }

    private void Update()
    {
        RotateCamera();
    }

    private void LateUpdate()
    {
        var ray = m_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            m_targetIndicator.SetActive(true);
            m_targetIndicator.transform.position = hit.point;

            if (Input.GetButtonDown("Fire1") && !m_mouseGrabbed)
            {
                SetDestination(hit.point);
            }
        }
        else
        {
            m_targetIndicator.SetActive(false);
        }
    }

    private void RotateCamera()
    {
        MouseGrabbed = Input.GetButton("Fire2");

        if (!m_mouseGrabbed)
            return;

        var rx = Input.GetAxis("Mouse X") * m_sensitivity * Time.deltaTime;

        if (rx == 0)
            return;

        transform.RotateAround(m_centerAnchor, Vector3.up, rx);
        transform.LookAt(m_centerAnchor);
    }

    private void SetDestination(Vector3 dest)
    {
        foreach (var ant in AntController.s_instances)
        {
            StartCoroutine(ant.SetDestination(dest));
        }

        m_destinationIndicator.transform.position = dest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, m_centerAnchor);
    }

    [ContextMenu("Look At Anchor")]
    public void LookAtAnchor()
    {
        transform.LookAt(m_centerAnchor);
    }
}