using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Transform m_destination;
    
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = m_destination.position;
    }
}
