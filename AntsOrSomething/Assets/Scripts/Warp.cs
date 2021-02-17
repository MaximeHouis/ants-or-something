using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Transform m_destination;
    
    private IEnumerator OnTriggerEnter(Collider other)
    {
        yield return new WaitForSeconds(0.1f);

        other.transform.position = m_destination.position;
    }
}
