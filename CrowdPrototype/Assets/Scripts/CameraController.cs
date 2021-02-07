using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum Mode
    {
        Free,
        Orbit
    }

    public Mode m_mode = Mode.Free;
    public Vector3 m_centerAnchor = Vector3.zero;
    public GameObject m_targetIndicator;
    public GameObject m_objectiveIndicator;

    [Space(5)]
    [Min(0)] public float m_sensitivity = 1.0f;

    [Min(0)] public float m_speed = 100.0f;
    [Range(0, 89.9f)] public float m_rotationLockX = 89.9f;

    [Tooltip("0 == Unlimited")] [Delayed] [Min(0)]
    public int m_fpsLimit = 0;

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

    private bool CanRaycast
    {
        get
        {
            switch (m_mode)
            {
                case Mode.Free:
                    return m_mouseGrabbed;
                case Mode.Orbit:
                    return !m_mouseGrabbed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        // if (m_mode == Mode.Free)
        //     MouseGrabbed = true;

        SetFPS(m_fpsLimit);
    }

    private void OnValidate()
    {
        SetFPS(m_fpsLimit);
    }

    private void Update()
    {
        MoveCamera();
        RotateCamera();

        if (m_mode == Mode.Free)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MouseGrabbed = !MouseGrabbed;
            }

            if (!MouseGrabbed && Input.GetMouseButtonDown(0))
            {
                MouseGrabbed = true;
            }
        }
    }

    private void LateUpdate()
    {
        var ray = m_camera.ScreenPointToRay(Input.mousePosition);

        if (CanRaycast && Physics.Raycast(ray, out var hit))
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
        var moved = false;

        if (dx != 0 || dy != 0)
        {
            transform.Translate(new Vector3(dx, 0, dy), Space.Self);
            moved = true;
        }

        if (dh != 0)
        {
            transform.Translate(new Vector3(0, dh, 0), Space.World);
            moved = true;
        }

        if (m_mode == Mode.Orbit && moved)
            transform.LookAt(m_centerAnchor);
    }

    private void RotateCamera()
    {
        var sensitivity = m_sensitivity;
        var rx = Input.GetAxis("Mouse X") * sensitivity;
        var ry = Input.GetAxis("Mouse Y") * sensitivity;

        switch (m_mode)
        {
            case Mode.Free:
                RotateCameraFree(rx, ry);
                break;
            case Mode.Orbit:
                RotateCameraOrbit(rx, ry);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RotateCameraFree(float rx, float ry)
    {
        if (!m_mouseGrabbed || (rx == 0 && ry == 0))
            return;

        transform.Rotate(Vector3.up * rx, Space.World);

        var y = transform.localEulerAngles.y;

        m_rotationX += ry;
        m_rotationX = Mathf.Clamp(m_rotationX, -m_rotationLockX, m_rotationLockX);

        transform.localEulerAngles = new Vector3(-m_rotationX, y, 0);
    }

    private void RotateCameraOrbit(float rx, float ry)
    {
        MouseGrabbed = Input.GetButton("Fire2");

        if (!m_mouseGrabbed)
            return;

        if (rx != 0)
            transform.RotateAround(m_centerAnchor, Vector3.up, rx);

        if (rx != 0 || ry != 0)
            transform.LookAt(m_centerAnchor);
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
        Gizmos.DrawLine(transform.position, m_centerAnchor);
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