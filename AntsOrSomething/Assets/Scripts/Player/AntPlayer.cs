using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class AntPlayer : MonoBehaviour
{
    [Header("Player statistics")]
    [Min(0)] public float m_speed = 10f;

    private Rigidbody m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveAnt();
    }

    private void MoveAnt()
    {
        // No need for fixed delta time because we want an instant move
        
        var x = Input.GetAxis("Horizontal") * m_speed;
        var z = Input.GetAxis("Vertical") * m_speed;

        m_rigidbody.velocity = new Vector3(x, m_rigidbody.velocity.y, z);
    }
}