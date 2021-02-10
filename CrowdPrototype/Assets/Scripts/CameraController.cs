using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("General config")]
    public Vector3 m_centerAnchor = Vector3.zero;

    public GameObject m_targetIndicator;
    public GameObject m_objectiveIndicator;

    [Header("Controls")]
    public bool m_enabled = true;

    [Min(0)] public float m_sensitivity = 1.0f;
    [Min(0)] public float m_speed = 100.0f;
    [Range(0, 89.9f)] public float m_rotationLockX = 89.9f;

    [Header("Other")] [Tooltip("0 == Unlimited")]
    [Delayed] [Min(0)] public int m_fpsLimit = 0;

    private Camera m_camera;
    private Rigidbody m_rigidbody;

    private bool m_mouseGrabbed;
    private float m_rotationX;

    private bool MouseGrabbed
    {
        get => m_mouseGrabbed;
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
            throw new NullReferenceException("No main camera assigned");

        m_rigidbody = GetComponent<Rigidbody>();
        m_rotationX = -m_camera.transform.localEulerAngles.x;

        m_targetIndicator = Instantiate(m_targetIndicator, Vector3.zero, Quaternion.identity);
        m_targetIndicator.name = "Target";

        m_objectiveIndicator = Instantiate(m_objectiveIndicator, Vector3.zero, Quaternion.identity);

        SetFPS(m_fpsLimit);
    }

    private void OnValidate()
    {
        SetFPS(m_fpsLimit);
    }

    private void Update()
    {
        if (!m_enabled)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MouseGrabbed = !MouseGrabbed;
        }

        if (!MouseGrabbed && Input.GetMouseButtonDown(0))
        {
            MouseGrabbed = true;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
                SetDestination(hit.point);
        }

        MoveCamera();
        RotateCamera();
    }

    private void LateUpdate()
    {
        var ray = m_camera.ScreenPointToRay(Input.mousePosition);

        if (MouseGrabbed && Physics.Raycast(ray, out var hit))
        {
            m_targetIndicator.SetActive(true);
            m_targetIndicator.transform.position = hit.point;
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
        var sensitivity = m_sensitivity;
        var rx = Input.GetAxis("Mouse X") * sensitivity;
        var ry = Input.GetAxis("Mouse Y") * sensitivity;

        if (!m_mouseGrabbed || (rx == 0 && ry == 0))
            return;

        transform.Rotate(Vector3.up * rx, Space.World);

        var y = transform.localEulerAngles.y;

        m_rotationX += ry;
        m_rotationX = Mathf.Clamp(m_rotationX, -m_rotationLockX, m_rotationLockX);

        transform.localEulerAngles = new Vector3(-m_rotationX, y, 0);
    }

    public void SetControlsEnabled(bool value)
    {
        m_enabled = value;
        MouseGrabbed = true;
    }

    private void SetDestination(Vector3 dest)
    {
        foreach (var ant in AntController.s_instances)
        {
            StartCoroutine(ant.SetDestination(dest));
        }

        m_objectiveIndicator.transform.position = dest;
    }

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;

        Gizmos.color = Color.grey;
        Gizmos.DrawLine(position, m_centerAnchor);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, m_targetIndicator.transform.position);
    }

    [ContextMenu("Look At Anchor")]
    public void LookAtAnchor()
    {
        transform.LookAt(m_centerAnchor);
    }

    private static void SetFPS(int value)
    {
        if (value == 0)
            value = -1;

        Application.targetFrameRate = value;
    }
}