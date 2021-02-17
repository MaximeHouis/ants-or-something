using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform m_target;

    public Vector3 m_offset;

    [Header("Camera Settings")]
    [Min(0)] public float m_lagSpeed = 5f;

    private Camera m_camera;

    private Vector3 TargetPos => m_target.position;

    public void Start()
    {
        m_camera = Camera.main;

        if (!m_camera)
            throw new NullReferenceException("Main camera not found");

        UpdateCameraPosition();
    }

#if !UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (QualitySettings.vSyncCount == 0)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
        }
    }
#endif

    public void FixedUpdate()
    {
        UpdateCameraPosition();
    }

    [ContextMenu("Update Camera Position")]
    public void UpdateCameraPosition()
    {
        var delta = Time.fixedDeltaTime;
        var position = transform.position;
        var interpolation = delta * m_lagSpeed;

        if (m_lagSpeed == 0)
            interpolation = 1;

#if UNITY_EDITOR
        if (!Application.isPlaying)
            interpolation = 1;
#endif

        transform.position = Vector3.Lerp(position, TargetPos + m_offset, interpolation);
    }

    [ContextMenu("Look At Target")]
    public void LookAtTarget()
    {
        transform.LookAt(m_target);
    }

    [ContextMenu("Update Pos And LookAt")]
    public void UpdatePosAndLookAt()
    {
        UpdateCameraPosition();
        LookAtTarget();
    }

    private void OnValidate()
    {
        if (!m_target)
            return;
        UpdatePosAndLookAt();
    }

    private void OnDrawGizmosSelected()
    {
        if (m_target)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, m_target.position);
        }
    }
}