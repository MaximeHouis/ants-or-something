using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CameraController : MonoBehaviour
{
    public float m_speed = 2.5f;
    public float m_sensitivity = 10;
    [Range(0, 89)] public float m_rotationLockY = 89f;

    private bool m_mouseGrabbed = false;

    private void Start()
    {
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

    private void ToggleMouseGrab(bool? forcedValue = null)
    {
        m_mouseGrabbed = forcedValue ?? !m_mouseGrabbed;
        Cursor.visible = !m_mouseGrabbed;
        Cursor.lockState = m_mouseGrabbed ? CursorLockMode.Locked : CursorLockMode.None;
    }
}