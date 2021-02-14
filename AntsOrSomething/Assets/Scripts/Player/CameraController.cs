using System;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("General config")]
    public Transform m_centerAnchor;

    [Header("Controls")]
    public bool m_enabled = true;

    [Min(0)] public float m_speed = 100.0f;

    [Header("Other")] [Tooltip("0 == Unlimited")]
    [Delayed] [Min(0)] public int m_fpsLimit = 0;

    private Camera m_camera;
    private Vector3 m_originalPosition;

    private void Start()
    {
        m_camera = Camera.main;

        if (!m_camera)
            throw new NullReferenceException("No main camera assigned");

        m_originalPosition = transform.position;

        // m_targetIndicator = Instantiate(m_targetIndicator, Vector3.zero, Quaternion.identity);
        // m_targetIndicator.name = "Target";
        //
        // m_objectiveIndicator = Instantiate(m_objectiveIndicator, Vector3.zero, Quaternion.identity);
        // m_objectiveIndicator.transform.position = new Vector3(0, 10000, 0);

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

        if (Input.GetButtonDown("Reset"))
        {
            transform.position = m_originalPosition;
        }

        if (Input.GetMouseButtonDown(1))
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit))
                SetDestination(hit.point);
        }

        MoveCamera();
    }

    private void MoveCamera()
    {
        var speed = m_speed * Time.deltaTime;

        var dx = Input.GetAxis("Horizontal") * speed;
        var dy = Input.GetAxis("Vertical") * speed;
        var dh = Input.GetAxis("Height") * speed;

        if (dx != 0 || dy != 0)
        {
            transform.Translate(new Vector3(dx, 0, dy), Space.World);
        }

        if (dh != 0)
        {
            transform.Translate(new Vector3(0, dh, 0), Space.World);
        }
    }

    public void SetControlsEnabled(bool value)
    {
        m_enabled = value;
    }

    private void SetDestination(Vector3 dest)
    {
        foreach (var ant in AntAgent.s_instances.Where(a => a.m_selected))
        {
            StartCoroutine(ant.SetDestination(dest));
        }
    }

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;

        Gizmos.color = Color.grey;
        Gizmos.DrawLine(position, m_centerAnchor.transform.position);
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