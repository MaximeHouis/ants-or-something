﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AntPlayer : MonoBehaviour
{
    [Header("Player statistics")]
    [Min(0)] public float m_speed = 10f;

    [Min(0)] public float m_rotationSpeed = 15f;

    private Rigidbody m_rigidbody;
    private Vector3 m_targetAngle = Vector3.zero;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveAnt();
        RotateAnt();
    }

    private void MoveAnt()
    {
        // No need for fixed delta time because we want an instant move
        
        var x = Input.GetAxis("Horizontal") * m_speed;
        var z = Input.GetAxis("Vertical") * m_speed;
        var velocity = new Vector3(x, m_rigidbody.velocity.y, z);

        m_rigidbody.velocity = velocity;

        if (x == 0 && z == 0)
            return;
        
        var localAngle = transform.localEulerAngles;
        localAngle.y = Mathf.Atan2(x, z) * Mathf.Rad2Deg;
        m_targetAngle = localAngle;
    }

    private void RotateAnt()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(m_targetAngle),
            Time.fixedDeltaTime * m_rotationSpeed);
    }
}