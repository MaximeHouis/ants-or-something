using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject m_targetIndicator;
    public float m_speed = 2.5f;
    public float m_sensitivity = 10;
    [Range(0, 89.9f)] public float m_rotationLockY = 89.9f;

    private GameObject m_destinationIndicator;
    private Camera m_camera;
    private bool m_mouseGrabbed = false;

    private void Start()
    {
        m_camera = Camera.main;

        m_targetIndicator = Instantiate(m_targetIndicator, Vector3.zero, Quaternion.identity);
        m_targetIndicator.name = "Target";

        m_destinationIndicator = Instantiate(m_targetIndicator, Vector3.zero, Quaternion.identity);
        m_destinationIndicator.name = "Destination";

        ToggleMouseGrab();
    }

    private void Update()
    {
        MoveCamera();
        RotateCamera();

        if (Input.GetKeyUp(KeyCode.Escape))
            ToggleMouseGrab();

#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0) && !m_mouseGrabbed)
        {
            ToggleMouseGrab(true);
        }
#endif
    }

    private void LateUpdate()
    {
        var ray = m_camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out var hit))
        {
            m_targetIndicator.SetActive(true);
            m_targetIndicator.transform.position = hit.point;

            if (Input.GetButtonDown("Fire1"))
            {
                SetDestination(hit.point);
            }
        }
        else
        {
            m_targetIndicator.SetActive(false);
        }
    }

    private void MoveCamera()
    {
        var speed = m_speed * Time.deltaTime;

        var dx = Input.GetAxis("Horizontal") * speed;
        var dy = Input.GetAxis("Vertical") * speed;
        var dh = Input.GetAxis("Height") * speed;

        if (dx != 0 || dy != 0)
        {
            transform.Translate(new Vector3(dx, 0, dy), Space.Self);
        }

        if (dh != 0)
        {
            transform.Translate(new Vector3(0, dh, 0), Space.World);
        }
    }

    private void RotateCamera()
    {
        if (!m_mouseGrabbed)
            return;

        var sensitivity = m_sensitivity * Time.deltaTime;
        var localRotation = transform.localEulerAngles;
        var rx = localRotation.y + Input.GetAxis("Mouse X") * sensitivity;
        var ry = localRotation.x - Input.GetAxis("Mouse Y") * sensitivity;

        transform.localEulerAngles = new Vector3(ry, rx, 0.0f);
    }

    private void SetDestination(Vector3 dest)
    {
        foreach (var ant in AntController.s_instances)
        {
            StartCoroutine(ant.SetDestination(dest));
        }

        m_destinationIndicator.transform.position = dest;
    }

    private void ToggleMouseGrab(bool? forcedValue = null)
    {
        m_mouseGrabbed = forcedValue ?? !m_mouseGrabbed;
        Cursor.visible = !m_mouseGrabbed;
        Cursor.lockState = m_mouseGrabbed ? CursorLockMode.Locked : CursorLockMode.None;
    }
}