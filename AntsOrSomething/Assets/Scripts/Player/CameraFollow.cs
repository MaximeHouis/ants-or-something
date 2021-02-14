using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public GameObject m_target;

    [Header("Camera Settings")]
    [Min(0)] public float m_lagSpeed = 5f;

    public float m_distance = 5f;
    public Vector3 m_offset = new Vector3(0, 1, 0);

    private Camera m_camera;

    private Vector3 TargetPos => m_target.transform.position;

    public void Start()
    {
        m_camera = Camera.main;

        if (!m_camera)
            throw new NullReferenceException("Main camera not found");
    }

    public void Update()
    {
#if !UNITY_EDITOR
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
#endif
    }

    public void FixedUpdate()
    {
        UpdateCameraPosition();
    }

    [ContextMenu("Update Camera Position")]
    public void UpdateCameraPosition()
    {
        var position = transform.position;
        var targetPos = TargetPos + (m_offset * m_distance);
        var interpolation = Time.deltaTime * m_lagSpeed;

        if (m_lagSpeed == 0)
            interpolation = 1;

#if UNITY_EDITOR
        if (!Application.isPlaying)
            interpolation = 1;
#endif

        position.x = Mathf.Lerp(position.x, targetPos.x, interpolation);
        position.y = Mathf.Lerp(position.y, targetPos.y, interpolation);
        position.z = Mathf.Lerp(position.z, targetPos.z, interpolation);

        transform.position = position;
    }

    [ContextMenu("Look At Target")]
    public void LookAtTarget()
    {
        transform.LookAt(m_target.transform);
    }

    [ContextMenu("Update Pos & LookAt")]
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
}